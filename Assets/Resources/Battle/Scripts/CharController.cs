using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class CharController : MonoBehaviour
    {
        public bool isEnemy = false;
        public bool isBoss = false;
        public bool isOverlord = false;

        public BattleManager bm; //init on BattleManager Initialize();
        public BattleLog log => bm.log;
        public AdminBRO.Character character;
        public CharacterPortrait charStats;
        public CharacterStatObserver observer;
        public GameObject popUpPrefab;
        public Transform selfVFX, topVFX, topVFX_L, topVFX_R;

        public List<AdminBRO.CharacterSkill> skill = new List<AdminBRO.CharacterSkill>();
        public List<AdminBRO.CharacterSkill> passiveSkill = new List<AdminBRO.CharacterSkill>();
        public Dictionary<AdminBRO.CharacterSkill, int> skillCD = new Dictionary<AdminBRO.CharacterSkill, int>(5);
        private List<AdminBRO.Sound> skillSFX = new List<AdminBRO.Sound>(3) { null, null, null };

        //New Resources:
        private string ani_idle_name = "idle";
        public string[] ani_pAttack_name = { "prepair1", "prepair2", "prepair3", "prepair4", "prepair5", "prepair6" };
        public string[] ani_attack_name = { "attack1", "attack2", "attack3", "attack4", "attack5", "attack6" };
        public string ani_defence_name = "defence";
        public string ani_defeat_name = "defeat";
        private float shakeDuration = 1.5f, shakeStrength = 12.5f;

        public Sprite icon, bigIcon;
        public string Name => character.name;
        public string characterClass => character.characterClass;
        public int level => (int)character.level;
        public string rarity => character.rarity;
        public float speed => (float)character.speed;
        public float power => (float)character.power;
        public float constitution => (float)character.constitution;
        public float agility => (float)character.agility;
        public float accuracy => (float)character.accuracy;
        public float dodge => (float)character.dodge;
        public float critrate => (float)character.critrate;
        public float damage => (float)character.damage;
        public float damageTotal = 10;


        public float zoomSpeed = 0.15f;
        private float idleScale = .9f, battleScale = 1.5f;
        private float overIdleScale = .85f, overBattleScale = 1.4f; //0.93f
        public int battleOrder = 1;
        public float health = 100, healthMax = 100;
        public float mana = 100, manaMax = 100;

        public bool isDead = false;

        private Transform battleLayer;
        public Transform persPos;
        private Transform battlePos;
        private SpineWidget spineWidget;
        private float defenceDuration = 1f;
        public float
            preAttackDuration = 1f,
            attackDuration = 1f,
            vfxDuration = 0f;
        private RectTransform rt;

        public int
            focus_blind, defUp_defDown,
            regen_poison, bless_healBlock,
            immunity, silence, curse;
        public bool stun;
        float defBuffDelay = 1.2f, defVfxDelay = 1.2f;
        float buffDelay = 1.2f, buffVFXDelay = 1.2f;

        [Tooltip("Reduce or add total damage in percentage up to 100")]
        public float defUp_defDown_dot = 0f;
        [Tooltip("(-) Heal or (+) Damage as a percentage of maximum health at the end of the round")]
        public float regen_poison_dot = 0f;
        [Tooltip("Heal Up damage in percentage up to 100")]
        public float bless_dot = 0f;
        [Tooltip("Total damage reduced by a percentage")]
        public float curse_dot = 0f;

        private string

            msg_defence_up = "<sprite=\"BuffsNDebuffs\" name=\"BuffDefenceUp\"> Defence up",
            msg_defence_down = "<sprite=\"BuffsNDebuffs\" name=\"DebuffDefenceDown\"> Defence down",

            msg_regen = "<sprite=\"BuffsNDebuffs\" name=\"BuffRegeneration\"> Regeneration",
            msg_poison = "<sprite=\"BuffsNDebuffs\" name=\"DebuffPoison\"> Poison",

            msg_focus = "<sprite=\"BuffsNDebuffs\" name=\"BuffFocus\"> Focus",
            msg_blind = "<sprite=\"BuffsNDebuffs\" name=\"DebuffBlind\"> Blind",

            msg_bless = "<sprite=\"BuffsNDebuffs\" name=\"BuffBless\"> Bless",
            msg_healblock = "<sprite=\"BuffsNDebuffs\" name=\"DebuffHealBlock\"> Heal Block",

            msg_immunity = "<sprite=\"BuffsNDebuffs\" name=\"BuffImmunity\"> Immunity",
            msg_curse = "<sprite=\"BuffsNDebuffs\" name=\"DebuffCurse\"> Curse",
            msg_stun = "<sprite=\"BuffsNDebuffs\" name=\"DebuffStun\"> Stun",
            msg_silence = "<sprite=\"BuffsNDebuffs\" name=\"DebuffSilence\"> Silence",

            msg_safeguard = "<sprite=\"BuffsNDebuffs\" name=\"BuffSafeguard\"> Safeguard",
            msg_dispel = "<sprite=\"BuffsNDebuffs\" name=\"BuffDispell\"> Dispell";
        private GameObject vfx_purple => bm.vfx_purple;
        private GameObject vfx_red => bm.vfx_red;
        private GameObject vfx_blue => bm.vfx_blue;
        private GameObject vfx_green => bm.vfx_green;
        private GameObject vfx_stun => bm.vfx_stun;

        private void Start()
        {
            StatInit();
            ShapeInit();
            UIInit();
            PlayIdle();
        }

        private void StatInit()
        {
            //isEnemy and battleOrder assign on battle manager
            isOverlord = character.characterClass == AdminBRO.Character.Class_Overlord;
            if (isOverlord)
            {
                bm.overlord = this;
                idleScale = overIdleScale;
                battleScale = overBattleScale;
            }
            health = (float)character.health;
            healthMax = health;
            mana = (float)character.mana;
            manaMax = mana;
            if (isBoss) battleScale = 1f;
            var skillStash = character.skills;
            foreach (var sk in skillStash)
                skillCD.Add(sk, 0);
            AdminBRO.CharacterSkill tempSK = null;
            if (isOverlord)
            {
                tempSK = skillStash?.Find(f => f.type == "overlord_attack");
                if (tempSK != null) skill.Add(tempSK);
                tempSK = skillStash?.Find(f => f.type == "overlord_enhanced_attack");
                if (tempSK != null) skill.Add(tempSK);
                tempSK = skillStash?.Find(f => f.type == "overlord_ultimate_attack");
                if (tempSK != null) skill.Add(tempSK);
                tempSK = skillStash?.Find(f => f.type == "overlord_first_passive_skill");
                if (tempSK != null) passiveSkill.Add(tempSK);
                tempSK = skillStash?.Find(f => f.type == "overlord_second_passive_skill");
                if (tempSK != null) passiveSkill.Add(tempSK);
            }
            else
            {
                tempSK = skillStash?.Find(f => f.type == "attack");
                if (tempSK != null) skill.Add(tempSK);
                tempSK = skillStash?.Find(f => f.type == "enhanced_attack");
                if (tempSK != null) skill.Add(tempSK);
                tempSK = skillStash?.Find(f => f.type == "passive_skill");
                if (tempSK != null) passiveSkill.Add(tempSK);
            }
            skillSFX[0] = (character.sfxAttack1 != null ? character.sfxAttack1 : skill[0].sfxAttack);
            if (skill.Count > 1) skillSFX[1] = (character.sfxAttack2 != null ? character.sfxAttack2 : skill[1].sfxAttack);
            if (skill.Count > 2) skillSFX[2] = (skill[2].sfxAttack);
        }
        private void ShapeInit()
        {
            icon = ResourceManager.LoadSprite(character.GetIconByRarity(character.rarity));
            bigIcon = ResourceManager.LoadSprite(character.battlePortraitIcon);
            rt = gameObject.AddComponent<RectTransform>();
            UITools.SetStretch(rt);
            rt.localScale *= idleScale;
            Transform spawnPos = bm.transform;
            battleLayer = spawnPos.Find("BattleCanvas/BattleLayer").transform;
            selfVFX = transform;
            topVFX = battleLayer.Find("topVFX");
            topVFX_L = topVFX.Find("L");
            topVFX_R = topVFX.Find("R");
            if (isEnemy)
            {
                if (isBoss)
                {
                    battlePos = battleLayer.Find("battlePosBoss").transform;
                    persPos = battleLayer.Find("boss").transform;
                    selfVFX = topVFX_R;
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
            if (character.animationData != null)
                spineWidget = SpineWidget.GetInstance(character.animationData, transform);
            else
                log.Add($"{name} animationData is null", error: true);
            defenceDuration = spineWidget.GetAnimationDuaration(ani_defence_name);
        }
        private void UIInit()
        {
            GameObject observerGO;
            if (isEnemy && !isBoss)
                observerGO = Resources.Load("Battle/Prefabs/CharGUIEnemy") as GameObject;
            else if (isBoss)
                observerGO = Resources.Load("Battle/Prefabs/CharGUIBoss") as GameObject;
            else
                observerGO = Resources.Load("Battle/Prefabs/CharGUI") as GameObject;
            observer = Instantiate(observerGO, persPos).GetComponent<CharacterStatObserver>();
            observer.cc = this;

            popUpPrefab = Resources.Load("Battle/Prefabs/BattlePopup") as GameObject;
            charStats?.InitUI(this);
        }

        public void CharPortraitSet() => charStats?.SetUI(this);
        public void UpdateUI() => observer?.UpdateUI();

        public void PlayIdle()
        {
            //if (!isDead)
            //{
            character.sfxIdle?.Play();
            spineWidget.PlayAnimation(ani_idle_name, true);
            //}
        }

        public void Attack(int attackID, bool AOE = false, CharController target = null) =>
            StartCoroutine(PlayAttack(attackID, AOE, target));

        IEnumerator PlayAttack(int id, bool AOE = false, CharController target = null)
        {
            if (id >= skill.Count) id = 0;                              //Overflow insurance
            var skillID = skill[id];
            var curseDotScale = curse > 0 ? 1 - curse_dot : 1f;
            BattleIn(AOE);
            vfxDuration = 0f;
            preAttackDuration = spineWidget.GetAnimationDuaration(ani_pAttack_name[id]);    //send to target Defence Animation
            attackDuration = spineWidget.GetAnimationDuaration(ani_attack_name[id]);

            if (skillID.vfxAOE != null && skillID.AOE)                 //VFX AOE            is here cuz we need vfxDuration
            {
                var vfxGO = new GameObject($"vfx_{skillID.vfxAOE.title}");
                var vfx = vfxGO.AddComponent<VFXManager>();
                var spawnPoint = topVFX;
                var heal = skillID.actionType == "heal";
                spawnPoint = (isOverlord) ? topVFX : (isEnemy == heal || !isEnemy == !heal) ? topVFX_R : topVFX_L;
                vfxDuration = vfx.Setup(skillID.vfxAOE, spawnPoint, preAttackDuration + zoomSpeed);
            }

            yield return new WaitForSeconds(zoomSpeed);
            foreach (var ps in passiveSkill)                            //on_attack Passive Skill Buff
                if (ps.trigger == "on_attack")
                    if (ps.actionType == "heal")
                        PassiveBuff(ps);
            damageTotal = damage * ((float)skillID.amount);
            damageTotal *= curseDotScale;
            damageTotal = Mathf.Round(damageTotal);
            spineWidget.PlayAnimation(ani_pAttack_name[id], false);     //Play SW                      
            skillSFX[id]?.Play();                                          //SFX
            yield return new WaitForSeconds(preAttackDuration);
            if (skillID.vfxSelf != null)                                //VFX Self
            {
                var vfxGO = new GameObject($"vfx_{skillID.vfxSelf.title}");
                var vfx = vfxGO.AddComponent<VFXManager>();
                vfx.Setup(skillID.vfxSelf, selfVFX);
            }
            if (skillID.shakeScreen)                                  //Shake
                bm.Shake(shakeDuration, shakeStrength);

            if (AOE) yield return new WaitForSeconds(vfxDuration);           //VFX Delay if it need (!=0)

            foreach (var ps in passiveSkill)                            //on_attack Passive Skill DeBuff
                if (ps.trigger == "on_attack" && ps.actionType == "damage")
                    if (AOE)
                        if (isEnemy)
                            foreach (var item in bm.enemyTargetList)
                                PassiveDeBuff(ps, item);
                        else
                            foreach (var item in bm.enemyAllyList)
                                PassiveDeBuff(ps, item);
                    else
                        PassiveDeBuff(ps, target);
            spineWidget.PlayAnimation(ani_attack_name[id], false);

            yield return new WaitForSeconds(attackDuration);
            PlayIdle();
            BattleOut(AOE);
            bm.BattleOut();
        }

        public void Defence(CharController attacker, int id, bool aoe = false) => StartCoroutine(PlayDefence(attacker, id, aoe));

        IEnumerator PlayDefence(CharController attacker, int id, bool AOE = false)
        {
            BattleIn(AOE);
            var attackerSkill = attacker.skill[id];

            yield return new WaitForSeconds(zoomSpeed);
            transform.SetParent(battlePos);
            UnHiglight();

            yield return new WaitForSeconds(attacker.preAttackDuration + attacker.vfxDuration);
            character.sfxDefense?.Play();

            if (attackerSkill.actionType != "heal")
                spineWidget.PlayAnimation(ani_defence_name, false);

            attackerSkill.sfxTarget?.Play();                    //Play on target SFX
            var isHit = (attacker.focus_blind == 0) ? attacker.accuracy > Random.value
                : (attacker.focus_blind > 0) ? true : false;
            var isDodge = dodge > Random.value;
            var isCrit = attacker.critrate > Random.value;
            if (isHit) AddEffect(attackerSkill);                //calculate probability and add effect
            if (attackerSkill.vfxTarget != null && !isDodge)    //VFX on Self
            {
                var vfxGO = new GameObject($"vfx_{attackerSkill.vfxTarget.title}");
                var vfx = vfxGO.AddComponent<VFXManager>();
                vfx.Setup(attackerSkill.vfxTarget, selfVFX);
            }
            foreach (var ps in passiveSkill)
            {
                if (ps?.trigger == "when_attacked")
                    if (ps.actionType == "heal")                //who is target "heal" = self    !"heal" = enemy
                        PassiveBuff(ps);
                    else
                        PassiveDeBuff(ps, attacker);
            }
            float defScale = defUp_defDown != 0 ? attacker.damageTotal * defUp_defDown_dot * -Mathf.Sign(defUp_defDown) : 0f; //defence up down
            Damage(attacker.damageTotal + defScale, isHit, isDodge, isCrit, uiDelay: 1.5f);

            yield return new WaitForSeconds(defenceDuration); // (/2)

            if (attackerSkill.actionType != "heal") //skip play idle to avoid strange loop transitions
                PlayIdle();
            BattleOut(AOE);
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
                //if (!isEnemy) CharPortraitSet();
                SoundManager.PlayOneShot(FMODEventPath.UI_Battle_SelectPers);
            }
        }

        public void Highlight() => observer?.Border(true);
        public void UnHiglight() => observer?.Border(false);

        public void BattleIn(bool AOE = false)
        {
            UnHiglight();
            transform.SetParent(battlePos);
            if (AOE) return;
            rt.DOAnchorPos(Vector2.zero, zoomSpeed);
            rt.DOScale(battleScale, zoomSpeed);
        }

        public void BattleOut(bool AOE = false)
        {
            transform.SetParent(persPos);
            transform.SetSiblingIndex(0);
            if (AOE) return;
            rt.DOAnchorPos(Vector2.zero, zoomSpeed);
            rt.DOScale(idleScale, zoomSpeed);
        }
        IEnumerator PlayDead(float delay)
        {
            yield return new WaitForSeconds(0.5f + delay); //need for avoid idle animation state if isDead
            character.sfxDefeat?.Play();
            if (isOverlord)
            {
                spineWidget.PlayAnimation("defeat1", false);
                yield return new WaitForSeconds(spineWidget.GetAnimationDuaration(ani_defeat_name));
                spineWidget.PlayAnimation("defeat2", true); //!костыль
            }
            else
            {
                spineWidget.PlayAnimation(ani_defeat_name, false);
                yield return new WaitForSeconds(spineWidget.GetAnimationDuaration(ani_defeat_name));
                Destroy(gameObject);
            }
        }

        public void Damage(float value, bool hit, bool dodge, bool crit, bool poison = false, float uiDelay = 0f)
        {
            if (hit == false)
            {
                DrawPopup("Miss!", "blue");
                return;
            }
            if (crit) value *= 2;
            value = Mathf.Round(value);
            if (value > 0)
            {
                if (dodge)
                {
                    DrawPopup("Dodge!", "blue");
                    return;
                }
                health -= value;
                health = Mathf.Round(health);
                health = Mathf.Max(health, 0);
                if (crit)
                    DrawPopup($"Crit!\n{value}", "yellow", fast: true);
                else if (poison)
                    DrawPopup($"-{value}HP", "purple", fast: true);
                else
                    DrawPopup($"{value}", "white", fast: true);
                if (health <= 0)
                {
                    isDead = true;
                    StartCoroutine(PlayDead(uiDelay));
                    bm.DeadStatesUpdate(this, poison);
                }
                StartCoroutine(UIDelay(uiDelay));
            }
            else if (value < 0)
                Heal(-value);
        }
        IEnumerator UIDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            UpdateUI();
        }
        public void Heal(float value)
        {
            value = Mathf.Round(value);
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
                health = Mathf.Round(health);
                health = Mathf.Min(health, healthMax);
                DrawPopup($"+{value}HP", "green");
                UpdateUI();
            }
        }
        public void HealMP(int value)
        {
            mana += value;
            mana = Mathf.Min(mana, manaMax);
            UpdateUI();
        }
        public void ManaReduce(float manaCost)
        {
            if (isOverlord)
            {
                mana -= manaCost;
                Mathf.RoundToInt(mana);
                UpdateUI();
            }
        }

        IEnumerator InstVFXDelay(GameObject vfx, Transform transform, float delay = 0f)
        {
            yield return new WaitForSeconds(delay);
            if (vfx) Instantiate(vfx, transform);
        }
        void AddEffect(AdminBRO.CharacterSkill sk, CharController targetCC = null, bool addManual = false)
        {
            if (targetCC == null) targetCC = this;
            bool hit = sk.effectProb >= Random.value;
            if (sk.effect != null)
                if (hit)
                {
                    var duration = (int)sk.effectActingDuration;
                    float ea = sk.effectAmount;
                    float effectAmount = healthMax * ea;
                    switch (sk.effect)
                    {
                        case "defense_up":
                            defUp_defDown = addManual ? defUp_defDown + duration : duration;
                            defUp_defDown_dot = ea;
                            StartCoroutine(InstVFXDelay(vfx_blue, selfVFX, buffVFXDelay));
                            DrawPopup(msg_defence_up, "blue", buffDelay);
                            break;
                        case "defense_down":
                            if (immunity == 0)
                            {
                                defUp_defDown = addManual ? defUp_defDown - duration : -duration; // -duration;
                                defUp_defDown_dot = ea;
                                StartCoroutine(InstVFXDelay(vfx_red, selfVFX, buffVFXDelay));
                                DrawPopup(msg_defence_down, "red", buffDelay);
                            }
                            else
                                DrawPopup(msg_immunity, "green", buffDelay);
                            break;
                        case "focus":
                            focus_blind = addManual ? focus_blind + duration : duration;
                            StartCoroutine(InstVFXDelay(vfx_blue, selfVFX, buffVFXDelay));
                            DrawPopup(msg_focus, "blue", buffDelay);
                            break;
                        case "blind":
                            if (immunity == 0)
                            {
                                focus_blind = addManual ? focus_blind - duration : -duration; //-duration;
                                StartCoroutine(InstVFXDelay(vfx_red, selfVFX, buffVFXDelay));
                                DrawPopup(msg_blind, "red", buffDelay);
                            }
                            else
                                DrawPopup(msg_immunity, "green", buffDelay);
                            break;
                        case "regeneration":
                            regen_poison = addManual ? regen_poison + duration : duration;
                            regen_poison_dot = -effectAmount;
                            StartCoroutine(InstVFXDelay(vfx_green, selfVFX, buffVFXDelay));
                            DrawPopup(msg_regen, "blue", buffDelay);
                            break;
                        case "poison":
                            if (immunity == 0)
                            {
                                regen_poison = addManual ? regen_poison - duration : -duration; //-duration;
                                regen_poison_dot = effectAmount;
                                StartCoroutine(InstVFXDelay(vfx_purple, selfVFX, buffVFXDelay));
                                DrawPopup(msg_poison, "purple", buffDelay);
                            }
                            else
                                DrawPopup(msg_immunity, "green", buffDelay);
                            break;
                        case "bless":
                            bless_healBlock = addManual ? bless_healBlock + duration : duration;
                            bless_dot = effectAmount;
                            StartCoroutine(InstVFXDelay(vfx_blue, selfVFX, buffVFXDelay));
                            DrawPopup(msg_bless, "blue", buffDelay);
                            break;
                        case "heal_block":
                            if (immunity == 0)
                            {
                                bless_healBlock = addManual ? bless_healBlock - duration : -duration;//-duration;
                                StartCoroutine(InstVFXDelay(vfx_red, selfVFX, buffVFXDelay));
                                DrawPopup(msg_blind, "red", buffDelay);
                            }
                            else
                                DrawPopup(msg_immunity, "green", buffDelay);
                            break;
                        case "silence":
                            if (immunity == 0)
                            {
                                silence = addManual ? silence + duration : duration;
                                StartCoroutine(InstVFXDelay(vfx_red, selfVFX, buffVFXDelay));
                                DrawPopup(msg_silence, "red", buffDelay);
                            }
                            else
                                DrawPopup(msg_immunity, "green", buffDelay);
                            break;
                        case "immunity":
                            immunity = addManual ? immunity + duration : duration;
                            StartCoroutine(InstVFXDelay(vfx_blue, selfVFX, buffVFXDelay));
                            DrawPopup(msg_immunity, "blue", buffDelay);
                            break;
                        case "stun":
                            if (immunity == 0)
                            {
                                stun = true;
                                StartCoroutine(InstVFXDelay(vfx_stun, selfVFX, buffVFXDelay));
                                DrawPopup(msg_stun, "red", buffDelay);
                            }
                            else
                                DrawPopup(msg_immunity, "green", buffDelay);
                            break;
                        case "curse":
                            if (immunity == 0)
                            {
                                curse = addManual ? curse + duration : duration;
                                curse_dot = ea; //Calculate in total damage
                                StartCoroutine(InstVFXDelay(vfx_red, selfVFX, buffVFXDelay));
                                DrawPopup(msg_curse, "red", buffDelay);
                            }
                            else
                                DrawPopup(msg_immunity, "green", buffDelay);
                            break;
                        case "dispel":
                            Dispel();
                            break;
                        case "safeguard":
                            Safeguard();
                            break;
                        default:
                            log.Add($"Unknow Value AdminBRO.CharacterSkill.effect: {sk.effect}", true);
                            break;
                    }
                    observer?.UpdateStatuses();
                } //else DrawPopup("Effect miss", "red");
        }

        void PassiveBuff(AdminBRO.CharacterSkill sk)
        {
            if (skillCD[sk] > 0) //skip if skill on Cool Down
                return;
            else
                skillCD[sk] = (int)sk.effectCooldownDuration;

            bool isCrit = critrate >= Random.value;
            Damage(sk.amount, true, false, isCrit);
            bool hitEffect = sk.effectProb >= Random.value;

            if (sk.effect != null)
                if (hitEffect)
                {
                    var duration = (int)sk.effectActingDuration;
                    float ea = sk.effectAmount;
                    float effectAmount = healthMax * ea;

                    switch (sk.effect)
                    {
                        case "defense_up":
                            defUp_defDown = duration;
                            defUp_defDown_dot = ea;
                            StartCoroutine(InstVFXDelay(vfx_blue, selfVFX, buffVFXDelay));
                            DrawPopup(msg_defence_up, "blue", buffDelay);
                            break;
                        case "focus":
                            focus_blind = duration;
                            StartCoroutine(InstVFXDelay(vfx_blue, selfVFX, buffVFXDelay));
                            DrawPopup(msg_focus, "blue", buffDelay);
                            break;
                        case "regeneration":
                            regen_poison = duration;
                            regen_poison_dot = -effectAmount;
                            StartCoroutine(InstVFXDelay(vfx_green, selfVFX, buffVFXDelay));
                            DrawPopup(msg_regen, "blue", buffDelay);
                            break;
                        case "bless":
                            bless_healBlock = duration;
                            bless_dot = effectAmount;
                            StartCoroutine(InstVFXDelay(vfx_blue, selfVFX, buffVFXDelay));
                            DrawPopup(msg_bless, "blue", buffDelay);
                            break;
                        case "immunity":
                            immunity = duration;
                            StartCoroutine(InstVFXDelay(vfx_blue, selfVFX, buffVFXDelay));
                            DrawPopup(msg_immunity, "blue", buffDelay);
                            break;
                        case "dispel":
                            Dispel();
                            break;
                        case "safeguard":
                            Safeguard();
                            break;
                        default:
                            log.Add($"Unknow Value or Debuff Effect on Passive Heal AdminBRO.CharacterSkill.effect: {sk.effect}", true);
                            break;
                    }
                    observer?.UpdateStatuses();
                }
        }
        void PassiveDeBuff(AdminBRO.CharacterSkill sk, CharController targetCC)
        {
            if (skillCD[sk] > 0) //skip if skill on Cool Down
                return;
            else
                skillCD[sk] = Mathf.RoundToInt(sk.effectCooldownDuration);
            bool isCrit = critrate >= Random.value;
            targetCC.Damage(sk.amount, true, false, isCrit);
            bool hitEffect = sk.effectProb >= Random.value;

            if (sk.effect != null)
                if (hitEffect)
                {
                    var duration = (int)sk.effectActingDuration;
                    float ea = sk.effectAmount;
                    float effectAmount = targetCC.healthMax * ea;
                    switch (sk.effect)
                    {
                        case "defense_down":
                            if (targetCC.immunity == 0)
                            {
                                targetCC.defUp_defDown = -duration;
                                targetCC.defUp_defDown_dot = ea;
                                StartCoroutine(InstVFXDelay(vfx_red, targetCC.selfVFX, buffVFXDelay));
                                DrawPopup(msg_defence_down, "red", buffDelay);
                            }
                            else
                                DrawPopup(msg_immunity, "green", buffDelay);
                            break;
                        case "blind":
                            if (targetCC.immunity == 0)
                            {
                                targetCC.focus_blind = -duration;
                                StartCoroutine(InstVFXDelay(vfx_red, targetCC.selfVFX, buffVFXDelay));
                                DrawPopup(msg_blind, "red", buffDelay);
                            }
                            else
                                DrawPopup(msg_immunity, "green", buffDelay);
                            break;
                        case "poison":
                            if (targetCC.immunity == 0)
                            {
                                targetCC.regen_poison = -duration;
                                targetCC.regen_poison_dot = effectAmount;
                                StartCoroutine(InstVFXDelay(vfx_purple, targetCC.selfVFX, buffVFXDelay));
                                DrawPopup(msg_poison, "purple", buffDelay);
                            }
                            else
                                DrawPopup(msg_immunity, "green", buffDelay);
                            break;
                        case "heal_block":
                            if (targetCC.immunity == 0)
                            {
                                targetCC.bless_healBlock = -duration;
                                StartCoroutine(InstVFXDelay(vfx_red, targetCC.selfVFX, buffVFXDelay));
                                DrawPopup(msg_healblock, "red", buffDelay);
                            }
                            else
                                DrawPopup(msg_immunity, "green", buffDelay);
                            break;
                        case "silence":
                            if (targetCC.immunity == 0)
                            {
                                targetCC.silence = duration;
                                StartCoroutine(InstVFXDelay(vfx_red, targetCC.selfVFX, buffVFXDelay));
                                DrawPopup(msg_silence, "red", buffDelay);
                            }
                            else
                                DrawPopup(msg_immunity, "green", buffDelay);
                            break;
                        case "stun":
                            if (targetCC.immunity == 0)
                            {
                                targetCC.stun = true;
                                StartCoroutine(InstVFXDelay(vfx_stun, targetCC.selfVFX, buffVFXDelay));
                                DrawPopup(msg_stun, "red", buffDelay);
                            }
                            else
                                DrawPopup(msg_immunity, "green", buffDelay);
                            break;
                        case "curse":
                            if (targetCC.immunity == 0)
                            {
                                targetCC.curse = duration;
                                targetCC.curse_dot = ea; //Calculate in total damage
                                StartCoroutine(InstVFXDelay(vfx_red, targetCC.selfVFX, buffVFXDelay));
                                DrawPopup(msg_curse, "red", buffDelay);
                            }
                            else
                                DrawPopup(msg_immunity, "green", buffDelay);
                            break;
                        case "dispel":
                            Dispel();
                            break;
                        default:
                            log.Add($"Unknow Value AdminBRO.CharacterSkill.effect: {sk.effect}", true);
                            break;
                    }
                    observer?.UpdateStatuses();
                }
        }
        public void AddEffectManual(string effect)
        {
            var customSkill = new AdminBRO.CharacterSkill()
            {
                effect = effect,
                effectActingDuration = 1,
                effectAmount = 0.25f,
                effectProb = 1
            };
            buffDelay = 0; buffVFXDelay = 0;
            AddEffect(customSkill, addManual: true);
            buffDelay = defBuffDelay; buffVFXDelay = defVfxDelay;
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
            StartCoroutine(InstVFXDelay(vfx_blue, selfVFX, buffDelay));
            DrawPopup(msg_dispel, "green", buffDelay);
            observer?.UpdateStatuses();
        }
        public void Safeguard()
        {
            if (stun)
            {
                stun = false;
                DrawPopup("- " + msg_stun, "green", buffDelay);
                observer?.UpdateStatuses();
                StartCoroutine(InstVFXDelay(vfx_blue, selfVFX, buffDelay));

            }
            if (defUp_defDown < 0)
            {
                defUp_defDown = 0;
                DrawPopup("- " + msg_defence_down, "green", buffDelay);
                observer?.UpdateStatuses();
                StartCoroutine(InstVFXDelay(vfx_blue, selfVFX, buffDelay));
            }
        }
        public void UpadeteRoundEnd()
        {
            if (focus_blind != 0) focus_blind -= (int)Mathf.Sign(focus_blind);
            if (defUp_defDown != 0) defUp_defDown -= (int)Mathf.Sign(defUp_defDown);
            if (bless_healBlock != 0) bless_healBlock -= (int)Mathf.Sign(bless_healBlock);

            if (immunity != 0) immunity--;
            if (silence != 0) silence--;
            if (curse != 0) curse--;
            foreach (var item in skill) //drop cool down from skills
                if (skillCD[item] > 0) skillCD[item] -= 1;
            foreach (var item in passiveSkill) //drop cool down from skills
                if (skillCD[item] > 0) skillCD[item] -= 1;
            observer?.UpdateStatuses();
        }
        public void UpadeteDot()
        {
            var delay = 0f;
            if (stun)
            {
                DrawPopup(msg_stun, "red");
                stun = false;
                delay = 0.5f;
            }
            if (regen_poison != 0)
            {
                Damage(regen_poison_dot, true, false, false, poison: true);
                regen_poison -= (int)Mathf.Sign(regen_poison);
                delay = 0.5f;
            }
            observer?.UpdateStatuses();
            if (!isDead)
                StartCoroutine(ChangeState(delay));
        }
        IEnumerator ChangeState(float delay = 0.5f)
        {
            yield return new WaitForSeconds(delay);
            bm.StepLate();
        }
        private int popUpCounter = 0;
        void DrawPopup(string msg, string color = "white", float delay = 0f, bool fast = false)
        {
            //set timer if multi-call from wherewer it place
            if (popUpPrefab != null && !isDead)
            {
                var overOffset = isOverlord ? 60 : 0;
                var damageText = Instantiate(popUpPrefab, selfVFX).GetComponent<DamagePopup>();
                damageText.Setup(msg, invertXScale: isEnemy && !isBoss, delay: (float)popUpCounter / 2 + delay, textColor: color, yOffset: -popUpCounter * 30 + overOffset, fast: fast);
                popUpCounter++;
                StartCoroutine(PopCounterAfterLife(1f));
                log.Add(name + ": " + msg);
            }
            else if (popUpPrefab == null)
                log.Add($"Null Draw Popup Prefab: {gameObject.name}", true);
        }
        IEnumerator PopCounterAfterLife(float lifetime)
        {
            yield return new WaitForSeconds(lifetime);
            if (popUpCounter > 0) popUpCounter--;
        }

        private void OnDestroy()
        {
            bm.unselect -= UnHiglight;
            bm.roundEnd -= UpadeteRoundEnd;
            Destroy(observer?.gameObject);
            //if (isEnemy) Destroy(charStats.gameObject);
        }
    }
}