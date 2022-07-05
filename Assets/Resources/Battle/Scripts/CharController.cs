using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


namespace Overlewd
{
    public class CharController : MonoBehaviour
    {

        public bool isEnemy = false;
        public bool isBoss = false;
        public bool isOverlord = false;

        public Button bt;
        public CharacterPortrait charStats;

        private GameObject border;

        public CharacterStatObserver observer;

        public BattleManager bm; //init on BattleManager Initialize();
        public GameObject popUpPrefab;

        public CharacterRes characterRes;
        public Skill[] skillRes => characterRes.skill;
        public AdminBRO.Character character;

        public List<AdminBRO.CharacterSkill> skill = new List<AdminBRO.CharacterSkill>();// => character.skills;
        public List<AdminBRO.CharacterSkill> passiveSkill = new List<AdminBRO.CharacterSkill>();


        public string Name => character.name;
        public float speed => (float)character.speed;
        public float power => (float)character.power;
        public float constitution => (float)character.constitution;
        public float agility => (float)character.agility;
        public float accuracy => (float)character.accuracy;
        public float dodge => (float)character.dodge;
        public float critrate => (float)character.critrate;
        public float damage => (float)character.damage;

        public float damageTotal = 10;


        private float idleScale = 1f, battleScale = 1.4f;
        public int battleOrder = 1;
        public float health = 100, healthMax = 100;
        public float mana = 100, manaMax = 100;

        [HideInInspector] public bool isDamageBuff = false;
        [HideInInspector] public int buffDamageScale = 1;

        public bool isDead = false;

        private Transform battleLayer;
        public Transform persPos;
        private Transform battlePos;
        private SpineWidget spineWidget;
        private float defenceDuration = 1f;
        public float
            preAttackDuration = 0.9f,
            attackDuration = 1f,
            vfxDuration = 0f;
        private RectTransform rt;

        public int
            focus_blind, defUp_defDown,
            regen_poison, bless_healBlock,
            immunity, silence, curse;
        public bool stun;

        [Tooltip("Reduce or add total damage in percentage up to 100")]
        public float defUp_defDown_dot = 0f;
        [Tooltip("(-) Heal or (+) Damage as a percentage of maximum health at the end of the round")]
        public float regen_poison_dot = 0f;
        [Tooltip("Heal Up damage in percentage up to 100")]
        public float bless_dot = 0f;
        [Tooltip("Total damage reduced by a percentage")]
        public float curse_dot = 0f;

        private string
            msg_immunity = "Immunity",
            msg_defence_down = "Defence down",
            msg_poison = "Poison",
            msg_defence_up = "Defence up",
            msg_stun = "Stun";

        private void Start()
        {
            StatInit();
            ShapeInit();
            UIInit();
            PlayIdle();
            StartCoroutine(LateInit());
        }

        private void StatInit()
        {
            //isEnemy and battleOrder assign on battle manager
            isOverlord = character.characterClass == AdminBRO.Character.Class_Overlord;
            if (isOverlord) bm.overlord = this;
            health = (float)character.health;
            healthMax = health;
            mana = (float)character.mana;
            manaMax = mana;
            idleScale = characterRes.idleScale;
            battleScale = characterRes.battleScale;

            skill = character.skills;
            if (!isOverlord)
            {
                passiveSkill.Add(skill?.Find(f => f.type == "passive_skill"));
                skill = skill.Except(passiveSkill).ToList();
            }
            else
            {
                passiveSkill.Add(skill?.Find(f => f.type == "overlord_first_passive_skill"));
                passiveSkill.Add(skill?.Find(f => f.type == "overlord_second_passive_skill"));
                skill = skill.Except(passiveSkill).ToList();
            }
        }
        private void ShapeInit()
        {
            rt = gameObject.AddComponent<RectTransform>();
            UITools.SetStretch(rt);
            rt.localScale *= idleScale;

            Transform spawnPos = bm.transform;
            battleLayer = spawnPos.Find("BattleCanvas/BattleLayer").transform;
            if (isEnemy)
            {
                if (isBoss)
                {
                    battlePos = battleLayer.Find("battlePosBoss").transform;
                    persPos = battleLayer.Find("boss").transform;
                }
                else
                {
                    battlePos = battleLayer.Find("battlePos2").transform;
                    persPos = battleLayer.Find("enemy" + battleOrder.ToString()).transform;
                }
            }
            else
            {
                battlePos = battleLayer.Find("battlePos1").transform;
                persPos = battleLayer.Find("pers" + battleOrder.ToString()).transform;
            }

            transform.SetParent(persPos, false);
            transform.SetSiblingIndex(0);
            //Drop GUI and Observer
            GameObject observerGO;
            if (isEnemy && !isBoss)
                observerGO = Resources.Load("Battle/Prefabs/CharGUIEnemy") as GameObject;
            else if (isBoss)
                observerGO = Resources.Load("Battle/Prefabs/CharGUIBoss") as GameObject;
            else
                observerGO = Resources.Load("Battle/Prefabs/CharGUI") as GameObject;

            observer = Instantiate(observerGO, persPos).GetComponent<CharacterStatObserver>();
            observer.cc = this;
            observer.charStats = charStats;

            var sW = characterRes.characterPrefab ?
                SpineWidget.GetInstance(characterRes.characterPrefab, transform) :
                SpineWidget.GetInstance(characterRes.idle_Prefab_Path, transform);
            spineWidget = sW;
            defenceDuration = spineWidget.GetAnimationDuaration(characterRes.ani_defence_name);
        }
        private void UIInit()
        {
            popUpPrefab = Resources.Load("Battle/Prefabs/BattlePopupText") as GameObject;

            charStats.cc = this;
            charStats.InitUI();

            bt = persPos.Find("button").GetComponent<Button>();
            bt.onClick.AddListener(Select);

            border = persPos.Find("button/border").gameObject;
            border.SetActive(false);
        }
        IEnumerator LateInit()
        {
            yield return new WaitForSeconds(0.01f);
            bt.gameObject.SetActive(true);
        }


        public void PowerBuff() => buffDamageScale *= 2;
        public void CharPortraitSet() => charStats.SetUI(this);
        public void UpdateUI() => observer?.UpdateUI();

        public void PlayIdle()
        {
            if (!isDead)
                spineWidget.PlayAnimation(characterRes.ani_idle_name, true);
        }

        public void Attack(int attackID, CharController target = null)
        {
            BattleIn();
            if (target != null) target.BattleIn();
            StartCoroutine(PlayAttack(attackID, target));
        }

        IEnumerator PlayAttack(int id, CharController target = null)
        {
            if (id >= skill.Count) id = 0; //if id overflow on skill array

            vfxDuration = characterRes.skill[id].vfxDuration;
            attackDuration = spineWidget.GetAnimationDuaration(characterRes.ani_attack_name[id]);

            foreach (var ps in passiveSkill)
            {
                if (ps.trigger == "on_attack")
                    if (ps.actionType == "heal")
                        PassiveBuff(ps);
            }

            var curseScale = curse > 0 ? curse_dot : 1f;
            damageTotal = Mathf.RoundToInt(damage * ((float)skill[id].amount / 100f) * buffDamageScale);
            damageTotal *= curseScale;

            if (characterRes.ani_pAttack_name[id] == "")
                preAttackDuration = 0f;
            else
            {
                preAttackDuration = spineWidget.GetAnimationDuaration(characterRes.ani_pAttack_name[id]);  //send to target Defence Animation
                spineWidget.PlayAnimation(characterRes.ani_pAttack_name[id], false);
            }

            if (isOverlord) mana -= skill[id].manaCost; //????
            SoundManager.PlayOneShot(skillRes[id].sfx);             //SFX
            yield return new WaitForSeconds(preAttackDuration);
            if (characterRes.skill[id].vfx != null)
            {
                Instantiate(characterRes.skill[id].vfx, battleLayer);
                yield return new WaitForSeconds(vfxDuration);
            }
            foreach (var ps in passiveSkill)
            {
                if (ps.trigger == "on_attack")
                    if (ps.actionType == "damage")
                        PassiveDeBuff(ps, target);
            }
            
            spineWidget.PlayAnimation(characterRes.ani_attack_name[id], false);
            yield return new WaitForSeconds(attackDuration);
            PlayIdle();
            BattleOut();
            bm.BattleOut();
        }

        public void Defence(CharController attacker, int id, GameObject vfx = null, bool aoe = false) => StartCoroutine(PlayDefence(attacker, id, vfx, aoe));

        IEnumerator PlayDefence(CharController attacker, int id, GameObject vfx = null, bool aoe = false)
        {
            transform.SetParent(battlePos);
            UnHiglight();
            yield return new WaitForSeconds(attacker.preAttackDuration + attacker.vfxDuration);
            if (vfx != null) Instantiate(vfx, transform);
            spineWidget.PlayAnimation(characterRes.ani_defence_name, false);
            yield return new WaitForSeconds(defenceDuration / 2);

            bool hit = attacker.accuracy > Random.value;
            bool isDodge = dodge > Random.value;
            bool isCrit = attacker.critrate > Random.value;

            if (attacker.focus_blind != 0)
                hit = attacker.focus_blind > 0 ? true : false;

            if (hit) AddEffect(attacker.skill[id]);

            foreach (var ps in passiveSkill)
            {
                if (ps.trigger == "when_attacked")
                    if (ps.actionType == "heal")
                        PassiveBuff(ps);
                    else
                        PassiveDeBuff(ps, attacker);
            }

            Debug.Log($"attacker.damage: {attacker.damageTotal}");
            Debug.Log($"hit {hit}, attacker.accuracy: {attacker.accuracy}");
            Debug.Log($"dodge {isDodge}, attacker.dodge: {dodge}");
            Debug.Log($"crit {isCrit}, attacker.critrate: {attacker.critrate}");

            float defScale = defUp_defDown != 0 ? attacker.damageTotal * defUp_defDown_dot * -Mathf.Sign(defUp_defDown) : 0f; //defence up down
            Damage(attacker.damageTotal + defScale, hit, isDodge, isCrit);

            yield return new WaitForSeconds(defenceDuration / 2);
            PlayIdle();
            BattleOut();
        }

        public void Select()
        {
            if (bm.battleState == BattleManager.BattleState.PLAYER && !isDead)
            {
                if (bm.CharPress(this))
                {
                    bm.unselect?.Invoke();
                    Highlight();
                }
                if (!isEnemy)
                    CharPortraitSet();
                SoundManager.PlayOneShot(FMODEventPath.UI_Battle_SelectPers);
            }
        }

        public void Highlight() => border?.SetActive(true);
        public void UnHiglight() => border?.SetActive(false);

        public void BattleIn()
        {
            UnHiglight();
            transform.SetParent(battlePos);
            rt.DOAnchorPos(Vector2.zero, 0.25f);
            rt.DOScale(battleScale, 0.25f);
        }

        public void BattleOut()
        {
            transform.SetParent(persPos);
            transform.SetSiblingIndex(0);
            rt.DOAnchorPos(Vector2.zero, 0.25f);
            rt.DOScale(idleScale, 0.25f);
        }
        IEnumerator PlayDead()
        {
            yield return new WaitForSeconds(0.2f); //need for avoid idle animation state if isDead
            if (isOverlord)
            {
                spineWidget.PlayAnimation(characterRes.ani_defeat_name, false);
                yield return new WaitForSeconds(spineWidget.GetAnimationDuaration(characterRes.ani_defeat_name));
                spineWidget.PlayAnimation("defeat2", true); //!костыль
            }
            else
                spineWidget.PlayAnimation(characterRes.ani_defeat_name, false);
        }

        public void Damage(float value, bool hit, bool dodge, bool crit)
        {
            if (!hit)
            {
                DrawPopup("Miss!", "blue");
                return;
            }
            if (dodge)
            {
                DrawPopup("Dodge!", "yellow");
                return;
            }
            if (crit) value *= 2;
            if (value > 0)
            {
                health -= value;
                health = Mathf.Max(health, 0);
                if (health <= 0)
                {
                    isDead = true;
                    bt.onClick.RemoveAllListeners();
                    StartCoroutine(PlayDead());
                    bm.StateUpdate(this);
                }
                if (crit)
                    DrawPopup($"Crit! {value}", "red");
                else
                    DrawPopup($"{value}", "red");
                UpdateUI();
            }
            else
            {
                Heal(-value);
            }
        }
        public void Heal(float value)
        {
            if (bless_healBlock < 0)
            {
                DrawPopup($"Heal blocked!", "red");
            }
            else
            {
                //heal fx or animation
                if (bless_healBlock > 0)
                    value += value * bless_dot;
                health += value;
                health = Mathf.Min(health, healthMax);
                DrawPopup($"{value}", "green");
                UpdateUI();
            }
        }
        public void HealMP(int value)
        {
            mana += value;
            mana = Mathf.Min(mana, manaMax);
            UpdateUI();
        }

        void AddEffect(AdminBRO.CharacterSkill sk, CharController targetCC = null)
        {
            if (targetCC == null) targetCC = this;
            bool hit = sk.effectProb >= Random.Range(0, 100);
            if (sk.effect != null)
                if (hit)
                {
                    var duration = Mathf.RoundToInt(sk.effectActingDuration);
                    float ea = sk.effectAmount;
                    float effectAmount = healthMax * (ea / 100f);
                    switch (sk.effect)
                    {
                        case "defense_up":
                            defUp_defDown = duration;
                            defUp_defDown_dot = ea / 100f;
                            DrawPopup(msg_defence_up, "green");
                            break;
                        case "defense_down":
                            if (immunity == 0)
                            {
                                defUp_defDown = -duration;
                                defUp_defDown_dot = effectAmount;
                                DrawPopup(msg_defence_down, "red");
                            }
                            else
                                DrawPopup(msg_immunity, "green");
                            break;
                        case "focus":
                            focus_blind = duration;
                            break;
                        case "blind":
                            if (immunity == 0)
                                focus_blind = -duration;
                            else
                                DrawPopup(msg_immunity, "blue");
                            break;
                        case "regeneration":
                            regen_poison = duration;
                            regen_poison_dot = -effectAmount;
                            break;
                        case "poison":
                            if (immunity == 0)
                            {
                                regen_poison = -duration;
                                regen_poison_dot = effectAmount;
                                DrawPopup(msg_poison, "red");
                            }
                            else
                                DrawPopup(msg_immunity, "blue");
                            break;
                        case "bless":
                            bless_healBlock = duration;
                            bless_dot = effectAmount;
                            break;
                        case "heal_block":
                            if (immunity == 0)
                                bless_healBlock = -duration;
                            else
                                DrawPopup(msg_immunity, "blue");
                            break;
                        case "silence":
                            if (immunity == 0)
                                silence = duration;
                            else
                                DrawPopup(msg_immunity, "blue");
                            break;
                        case "immunity":
                            immunity = duration;
                            DrawPopup(msg_immunity, "green");
                            break;
                        case "stun":
                            if (immunity == 0)
                                stun = true;
                            else
                                DrawPopup(msg_immunity, "blue");
                            break;
                        case "curse":
                            if (immunity == 0)
                            {
                                curse = duration;
                                curse_dot = ea / 100f; //Calculate in total damage
                            }
                            break;
                        case "dispel":
                            Dispel();
                            break;
                        case "safeguard":
                            Safeguard();
                            break;
                        default:
                            Debug.LogError($"Unknow Value AdminBRO.CharacterSkill.effect: {sk.effect}");
                            break;
                    }
                    observer.UpdateStatuses();
                }
                else
                {
                    DrawPopup("Effect miss", "red");
                }
        }
        void PassiveBuff(AdminBRO.CharacterSkill sk)
        {
            bool isCrit = critrate > Random.value;

            Damage(sk.amount, true, false, isCrit);
            bool hitEffect = sk.effectProb >= Random.Range(0, 100);

            if (sk.effect != null)
                if (hitEffect)
                {
                    var duration = Mathf.RoundToInt(sk.effectActingDuration);
                    float ea = sk.effectAmount;
                    float effectAmount = healthMax * (ea / 100f);
                    switch (sk.effect)
                    {
                        case "defense_up":
                            defUp_defDown = duration;
                            defUp_defDown_dot = ea / 100f;
                            DrawPopup(msg_defence_up, "green");
                            break;
                        case "focus":
                            focus_blind = duration;
                            break;
                        case "regeneration":
                            regen_poison = duration;
                            regen_poison_dot = -effectAmount;
                            break;
                        case "bless":
                            bless_healBlock = duration;
                            bless_dot = effectAmount;
                            break;
                        case "immunity":
                            immunity = duration;
                            DrawPopup(msg_immunity, "green");
                            break;
                        case "dispel":
                            Dispel();
                            break;
                        case "safeguard":
                            Safeguard();
                            break;
                        default:
                            Debug.LogError($"Unknow Value or Debuff Effect on Passive Heal AdminBRO.CharacterSkill.effect: {sk.effect}");
                            break;
                    }
                    observer.UpdateStatuses();
                }
                else
                {
                    DrawPopup("Passive effect miss", "red");
                }
        }
        void PassiveDeBuff(AdminBRO.CharacterSkill sk, CharController targetCC)
        {
            bool isCrit = critrate > Random.value;

            Damage(sk.amount, true, false, isCrit);
            bool hitEffect = sk.effectProb >= Random.Range(0, 100);
            if (sk.effect != null)
                if (hitEffect)
                {
                    var duration = Mathf.RoundToInt(sk.effectActingDuration);
                    float ea = sk.effectAmount;
                    float effectAmount = targetCC.healthMax * (ea / 100f);
                    switch (sk.effect)
                    {
                        case "defense_down":
                            if (targetCC.immunity == 0)
                            {
                                targetCC.defUp_defDown = -duration;
                                targetCC.defUp_defDown_dot = effectAmount;
                                DrawPopup(msg_defence_down, "red");
                            }
                            else
                                DrawPopup(msg_immunity, "green");
                            break;
                        case "blind":
                            if (targetCC.immunity == 0)
                                targetCC.focus_blind = -duration;
                            else
                                DrawPopup(msg_immunity, "blue");
                            break;
                        case "poison":
                            if (targetCC.immunity == 0)
                            {
                                targetCC.regen_poison = -duration;
                                targetCC.regen_poison_dot = effectAmount;
                                DrawPopup(msg_poison, "red");
                            }
                            else
                                DrawPopup(msg_immunity, "blue");
                            break;
                        case "heal_block":
                            if (targetCC.immunity == 0)
                                targetCC.bless_healBlock = -duration;
                            else
                                DrawPopup(msg_immunity, "blue");
                            break;
                        case "silence":
                            if (targetCC.immunity == 0)
                                targetCC.silence = duration;
                            else
                                DrawPopup(msg_immunity, "blue");
                            break;
                        case "stun":
                            if (targetCC.immunity == 0)
                                targetCC.stun = true;
                            else
                                DrawPopup(msg_immunity, "blue");
                            break;
                        case "curse":
                            if (targetCC.immunity == 0)
                            {
                                targetCC.curse = duration;
                                targetCC.curse_dot = ea / 100f; //Calculate in total damage
                            }
                            break;
                        case "dispel":
                            Dispel();
                            break;
                        default:
                            Debug.LogError($"Unknow Value AdminBRO.CharacterSkill.effect: {sk.effect}");
                            break;
                    }
                    observer.UpdateStatuses();
                }
                else
                {
                    DrawPopup("Effect miss", "red");
                }
        }
        public void Dispel()
        {
            //dispel vfx
            focus_blind = 0;
            defUp_defDown = 0;
            regen_poison = 0;
            bless_healBlock = 0;
            immunity = 0;
            silence = 0;
            curse = 0;
            stun = false;
            observer.UpdateStatuses();
        }
        public void Safeguard()
        {
            if (stun)
            {
                stun = false;
                DrawPopup("- " + msg_stun, "green");
                observer.UpdateStatuses();
            }
            if (defUp_defDown < 0)
            {
                defUp_defDown = 0;
                DrawPopup("- " + msg_defence_down, "green");
                observer.UpdateStatuses();
            }
        }
        public void UpadeteRoundEnd()
        {
            if (regen_poison != 0)
            {
                Damage(regen_poison_dot, true, false, false);
                regen_poison -= (int)Mathf.Sign(regen_poison);
            }
            if (focus_blind != 0)
                focus_blind -= (int)Mathf.Sign(focus_blind);
            if (defUp_defDown != 0)
                defUp_defDown -= (int)Mathf.Sign(defUp_defDown);
            if (regen_poison != 0)
                regen_poison -= (int)Mathf.Sign(regen_poison);
            if (bless_healBlock != 0)
                bless_healBlock -= (int)Mathf.Sign(bless_healBlock);

            if (immunity != 0) immunity--;
            if (silence != 0) silence--;
            if (curse != 0) curse--;
            observer.UpdateStatuses();
        }

        void DrawPopup(string msg, string color = "white")
        {
            if (popUpPrefab != null)
            {
                var damageText = Instantiate(popUpPrefab, rt);
                damageText.GetComponent<DamagePopup>().Setup(msg, isEnemy && !isBoss, 0f, color);
            }
            else Debug.LogError($"Null Draw Popup Prefab: {gameObject.name}");
        }

        private void OnDestroy()
        {
            bm.unselect -= UnHiglight;
            bm.roundEnd -= UpadeteRoundEnd;
            Destroy(observer.gameObject);
            bt.gameObject.SetActive(false);
        }

    }
}