using DG.Tweening;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public enum BattleEvent
    {
        EventId,

    }

    public class BattleManager : MonoBehaviour
    {
        private BaseBattleScreen battleScene; //In Out of Stage
        [HideInInspector] public CharController overlord;
        [HideInInspector] public List<CharController> charControllerList;
        [HideInInspector] public List<CharController> enemyTargetList;
        [HideInInspector] public List<CharController> enemyAllyList;
        [HideInInspector] public List<QueuePortraitController> QueueElements = null;
        [SerializeField] private float portraitScale = 1.5f;
        [Tooltip("selected as current turn")] public CharController ccOnSelect;
        [Tooltip("selected as target")] public CharController ccTarget = null;
        public Animator ani, charAni;

        //new init
        private AdminBRO.Battle battleData => battleScene.GetBattleData().battleData;
        public BattleLog log => GetComponent<BattleLog>();
        bool bossLevel => battleData.isTypeBoss;
        private List<AdminBRO.Character> playerTeam => battleScene.GetBattleData().myTeam;
        private List<AdminBRO.Character> enemyTeam;

        private Transform BattleCanvas => transform.Find("BattleCanvas");
        public GameObject portraitPrefab; //attach to BM in inspector; "content" is ui spawn point
        public Transform QueueUI;
        private Transform QueueUIContent;
        public TextMeshProUGUI wavesTMP;
        public TextMeshProUGUI roundTMP;
        public Transform EnemyStatsContent;
        [Tooltip("add this prefab here BattleUICanvas/Character/PlayerStats")]
        public CharacterPortrait PlayerStats;
        public CharacterPortrait EnemyStats;
        Transform bPosPlayer, bPosEnemy;
        int siblingPlayer, siblingEnemy;

        private SkillController[] skillControllers = new SkillController[3];
        private SkillController[] passiveControllers = new SkillController[2];
        private SkillController potion_mp, potion_hp;
        private RectTransform skillPanelWidthScale;
        private int skillOnSelect = -1; //-1 unselect
        private int charOnSelect = -1; // 0 - allay 1 - enemy
        public GameObject vfx_purple, vfx_red, vfx_blue, vfx_green, vfx_stun;

        private int enemyNum = 0, enemyIsDead = 0, charNum = 0, charIsDead = 0;
        private int step = 0; //current characters list step
        private int maxStep = 2; //max count of battle queue, take from character's list
        private int wave = 0, maxWave = 2; //Wave count
        private int round = 1;
        public delegate void Unselect();
        public delegate void RoundEnd();
        public Unselect unselect;
        public RoundEnd roundEnd;
        [SerializeField] private Color redColor;
        private Transform aniDropPoint;
        [SerializeField] private GameObject aniRound, aniBoss, aniWave;

        //statistic, trackers, notif flags
        private bool castAOEForNotify = false;
        private bool battleStart = false;

        //potions
        int potionMP => battleScene.GetBattleData().mana;
        int potionHP => battleScene.GetBattleData().hp;

        int usedMP = 0, usedHP = 0;

        public int debug = 0;
        [SerializeField]
        GameObject debugObj;

        private void Start()
        {
            battleScene = FindObjectOfType<BaseBattleScreen>();

            enemyTeam = battleScene.GetBattleData().enemyWaves[wave].enemyTeam; //wave 0
            maxWave = battleScene.GetBattleData().enemyWaves.Count - 1;
            Debug.Log($"Battle ID: {battleData.id}");

            if (QueueUI == null) QueueUI = transform.Find("BattleUICanvas/QueueUI");
            QueueUIContent = QueueUI.Find("Content");
            if (wavesTMP == null) wavesTMP = QueueUI.Find("text_Waves").GetComponent<TextMeshProUGUI>();
            if (wavesTMP) wavesTMP.text = $"Wave {wave + 1}/{maxWave + 1}";
            if (roundTMP == null) roundTMP = transform.Find("BattleUICanvas/Background/Round/text").GetComponent<TextMeshProUGUI>();
            if (EnemyStatsContent == null) EnemyStatsContent = transform.Find("BattleUICanvas/Enemys/Content.enemy");
            if (PlayerStats == null) PlayerStats = transform.Find("BattleUICanvas/Character/PlayerStats").GetComponent<CharacterPortrait>();
            if (EnemyStats == null) EnemyStats = transform.Find("BattleUICanvas/Character/EnemyStats").GetComponent<CharacterPortrait>();
            EnemyStats.isBoss = bossLevel;

            if (bossLevel) QueueUI.gameObject.SetActive(false);
            if (portraitPrefab == null) portraitPrefab = Resources.Load("Battle/Prefabs/Battle/Portrait") as GameObject;

            bPosPlayer = transform.Find("BattleCanvas/BattleLayer/battlePos1");
            bPosEnemy = transform.Find("BattleCanvas/BattleLayer/battlePos2");
            siblingPlayer = bPosPlayer.GetSiblingIndex();
            siblingEnemy = bPosEnemy.GetSiblingIndex();

            for (int i = 0; i < skillControllers.Length; i++)
            {
                var x = i; //Captured variable issue
                skillControllers[x] = transform.Find($"BattleUICanvas/Character/Buttons/Skills/Button_{x}").GetComponent<SkillController>();
                skillControllers[x].OnClickAction.AddListener(delegate { ButtonPress(x); });
            }
            skillPanelWidthScale = transform.Find("BattleUICanvas/Character/Buttons/Skills/").GetComponent<RectTransform>();
            passiveControllers[0] = transform.Find("BattleUICanvas/Character/Buttons/Skills/Button_0/Passives/Button_0").GetComponent<SkillController>();
            passiveControllers[1] = transform.Find("BattleUICanvas/Character/Buttons/Skills/Button_0/Passives/Button_1").GetComponent<SkillController>();
            if (potion_mp == null) potion_mp = transform.Find("BattleUICanvas/Character/Buttons/Bottles/Potion_mp").GetComponent<SkillController>();
            if (potion_hp == null) potion_hp = transform.Find("BattleUICanvas/Character/Buttons/Bottles/Potion_hp").GetComponent<SkillController>();
            potion_hp.potionAmount = potionHP;
            potion_mp.potionAmount = potionMP;
            potion_mp?.OnClickAction.AddListener(UseMPPotion);
            potion_hp?.OnClickAction.AddListener(UseHPPotion);

            transform.Find("BattleUICanvas/Character/Buttons/Bottles/").gameObject.SetActive(ManaPotionsChecker()); //turn off bottles

            DropCharactersFromList(playerTeam, isEnemy: false);
            DropCharactersFromList(enemyTeam, isEnemy: true);

            charControllerList.Sort(SortBySpeed); //Sorting then create portraits

            CreatePortraitQueue();
            maxStep = charControllerList.Count;
            if (!bossLevel) QueueElements[0].Select(); //Scale Up First Element
            ccOnSelect = charControllerList[step];

            aniDropPoint = transform.Find("Animations");
            
            StartCoroutine(LateInit());
            StartCoroutine(ShowStartAnimation());
            StartCoroutine(PreBattlePause());
        }
        IEnumerator LateInit()
        {
            yield return new WaitForEndOfFrame();
            SetSkillCtrl(ccOnSelect);
            ccOnSelect.CharPortraitSet();
        }
        IEnumerator ShowStartAnimation()
        {
            yield return new WaitForSeconds(.7f);
            if (bossLevel && aniBoss)
                Instantiate(aniBoss, aniDropPoint);
            else if (aniWave)
                Instantiate(aniWave, aniDropPoint).GetComponent<AnimationController>().SetUp("Wave 1");
            else if (aniRound)
                Instantiate(aniRound, aniDropPoint).GetComponent<AnimationController>().SetUp("Round 1");
        }
        IEnumerator PreBattlePause()
        {
            yield return new WaitForSeconds(1.75f);
            if (ccOnSelect.isEnemy)
            {
                battleState = BattleState.ENEMY;
                prevState = battleState;
                if (!ccOnSelect.isBoss)
                    bPosEnemy.SetSiblingIndex(siblingEnemy + 1);
                StartCoroutine(EnemyAttack());
                charAni.SetTrigger("startEnemy");
            }
            else
            {
                battleState = BattleState.PLAYER;
                bPosPlayer.SetSiblingIndex(siblingPlayer + 1);
                charAni.SetTrigger("startPlayer");
            }

            charControllerList[step].Highlight();
            if (battleState == BattleState.PLAYER && !ccOnSelect.skill[0].AOE)
                ButtonPress(0);
            
            if (!battleStart) //skip button = true; back button = false
            {
                if (battleScene != null)
                    battleScene.StartBattle();
                battleStart = true;
            }
        }
        void DropCharactersFromList(List<AdminBRO.Character> characterList, bool isEnemy)
        {
            var orderCount = 3;
            var k = 1;
            foreach (var c in characterList)
            {
                var charGO = new GameObject($"{c.name}_{k}");
                var cc = charGO.AddComponent<CharController>();
                cc.character = c;
                charControllerList.Add(cc);
                cc.bm = this;
                unselect += cc.UnHiglight;
                roundEnd += cc.UpadeteRoundEnd;
                cc.battleOrder = orderCount;

                if (isEnemy)
                {
                    cc.isEnemy = true; //Add Enemy flag
                    cc.isBoss = bossLevel;
                    cc.charStats = EnemyStats;
                    enemyAllyList.Add(cc);
                    enemyNum++;
                }
                else
                {
                    enemyTargetList.Add(cc);
                    cc.charStats = PlayerStats;
                    charNum++;
                }
                orderCount--;
                k++;
            }
        }

        IEnumerator NextWave()
        {
            yield return new WaitForSeconds(3.5f);
            step = 0;
            if (wavesTMP) wavesTMP.text = $"Wave {wave + 1}/{maxWave + 1}";
            //Destroy phase ============================================================
            foreach (var cc in charControllerList)
                if (cc.isEnemy)
                {
                    Destroy(cc.charStats.gameObject);
                    Destroy(cc.gameObject);
                }
            charControllerList.RemoveAll(item => item.isEnemy == true);
            //destroy portraits queue
            foreach (QueuePortraitController item in QueueElements) //delete all portraits content
                Destroy(item.gameObject);
            QueueElements.Clear();
            enemyAllyList.Clear(); //Clear Ally List before add new
            //Create phase =============================================================
            var waveList = battleScene.GetBattleData().enemyWaves[wave].enemyTeam;
            DropCharactersFromList(waveList, true); //create
            charControllerList.Sort(SortBySpeed);   //sort
            maxStep = charControllerList.Count;
            CreatePortraitQueue();                  //drop new portraits with sorting
            QueueElements[0].Select(); //Scale Up First Element
            ccOnSelect = charControllerList[step];
            battleState = BattleState.ANIMATION;
            ccOnSelect.UpadeteDot();
            if (aniWave)
                Instantiate(aniWave, aniDropPoint).GetComponent<AnimationController>().SetUp($"Wave {wave + 1}");
        }
        private void CreatePortraitQueue()
        {
            if (!bossLevel)
                foreach (var cc in charControllerList) //Fill QueueUI characters icons
                {
                    var portraitQ = Instantiate(portraitPrefab, QueueUIContent).GetComponent<QueuePortraitController>();
                    portraitQ.name = "Portrait_" + cc.Name;
                    portraitQ.SetUp(cc);
                    if (cc.isEnemy)
                        portraitQ.transform.GetComponent<Image>().color = redColor;  //Switch color on portrait indicator from blue to red
                    QueueElements.Add(portraitQ);
                }
            else return;
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
                foreach (var cc in charControllerList)
                    if (cc.isEnemy == true && !cc.isDead)
                    {
                        cc.health = cc.healthMax;
                        cc.UpdateUI();
                    }
        }

        public bool CharPress(CharController ccOnPress)
        {
            if (skillOnSelect == -1)
            {
                if (charOnSelect != -1 && ccTarget == ccOnPress)
                {
                    unselect?.Invoke();
                    charOnSelect = -1;
                    ccTarget = null;
                    foreach (var sc in skillControllers)
                        sc.Enable();
                    return false;
                }
                else
                {
                    ccTarget = ccOnPress;
                    charOnSelect = ccOnPress.isEnemy ? 1 : 0;
                    foreach (var sc in skillControllers)
                    {
                        sc.Enable();
                        if (ccTarget.isEnemy == sc.isHeal || !ccTarget.isEnemy == !sc.isHeal)
                            sc.Disable(); //desable conflict buttons
                    }
                    AttackCheck();
                    return true;
                }
            }
            else
            {
                if (skillControllers[skillOnSelect].isHeal == ccOnPress.isEnemy)
                {
                    foreach (var item in skillControllers)
                        if (item.isHeal == ccOnPress.isEnemy)
                            item.BlinkDisable();
                    return false;
                }
                else
                {
                    ccTarget = ccOnPress;
                    charOnSelect = ccOnPress.isEnemy ? 1 : 0;
                    AttackCheck();
                    return true;
                }
            }
        }

        private void ButtonPress(int id)
        {
            var sc = skillControllers[id];
            var AOE = sc.skill.AOE;

            if (sc.CheckMana(ccOnSelect.mana) && !sc.SkillOnCD())
            {
                if (sc.selectable && !sc.silence)
                {
                    if (AOE)
                    {
                        AttackAction(id);
                        return;
                    }
                    if (skillOnSelect > -1 && skillOnSelect != id)
                        skillControllers[skillOnSelect]?.Unselect();
                    skillOnSelect = sc.Press() ? id : -1;
                    AttackCheck();
                }
                else if (sc.silence)
                {
                    sc.BlinkDisable();
                }
            }
            else
            {
                sc.BlinkDisable();
            }
        }

        private void AttackCheck()
        {
            if (skillOnSelect != -1 && charOnSelect != -1)
            {
                AttackAction(skillOnSelect);
                skillControllers[skillOnSelect]?.Unselect();
                skillOnSelect = -1;
                charOnSelect = -1;
            }
        }

        private void AttackAction(int id, bool isEnemyAttack = false)
        {
            if (battleState == BattleState.PLAYER || battleState == BattleState.ENEMY)
            {
                if (id > ccOnSelect.skill.Count) id = 0;
                unselect?.Invoke();
                bool AOE = ccOnSelect.skill[id].AOE;
                bool HEAL = ccOnSelect.skill[id].actionType == "heal";
                ccOnSelect.ManaReduce(ccOnSelect.skill[id].manaCost);
                if (AOE)
                {
                    ani.SetTrigger("General");
                    ccTarget = null;
                    ccOnSelect.Attack(id, true);
                    if (isEnemyAttack)
                        HEAL = !HEAL;
                    foreach (var cc in charControllerList)
                        if (!cc.isDead && cc.isEnemy != HEAL)
                        {
                            cc.Defence(ccOnSelect, id, aoe: true);
                            cc.UnHiglight();
                        }
                    if (ccOnSelect.isOverlord && id > 1)
                        castAOEForNotify = true; //for battle notify
                }
                else
                {
                    if (HEAL)
                        ani.SetTrigger("General");
                    else if (isEnemyAttack)
                        ani.SetTrigger("Enemy");
                    else
                        ani.SetTrigger("Player");
                    ccOnSelect.Attack(id, AOE: HEAL, ccTarget);
                    ccTarget.Defence(ccOnSelect, id, aoe: HEAL);
                }
                ccOnSelect.skillCD[ccOnSelect.skill[id]] = Mathf.RoundToInt(ccOnSelect.skill[id].effectCooldownDuration);
                battleState = BattleState.ANIMATION;
            }
        }
        public void Shake(float duration, float strenght) =>
            BattleCanvas.DOShakePosition(duration, strenght);

        IEnumerator EnemyAttack()
        {
            //Must be empty
            yield return new WaitForSeconds(1.5f); //Pause then show target stats
            if (!ccOnSelect.isDead)
            {
                int id = (ccOnSelect.skillCD[ccOnSelect.skill[1]] == 0) ? Random.Range(0, ccOnSelect.skill.Count) : 0;
                if (ccOnSelect.skill[id].AOE)
                    ccTarget = null;
                else if (ccOnSelect.skill[id].actionType == "heal")
                    ccTarget = enemyAllyList[Random.Range(0, enemyAllyList.Count)];
                else
                    ccTarget = enemyTargetList[Random.Range(0, enemyTargetList.Count)];
                ccTarget?.Highlight();
                //ccTarget?.CharPortraitSet();
                yield return new WaitForSeconds(0.5f);
                AttackAction(id, isEnemyAttack: true);
            }
        }
        public void UseHPPotion()
        {
            if (potion_hp.potionAmount > 0 && overlord.health < overlord.healthMax && !overlord.isDead)
                if (battleState == BattleState.PLAYER)
                {
                    SoundManager.PlayOneShot(FMODEventPath.UI_Battle_Healpotion);
                    overlord.Heal(Mathf.RoundToInt(overlord.healthMax * 0.2f));
                    potion_hp.InstVFX(overlord.persPos);
                    potion_hp.potionAmount--;
                    potion_hp.StatUpdate();
                    usedHP++;
                }
        }
        public void UseMPPotion()
        {
            if (potion_mp.potionAmount > 0 && overlord.mana < overlord.manaMax && !overlord.isDead)
                if (battleState == BattleState.PLAYER)
                {
                    SoundManager.PlayOneShot(FMODEventPath.UI_Battle_Manapotion);
                    overlord.HealMP(Mathf.RoundToInt(overlord.manaMax * 0.2f));
                    potion_mp.InstVFX(overlord.persPos);
                    potion_mp.potionAmount--;
                    potion_mp.StatUpdate();
                    usedMP++;
                }
        }
        public void UnselectButtons()
        {
            foreach (var item in skillControllers)
                item.Unselect();
        }
        public void BattleOut()
        {
            UnselectButtons();
            ani.SetTrigger("BattleOut");
            if (battleState != BattleState.LOSE && battleState != BattleState.WIN && battleState != BattleState.NEXTWAVE)
            {
                Step();
                unselect?.Invoke();
            }
            charOnSelect = -1;
            ccTarget = null;
        }

        public void DeadStatesUpdate(CharController invoker, bool poison = false) //call when any character is dead
        {
            var index = charControllerList.FindIndex(x => x == invoker);
            if (!bossLevel)
            { //Destroy queue portrait
                var qe = QueueElements.Find(f => f.cc == invoker);
                Destroy(qe.gameObject);
                QueueElements.Remove(qe);
            }
            charControllerList.Remove(invoker);

            if (invoker.isEnemy)
                enemyIsDead++;
            else
            {
                charIsDead++;
                enemyTargetList.Remove(invoker); //remove character from enemy target list
            }
            if (enemyNum == enemyIsDead)
            {
                if (wave == maxWave)
                {
                    //next wave function or
                    battleState = BattleState.WIN;
                    StartCoroutine(WinScreenWithDelay());
                    Debug.Log("WINNIG");
                }
                else
                {
                    battleState = BattleState.NEXTWAVE;
                    wave++;
                    StartCoroutine(NextWave());
                    BattleNotif("chapter1", "battle1", "battletutor3");
                }
            }
            if (charNum == charIsDead)
            {
                battleState = BattleState.LOSE;
                if (battleScene != null)
                    battleScene.EndBattle(new BattleManagerOutData
                    {
                        battleWin = false,
                        manaSpent = usedMP,
                        hpSpent = usedHP
                    });
                if (CheckBattleGameData("chapter1", "battle2"))
                    Debug.Log("LOOSING");
            }
            if (battleState == BattleState.ANIMATION && poison)
                StartCoroutine(PoisonStep()); //if we dead from DOT on start move we call next step
        }
        IEnumerator PoisonStep()
        {
            yield return new WaitForSeconds(0.5f);
            Step();
        }
        IEnumerator WinScreenWithDelay()
        {
            yield return new WaitForSeconds(3.5f);
            if (battleScene != null)
                battleScene.EndBattle(new BattleManagerOutData
                {
                    battleWin = true,
                    manaSpent = usedMP,
                    hpSpent = usedHP
                });
        }
        
        private void Step()
        {
            log.Add("Battle Manager: Step");

            var qe = QueueElements.Find(i => i.cc == ccOnSelect);
            qe?.Deselect();
            maxStep = charControllerList.Count;
            if (++step >= maxStep)
            {
                if (round == 1)
                {
                    BattleNotif("chapter1", "battle1", "battletutor2"); //one round later
                }
                roundEnd?.Invoke(); //drop some statuses
                round++;
                roundTMP.text = $"Round {round}";
                log.Add($"Round {round}");
                step = 0;
                if (aniRound)
                    Instantiate(aniRound, aniDropPoint).GetComponent<AnimationController>().SetUp($"Round {round}");
            }
            if (castAOEForNotify == true)
            {
                BattleNotif("chapter1", "battle5", "potionstutor3"); //AOE Cast
            }
            ccOnSelect = charControllerList[step];
            if (!bossLevel)
                QueueElements.Find(i => i?.cc == ccOnSelect).Select();

            bPosEnemy.SetSiblingIndex(siblingEnemy);
            bPosPlayer.SetSiblingIndex(siblingPlayer);
            battleState = BattleState.ANIMATION;
            ccOnSelect.UpadeteDot();
        }
        public void StepLate()
        {
            if (battleState != BattleState.LOSE && battleState != BattleState.WIN && battleState != BattleState.NEXTWAVE)
            {
                battleState = ccOnSelect.isEnemy ? BattleState.ENEMY : BattleState.PLAYER;
                ccOnSelect.CharPortraitSet();
                if (ccOnSelect.isEnemy)
                {
                    battleState = BattleState.ENEMY;
                    if (!ccOnSelect.isBoss)
                        bPosEnemy.SetSiblingIndex(siblingEnemy + 1);
                    StartCoroutine(EnemyAttack());
                    //Do not restart the animation if previous character on the same team
                    if (battleState != prevState)
                        charAni.SetTrigger("enemy");
                }
                else
                {
                    battleState = BattleState.PLAYER;
                    SetSkillCtrl(ccOnSelect);
                    bPosPlayer.SetSiblingIndex(siblingPlayer + 1);
                    if (!ccOnSelect.skill[0].AOE) ButtonPress(0);
                    if (battleState != prevState)
                        charAni.SetTrigger("player");
                }
                prevState = battleState; //save prew state for portrait animation
                ccOnSelect.Highlight();
            }
        }

        private void SetSkillCtrl(CharController cc)
        {
            var i = cc.skill?.Count;
            var j = 0;
            bool silent = cc.silence > 0;
            foreach (var sk in skillControllers)
            {
                sk.gameObject.SetActive(true);
                if (i > 0)
                {
                    sk.ReplaceSkill(cc.skill[j], cc.skillCD, cc.isOverlord);
                    if (j != 0) sk.silence = silent; //silence realisation
                }
                else
                    sk.gameObject.SetActive(false);
                i--;
                j++;
            }

            i = cc.passiveSkill?.Count;
            j = 0;
            bool k = cc.passiveSkill.Any();
            foreach (var sk in passiveControllers)
            {
                sk.gameObject.SetActive(k);
                if (i > 0)
                    sk.ReplaceSkill(cc.passiveSkill[j], cc.skillCD, cc.isOverlord);
                else
                    sk.gameObject.SetActive(false);
                i--;
                j++;
            }
        }

        public void BattleNotif(string chapterID, string battleID, string notifID = null)
        {
            if (battleScene.GetBattleData() != null)
                if (chapterID == battleScene.GetBattleData().ftueChapterKey &&
                    battleID == battleScene.GetBattleData().ftueStageKey)
                {
                    Debug.Log($"{chapterID} {battleID} {notifID}");
                    battleScene.OnBattleNotification(battleID, chapterID, notifID);
                }
        }
        private bool ManaPotionsChecker()
        {
            if (battleScene.GetBattleData().ftueChapterKey == "chapter1")
            {
                string[] battlesToCheck = { "battle1", "battle2", "battle3", "battle4" };
                var stageKey = battleScene.GetBattleData().ftueStageKey;
                foreach (var item in battlesToCheck)
                    if (item == stageKey)
                        return false;
            }
            return true;
        }
        public bool MagicGuildChecker() => GameData.buildings.magicGuild.meta.isBuilt;

        public bool CheckBattleGameData(string chapterID, string battleID)
        {
            return false;

            if (battleScene.GetBattleData() != null)
                return chapterID == battleScene.GetBattleData().ftueChapterKey &&
                       battleID == battleScene.GetBattleData().ftueStageKey;
            else
                return false;
        }
        public void AfterShowBattleScreen()
        {
            BattleNotif("chapter1", "battle1", "battletutor1");
            BattleNotif("chapter1", "battle3", "battletutor4");
            BattleNotif("chapter1", "battle5", "potionstutor2");
            BattleNotif("chapter1", "battle4", "bosstutor");
        }
        public void AddStatus(string effect)
        {
            if (ccTarget != null)
                ccTarget.AddEffectManual(effect);
            else
                log.Add("Please select character");
        }
        private void OnGUI()
        {
            if (GUI.Button(new Rect(Screen.width / 2, Screen.height - 64, 72, 32), "Debug"))
                if (debug < 3) debug++; else debug = 0;
            if (debug > 0)
            {
                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.white;
                style.fontSize = 22;

                GUI.Label(new Rect(Screen.width / 2 - 80, 64, 300, 500), $"Battle ID: {battleData.id}\n" +
                    $"is BossLevel {battleData.isTypeBoss}", style);
            }
            debugObj?.SetActive(debug > 0);
        }

        private int SortBySpeed(CharController a, CharController b)
        {
            if (a.character.speed < b.character.speed)
                return 1;
            else if (a.character.speed > b.character.speed)
                return -1;
            else if (a.isEnemy)
                return 1;
            else if (b.isEnemy)
                return -1;
            return 0;
        }

        public enum BattleState { PLAYER, ENEMY, ANIMATION, INIT, WIN, LOSE, NEXTWAVE }
        public BattleState battleState = BattleState.INIT;
        private BattleState prevState = BattleState.PLAYER;
    }
}