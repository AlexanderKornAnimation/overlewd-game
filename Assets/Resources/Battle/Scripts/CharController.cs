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
        public PSR psr;
        public BattleCry battleCry;
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
        public float damageTotal = 0;


        public float zoomSpeed = 0.15f;
        private float idleScale = 1f, battleScale = 1.4f;
        private float overBattleScale = 1.4f, bossBattleScale = 1.4f;
        public int battleOrder = 1;
        public float health = 100, healthMax = 100;
        public float mana = 100, manaMax = 100;

        public bool isDead = false;
        public bool iHit = false; //check if a character hit any target, then add hitDelay to BattleOut action

        private Transform battleLayer;
        public Transform persPos;
        private Transform battlePos;
        private SpineWidget spineWidget;
        private float defenceDuration = 1f;
        public float preAttackDuration = 1f;
        public float attackDuration = 1f;
        public float vfxDuration = 0f;
        
        private float hitDelay = 1f; //BattleOut step delay
        float effectDefDelay = 1.2f;
        float effectActDelay = 0.4f;
        float buffDelay = 1.2f;
        float buffVFXDelay = 1.2f;

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

            msg_defence_up = "<size=90%><sprite=\"BuffsNDebuffs\" name=\"DefenceUp\"></size> Defence up",
            msg_defence_down = "<size=90%><sprite=\"BuffsNDebuffs\" name=\"DefenceDown\"></size> Defence down",

            msg_regen = "<size=90%><sprite=\"BuffsNDebuffs\" name=\"Regeneration\"></size> Regeneration",
            msg_poison = "<size=90%><sprite=\"BuffsNDebuffs\" name=\"Poison\"></size> Poison",

            msg_focus = "<size=90%><sprite=\"BuffsNDebuffs\" name=\"Focus\"></size> Focus",
            msg_blind = "<size=90%><sprite=\"BuffsNDebuffs\" name=\"Blind\"></size> Blind",

            msg_bless = "<size=90%><sprite=\"BuffsNDebuffs\" name=\"Bless\"></size> Bless",
            msg_healblock = "<size=90%><sprite=\"BuffsNDebuffs\" name=\"HealBlock\"></size> Heal Block",

            msg_immunity = "<size=90%><sprite=\"BuffsNDebuffs\" name=\"Immunity\"></size> Immunity",
            msg_curse = "<size=90%><sprite=\"BuffsNDebuffs\" name=\"Curse\"></size> Curse",
            msg_stun = "<size=90%><sprite=\"BuffsNDebuffs\" name=\"Stun\"></size> Stun",
            msg_silence = "<size=90%><sprite=\"BuffsNDebuffs\" name=\"Silence\"></size> Silence",

            msg_safeguard = "<size=90%><sprite=\"BuffsNDebuffs\" name=\"Safeguard\"></size> Safeguard",
            msg_dispel = "<size=90%><sprite=\"BuffsNDebuffs\" name=\"Dispell\"></size> Dispell";
        private GameObject vfx_purple => bm.vfx.purple;
        private GameObject vfx_red => bm.vfx.red;
        private GameObject vfx_blue => bm.vfx.blue;
        private GameObject vfx_green => bm.vfx.green;
        private GameObject vfx_stun => bm.vfx.stun;
        private GameObject vfx_blood => bm.vfx.blood;

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
            psr = gameObject.AddComponent<PSR>();
            psr.SetupPSR(bm.battleScene.GetBattleData().battleFlow, isEnemy);
            battleCry = gameObject.AddComponent<BattleCry>();
            isOverlord = character.characterClass == AdminBRO.CharacterClass.Overlord;
            battleCry?.SetUp(isEnemy && !isBoss, isBoss, isOverlord ? 100 : 0);
            if (isOverlord)
            {
                bm.overlord = this;
                battleScale = overBattleScale;
            }
            if (isBoss)
            {
                battleScale = bossBattleScale;
            }
            health = (float)character.health;
            healthMax = health;
            mana = (float)character.mana;
            manaMax = mana;
            var skillStash = character.skills;
            foreach (var sk in skillStash)
                skillCD.Add(sk, (int)sk.effectCooldownDuration);
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
                    persPos = battleLayer.Find("Enemy/enemy" + battleOrder.ToString()).transform;
                }
            }
            else
            {
                battlePos = battleLayer.Find("battlePos1").transform;
                persPos = battleLayer.Find("Player/pers" + battleOrder.ToString()).transform;
            }
            transform.SetParent(persPos, false);
            transform.SetSiblingIndex(0);
            if (character.animationData != null)
                spineWidget = SpineWidget.GetInstance(character.animationData, transform);
            else
                log.Add($"{name} animationData is null", error: true);
            //fix out of bounds issues and animator's prefab mistakes
            if (spineWidget != null)
                spineWidget.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 700);
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

        public void PlayIdle(bool loop = true)
        {
            //if (!isDead)
            //{
            character.sfxIdle?.Play();
            spineWidget.PlayAnimation(ani_idle_name, loop);
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
                if (!isOverlord) vfxDuration = 0f;
            }

            yield return new WaitForSeconds(zoomSpeed);
            foreach (var ps in passiveSkill)                            //on_attack Passive Skill Buff
                if (ps.trigger == "on_attack")
                    if (ps.actionType == "heal")
                        AddEffect(ps, passive: true, buff: true); //passive buff
            damageTotal = damage * ((float)skillID.amount);
            damageTotal *= curseDotScale;
            damageTotal = Mathf.Round(damageTotal);
            spineWidget.PlayAnimation(ani_pAttack_name[id], false);     //Play SW                      
            skillSFX[id]?.Play();                                       //SFX

            yield return new WaitForSeconds(preAttackDuration);
            if (skillID.vfxSelf != null)                                //VFX Self
            {
                var vfxGO = new GameObject($"vfx_{skillID.vfxSelf.title}");
                var vfx = vfxGO.AddComponent<VFXManager>();
                vfx.Setup(skillID.vfxSelf, selfVFX);
            }
            if (skillID.shakeScreen)                                    //Shake
                bm.Shake(shakeDuration, shakeStrength);

            if (AOE) yield return new WaitForSeconds(vfxDuration);      //VFX Delay if it need (!=0)

            foreach (var ps in passiveSkill)                            //on_attack Debuff
                if (ps.trigger == "on_attack" && ps.actionType == "damage")
                    if (AOE)
                        if (isEnemy)
                            foreach (var trg in bm.enemyTargetList)
                                AddEffect(ps, trg, passive: true, buff: false); //passive debuff
                        else
                            foreach (var trg in bm.enemyAllyList)
                                AddEffect(ps, trg, passive: true, buff: false); //passive debuff
                    else
                        AddEffect(ps, target, passive: true, buff: false); //passive debuff
            spineWidget.PlayAnimation(ani_attack_name[id], false);

            yield return new WaitForSeconds(attackDuration);
            PlayIdle();
            BattleOut(AOE);
            bm.BattleOut(iHit ? hitDelay : 0f);
            iHit = false;
        }

        public void Defence(CharController attacker, int id, bool aoe = false) => StartCoroutine(PlayDefence(attacker, id, aoe));

        IEnumerator PlayDefence(CharController attacker, int id, bool AOE = false)
        {
            BattleIn(AOE);
            if (stun) PlayIdle();
            var attackerSkill = attacker.skill[id];
            bool HEAL = attackerSkill.actionType == "heal";
            yield return new WaitForSeconds(zoomSpeed);
            transform.SetParent(battlePos);
            UnHiglight();

            yield return new WaitForSeconds(attacker.preAttackDuration + attacker.vfxDuration);
            character.sfxDefense?.Play();
            attackerSkill.sfxTarget?.Play(); //Play on target SFX

            var isHit = (attacker.focus_blind == 0) ? attacker.accuracy + attacker.psr?.accyracy > Random.value
                : (attacker.focus_blind > 0) ? true : false;

            var isDodge = dodge + psr?.dodge > Random.value;
            var isCrit = attacker.critrate + attacker.psr?.crit > Random.value;
            if (isDodge)
            {
                rt.DOAnchorPos(new Vector2(-230, 0), 0.1f);
                psr?.Dodge();
            }
            if (isCrit) attacker.psr?.Crit(); else attacker.psr?.CritMiss();

            if (!HEAL && isHit)
                spineWidget.PlayAnimation(ani_defence_name, false);
            if (isHit)
            {
                if (!isDodge)
                {
                    AddEffect(attackerSkill); //when attack
                    attacker.iHit = true;
                }
                attacker.psr?.HitEnemy();
            }
            else
            {
                attacker.psr?.Miss();
            }
            bool targetVFXisSpawn = false;
            if (attackerSkill.vfxTarget != null && !isDodge) //VFXTarget
            {
                var vfxGO = new GameObject($"vfx_{attackerSkill.vfxTarget.title}");
                var vfx = vfxGO.AddComponent<VFXManager>();
                vfx.Setup(attackerSkill.vfxTarget, selfVFX,invertX: true);
                targetVFXisSpawn = (vfxGO != null);
            }
            if (vfx_blood && !isDodge && !HEAL && isHit && !targetVFXisSpawn)
                Instantiate(vfx_blood, selfVFX);
            foreach (var ps in passiveSkill)
            {
                if (ps?.trigger == "when_attacked")
                    if (ps.actionType == "heal")                  //who is target "heal" = self    !"heal" = enemy
                        AddEffect(ps, passive: true, buff: true); //passive buff
                    else
                        AddEffect(ps, attacker, passive: true, buff: false); //BUG! passive debuff
            }
            var aDamage = attacker.damageTotal;
            float defScale = defUp_defDown != 0 ? aDamage * defUp_defDown_dot * -Mathf.Sign(defUp_defDown) : 0f; //defence up down
            if (HEAL) aDamage = aDamage > 0 ? aDamage *= -1 : aDamage + defScale;
            Damage(aDamage, isHit, isDodge, isCrit, uiDelay: 1.5f, attacker: attacker, aSkill: attackerSkill);

            yield return new WaitForSeconds(defenceDuration);

            if (!HEAL && !stun) //skip play idle to avoid strange loop transitions
                PlayIdle();
            if (stun)
                PlayIdle(false);
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
        public void HighlightForTurn() => observer?.Select(0);
        public void HighlightForHit() => observer?.Select(1);
        public void HighlightForHeal() => observer?.Select(2);
        public void HighlightDeselect() => observer?.Select(-1);

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
            battleCry.ShowBattleCry(); //show battle cry if one of them will be added
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
            float duration = 0;
            if (isOverlord)
            {
                spineWidget.PlayAnimation("defeat1", false);
                yield return new WaitForSeconds(spineWidget.GetAnimationDuaration("defeat1"));
                spineWidget.PlayAnimation("defeat2", true); //!костыль
            }
            else
            {
                var fadeTime = 0.2f;
                spineWidget.PlayAnimation(ani_defeat_name, false);
                duration = spineWidget.GetAnimationDuaration(ani_defeat_name);
                yield return new WaitForSeconds(duration - fadeTime);
                observer?.FadeOut(fadeTime);
                yield return new WaitForSeconds(fadeTime);
                Destroy(gameObject);
            }
        }

        public void Damage(float value, bool hit, bool dodge, bool crit, bool poison = false, float uiDelay = 0f, CharController attacker = null, AdminBRO.CharacterSkill aSkill = null)
        {
            if (hit == false)
            {
                if (aSkill.AOE)
                    DrawPopup("Miss!", "blue");
                else
                    attacker.DrawPopup("Miss!", "blue");
                return;
            }
            if (crit)
            {
                value *= 2;
            }
            value = Mathf.Round(value);
            if (value > 0)
            {
                if (dodge)
                {
                    DrawPopup("Dodge!", "blue");
                    return;
                }
                psr?.TakeDamage();
                health -= value;
                health = Mathf.Round(health);
                health = Mathf.Max(health, 0);
                if (crit)
                    DrawPopup($"Crit!\n{value}", "yellow", fast: true);
                else if (poison)
                    DrawPopup($"-{value}HP", "green", fast: false);
                else
                    DrawPopup($"{value}", "white", fast: true);

                if (health <= 0)
                {
                    if (attacker != null)
                    {
                        if (value >= healthMax)
                            attacker?.battleCry.CallBattleCry(BattleCry.CryEvent.OneShoot);
                        var targets = attacker.isEnemy ? bm.enemyTargetList.Count : bm.enemyAllyList.Count;
                        if (attacker.isBoss && isOverlord) targets -= 1;
                        if (aSkill.AOE && targets > 1)
                            attacker?.battleCry.AddKill(targets, attacker.isBoss);
                    }
                    isDead = true;
                    StartCoroutine(PlayDead(uiDelay));
                    bm.DeadStatesUpdate(this, poison);
                }
                if (value >= healthMax / 2 && !isDead)
                    attacker.battleCry.CallBattleCry(BattleCry.CryEvent.HalfKill);
                StartCoroutine(UIDelay(uiDelay));
            }
            else if (value < 0)
                Heal(value, healer: attacker);
        }

        IEnumerator UIDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            UpdateUI();
        }
        public void Heal(float value, CharController healer = null)
        {
            value = Mathf.Abs(Mathf.Round(value));
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
                DrawPopup($"+{value}HP", "purple");
                if (health >= healthMax && healer != null)
                    healer.battleCry.CallBattleCry(BattleCry.CryEvent.MaxHP);
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
        void AddEffect(AdminBRO.CharacterSkill sk, CharController targetCC = null, bool addManual = false, bool passive = false, bool buff = true)
        {
            if (targetCC == null) targetCC = this;

            if (passive)
            {
                if (skillCD[sk] > 0)
                    return; //this one not a bug this is a feature, first move shut down status effect.
                else
                    skillCD[sk] = (int)sk.effectCooldownDuration;

                bool isCrit = critrate >= Random.value;
                Damage(sk.amount, true, false, isCrit);
            }

            bool hitEffect = sk.effectProb + psr.effectProb >= Random.value;

            if (sk.effect != null)
                if (hitEffect)
                {
                    psr?.EffectHit(); // this line probably make some issues

                    var duration = (int)sk.effectActingDuration;
                    float ea = sk.effectAmount;
                    float effectAmount = healthMax * ea;
                    buffVFXDelay = passive ? effectDefDelay : effectActDelay;
                    buffDelay = passive ? effectDefDelay : effectActDelay;
                    switch (sk.effect)
                    {
                        case "defense_up":
                            if (passive && !buff) { log.Add("Error: defUp on passive debuff", true); return; }
                            defUp_defDown = addManual ? defUp_defDown + duration : duration;
                            defUp_defDown_dot = ea;
                            StartCoroutine(InstVFXDelay(vfx_blue, selfVFX, buffVFXDelay));
                            DrawPopup(msg_defence_up, "yellow", buffDelay, passive: passive);
                            break;
                        case "defense_down":
                            if (passive && buff) { log.Add("Error: defDown on passive buff", true); return; }
                            if (immunity != 0) { DrawPopup(msg_immunity, "green", buffDelay); return; }
                            defUp_defDown = addManual ? defUp_defDown - duration : -duration; // -duration;
                            defUp_defDown_dot = ea;
                            StartCoroutine(InstVFXDelay(vfx_red, selfVFX, buffVFXDelay));
                            DrawPopup(msg_defence_down, "red", buffDelay, passive: passive);
                            break;
                        case "focus":
                            if (passive && !buff) { log.Add("Error: focus on passive debuff", true); return; }
                            focus_blind = addManual ? focus_blind + duration : duration;
                            StartCoroutine(InstVFXDelay(vfx_blue, selfVFX, buffVFXDelay));
                            DrawPopup(msg_focus, "yellow", buffDelay, passive: passive);
                            break;
                        case "blind":
                            if (passive && buff) { log.Add("Error: blind on passive buff", true); return; }
                            if (immunity != 0) { DrawPopup(msg_immunity, "green", buffDelay); return; }
                            focus_blind = addManual ? focus_blind - duration : -duration; //-duration;
                            StartCoroutine(InstVFXDelay(vfx_red, selfVFX, buffVFXDelay));
                            DrawPopup(msg_blind, "red", buffDelay, passive: passive);
                            break;
                        case "regeneration":
                            if (passive && !buff) { log.Add("Error: regen on passive debuff", true); return; }
                            regen_poison = addManual ? regen_poison + duration : duration;
                            regen_poison_dot = -effectAmount;
                            StartCoroutine(InstVFXDelay(vfx_green, selfVFX, buffVFXDelay));
                            DrawPopup(msg_regen, "purple", buffDelay, passive: passive);
                            break;
                        case "poison":
                            if (passive && buff) { log.Add("Error: poison on passive buff", true); return; }
                            if (immunity != 0) { DrawPopup(msg_immunity, "green", buffDelay); return; }
                            regen_poison = addManual ? regen_poison - duration : -duration; //-duration;
                            regen_poison_dot = effectAmount;
                            StartCoroutine(InstVFXDelay(vfx_purple, selfVFX, buffVFXDelay));
                            DrawPopup(msg_poison, "green", buffDelay, passive: passive);
                            break;
                        case "bless":
                            if (passive && !buff) { log.Add("Error: bless on passive debuff", true); return; }
                            bless_healBlock = addManual ? bless_healBlock + duration : duration;
                            bless_dot = effectAmount;
                            StartCoroutine(InstVFXDelay(vfx_blue, selfVFX, buffVFXDelay));
                            DrawPopup(msg_bless, "yellow", buffDelay, passive: passive);
                            break;
                        case "heal_block":
                            if (passive && buff) { log.Add("Error: heal block on passive buff", true); return; }
                            if (immunity != 0) { DrawPopup(msg_immunity, "green", buffDelay); return; }
                            bless_healBlock = addManual ? bless_healBlock - duration : -duration;//-duration;
                            StartCoroutine(InstVFXDelay(vfx_red, selfVFX, buffVFXDelay));
                            DrawPopup(msg_healblock, "red", buffDelay, passive: passive);
                            break;
                        case "silence":
                            if (passive && buff) { log.Add("Error: silence on passive buff", true); return; }
                            if (immunity != 0) { DrawPopup(msg_immunity, "green", buffDelay); return; }
                            silence = addManual ? silence + duration : duration;
                            StartCoroutine(InstVFXDelay(vfx_red, selfVFX, buffVFXDelay));
                            DrawPopup(msg_silence, "red", buffDelay, passive: passive);
                            break;
                        case "immunity":
                            if (passive && !buff) { log.Add("Error: immunity on passive debuff", true); return; }
                            immunity = addManual ? immunity + duration : duration;
                            StartCoroutine(InstVFXDelay(vfx_blue, selfVFX, buffVFXDelay));
                            DrawPopup(msg_immunity, "yellow", buffDelay, passive: passive);
                            break;
                        case "stun":
                            if (passive && buff) { log.Add("Error: stun on passive buff", true); return; }
                            if (immunity != 0) { DrawPopup(msg_immunity, "green", buffDelay); return; }
                            stun = true;
                            StartCoroutine(InstVFXDelay(vfx_stun, selfVFX, buffVFXDelay));
                            DrawPopup(msg_stun, "red", buffDelay, passive: passive);
                            break;
                        case "curse":
                            if (passive && buff) { log.Add("Error: curse on passive buff", true); return; }
                            if (immunity != 0) { DrawPopup(msg_immunity, "green", buffDelay); return; }
                            curse = addManual ? curse + duration : duration;
                            curse_dot = ea; //Calculate in total damage
                            StartCoroutine(InstVFXDelay(vfx_red, selfVFX, buffVFXDelay));
                            DrawPopup(msg_curse, "red", buffDelay, passive: passive);
                            break;
                        case "dispel":
                            Dispel(passive: passive);
                            break;
                        case "safeguard":
                            if (passive && !buff) { log.Add("Error: safeguard on passive debuff", true); return; }
                            Safeguard(passive: passive);
                            break;
                        default:
                            log.Add($"Unknow Value AdminBRO.CharacterSkill.effect: {sk.effect}", true);
                            break;
                    }
                    observer?.UpdateStatuses();
                }
                else
                {
                    psr.EffectMiss(); //DrawPopup("Effect miss", "red");
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
            AddEffect(customSkill, addManual: true); //custom add
        }
        public void Dispel(bool passive = false)
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
            DrawPopup(msg_dispel, "green", buffDelay, passive: passive);
            observer?.UpdateStatuses();
        }
        public void Safeguard(bool passive = false)
        {
            if (stun)
            {
                stun = false;
                DrawPopup("- " + msg_stun, "green", buffDelay, passive: passive);
                observer?.UpdateStatuses();
                StartCoroutine(InstVFXDelay(vfx_blue, selfVFX, buffDelay));
            }
            if (defUp_defDown < 0)
            {
                defUp_defDown = 0;
                DrawPopup("- " + msg_defence_down, "green", buffDelay, passive: passive);
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
            if (regen_poison != 0)
            {
                Damage(regen_poison_dot, true, false, false, poison: true);
                regen_poison -= (int)Mathf.Sign(regen_poison);
                delay = 0.75f;
            }
            if (stun && !isDead)
            {
                DrawPopup(msg_stun, "red");
                delay = 0.5f;
            }
            observer?.UpdateStatuses();
            if (!isDead)
                StartCoroutine(ChangeState(delay));
        }
        IEnumerator ChangeState(float delay = 0.5f)
        {
            yield return new WaitForSeconds(delay);
            bm.StepLate(stun);
            stun = false;
            PlayIdle();
            observer?.UpdateStatuses();
        }
        private int popUpCounter = 0;
        void DrawPopup(string msg, string color = "white", float delay = 0f, bool fast = false, bool passive = false)
        {
            //set timer if multi-call from wherewer it place
            if (popUpPrefab != null && !isDead)
            {
                var overOffset = isOverlord ? 60 : 0;
                var damageText = Instantiate(popUpPrefab, selfVFX).GetComponent<DamagePopup>();
                if (passive)
                    msg = "<color=#E8D9AC><pos=-5%><size=50%>Passive skill</color></pos></size>\n" + msg;
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
            Destroy(observer?.selector.gameObject);
        }
    }
}