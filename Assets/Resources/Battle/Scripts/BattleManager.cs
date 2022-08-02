using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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
        [HideInInspector] public List<GameObject> QueueElements;
        [SerializeField] private float portraitScale = 1.5f;
        [Tooltip("selected as current turn")] public CharController ccOnSelect;
        [Tooltip("selected as target")] public CharController ccTarget = null;
        public Animator ani;

        public List<CharacterRes> characterResList; //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

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
        public GameObject EnemyStats;
        public Transform EnemyStatsContent;
        [Tooltip("add this prefab here BattleUICanvas/Character/PlayerStats")]
        public CharacterPortrait PlayerStats;
        public CharacterPortrait BossStats;
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

        private Color redColor;

        //statistic, trackers, notif flags
        private bool castAOEForNotify = false;
        private bool battleStart = false;
        private bool hidePotion = false;
        private bool hideAOE = false;
        private bool notifIsShow = false;

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
            PlayerStats.bigPortrait = true;
            if (bossLevel)
            {
                if (BossStats == null) BossStats = transform.Find("BattleUICanvas/Character/BossStats").GetComponent<CharacterPortrait>();
                BossStats.bigPortrait = true;
                BossStats.gameObject.SetActive(true);
                QueueUI.gameObject.SetActive(false);
            }
            if (portraitPrefab == null) portraitPrefab = Resources.Load("Battle/Prefabs/Battle/Portrait") as GameObject;
            if (EnemyStats == null) EnemyStats = Resources.Load("Battle/Prefabs/Battle/EnemyStats") as GameObject;

            bPosPlayer = transform.Find("BattleCanvas/BattleLayer/battlePos1");
            bPosEnemy  = transform.Find("BattleCanvas/BattleLayer/battlePos2");
            siblingPlayer = bPosPlayer.GetSiblingIndex();
            siblingEnemy  = bPosEnemy.GetSiblingIndex();

            for (int i = 0; i < skillControllers.Length; i++)
            {
                var x = i; //Captured variable issue
                skillControllers[x] = transform.Find($"BattleUICanvas/Character/Buttons/Skills/Button_{x}").GetComponent<SkillController>();
                skillControllers[x].OnClickAction.AddListener(delegate { ButtonPress(x); });
            }
            skillPanelWidthScale = transform.Find("BattleUICanvas/Character/Buttons/Skills/").GetComponent<RectTransform>();
            passiveControllers[0] = transform.Find("BattleUICanvas/Character/Buttons/Passives/Button_0").GetComponent<SkillController>();
            passiveControllers[1] = transform.Find("BattleUICanvas/Character/Buttons/Passives/Button_1").GetComponent<SkillController>();
            if (potion_mp == null) potion_mp = transform.Find("BattleUICanvas/Character/Buttons/Bottles/Potion_mp").GetComponent<SkillController>();
            if (potion_hp == null) potion_hp = transform.Find("BattleUICanvas/Character/Buttons/Bottles/Potion_hp").GetComponent<SkillController>();
            potion_hp.potionAmount = potionHP;
            potion_mp.potionAmount = potionMP;
            potion_mp?.OnClickAction.AddListener(UseMPPotion);
            potion_hp?.OnClickAction.AddListener(UseHPPotion);

            /*if (battleSettings.hidePotions)
            {
                potion_hp.gameObject.SetActive(false);
                potion_mp.gameObject.SetActive(false);
            }*/

            if (characterResList == null)
                characterResList = new List<CharacterRes>(Resources.LoadAll<CharacterRes>("Battle/BattlePersonages/Profiles"));

            DropCharactersFromList(playerTeam, isEnemy: false);
            DropCharactersFromList(enemyTeam, isEnemy: true);

            charControllerList.Sort(SortBySpeed); //Sorting then create portraits

            ColorUtility.TryParseHtmlString("#A64646", out redColor);
            CreatePortraitQueue();

            maxStep = charControllerList.Count;
            QueueElements[step].transform.localScale *= portraitScale; //Scale Up First Element
            if (charControllerList[0].isEnemy)
            {
                battleState = BattleState.ENEMY;
                if (!ccOnSelect.isBoss)
                    bPosEnemy.SetSiblingIndex(siblingEnemy + 1);
                StartCoroutine(EnemyAttack());
            }
            else
            {
                battleState = BattleState.PLAYER;
                bPosPlayer.SetSiblingIndex(siblingPlayer + 1);
            }
            ccOnSelect = charControllerList[step];
            StartCoroutine(LateInit());
        }
        IEnumerator LateInit()
        {
            yield return new WaitForSeconds(0.01f);
            charControllerList[step].Highlight();
            SetSkillCtrl(ccOnSelect);
            if (battleState == BattleState.PLAYER)
                ccOnSelect.CharPortraitSet();

            //if (battleSettings.powerBuff)
            //    WinOrLose(wannaWin);
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

                var cRes = characterResList?.Find(item => item.key == c.key);
                var defaultSkin = bossLevel ? characterResList[3] : characterResList[2];
                cc.characterRes = cRes == null ? defaultSkin : cRes; //load default skin if key not found

                charControllerList.Add(cc);
                cc.bm = this;
                unselect += cc.UnHiglight;
                roundEnd += cc.UpadeteRoundEnd;
                cc.battleOrder = orderCount;

                if (isEnemy)
                {
                    cc.isEnemy = true; //Add Enemy flag
                    if (bossLevel)
                    {
                        cc.isBoss = bossLevel;
                        cc.charStats = BossStats;
                    }
                    else
                    {
                        var pos = EnemyStatsContent.Find($"portrait_pos_{orderCount}"); //Drop portrait on Content Queue and turn on
                        pos.gameObject.SetActive(true);
                        var eStats = Instantiate(EnemyStats, pos);
                        cc.charStats = eStats.GetComponent<CharacterPortrait>();
                    }
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

        private void NextWave()
        {
            if (wavesTMP) wavesTMP.text = $"Wave {wave + 1}/{maxWave}";
            //destroy old enemys
            foreach (var cc in charControllerList)
                if (cc.isEnemy)
                {
                    Destroy(cc.charStats.gameObject);
                    Destroy(cc.gameObject);
                }
            charControllerList.RemoveAll(item => item.isEnemy == true);

            //destroy portraits queue
            foreach (Transform child in QueueUIContent.transform) //delete all portraits content
                Destroy(child.gameObject);
            QueueElements.Clear(); //clear list
            enemyAllyList.Clear(); //Clear Ally List before add new
            var waveList = battleScene.GetBattleData().enemyWaves[wave].enemyTeam;

            DropCharactersFromList(waveList, true); //create
            charControllerList.Sort(SortBySpeed);   //sort
            CreatePortraitQueue();                  //drop new portraits with sorting

            step = 0;
            maxStep = charControllerList.Count;
            QueueElements[step].transform.localScale *= portraitScale; //Scale Up First Element
            if (charControllerList[0].isEnemy)
                battleState = BattleState.ENEMY;
            else
                battleState = BattleState.PLAYER;
            ccOnSelect = charControllerList[step];
        }

        private void CreatePortraitQueue()
        {
            foreach (var cc in charControllerList) //Fill QueueUI characters icons
            {
                var portraitQ = Instantiate(portraitPrefab, QueueUIContent);
                portraitQ.name = "Portrait_" + cc.Name;
                portraitQ.GetComponent<Image>().sprite = cc.characterRes.icoPortrait;
                portraitQ.GetComponent<Button>().onClick.AddListener(cc.Select);
                if (cc.isEnemy)
                    portraitQ.transform.Find("color").GetComponent<Image>().color = redColor;  //Switch color on portrait indicator from blue to red
                QueueElements.Add(portraitQ);
            }
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

            if (sc.CheckMana(ccOnSelect.mana)) { 

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

                GameObject vfx = ccOnSelect.characterRes.skill[id].vfxOnTarget;
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
                            cc.Defence(ccOnSelect, id, vfx, aoe: true);
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
                    ccTarget.Defence(ccOnSelect, id, vfx, aoe: HEAL);
                }
                battleState = BattleState.ANIMATION;

                if (!battleStart) //skip button = true; back button = false
                {
                    if (battleScene != null)
                        battleScene.StartBattle();
                    battleStart = true;
                }
            }
        }
        public void Shake(float duration, float strenght) =>
            BattleCanvas.DOShakePosition(duration, strenght);

        IEnumerator EnemyAttack()
        {
            //Must be empty
            yield return new WaitForSeconds(0.5f); //Pause then show target stats
            int id = Random.Range(0, ccOnSelect.skill.Count);
            if (ccOnSelect.skill[id].AOE)
                ccTarget = null;
            else if (ccOnSelect.skill[id].actionType == "heal")
                ccTarget = enemyAllyList[Random.Range(0, enemyAllyList.Count)];
            else
                ccTarget = enemyTargetList[Random.Range(0, enemyTargetList.Count)];
            ccTarget?.Highlight();
            ccTarget?.CharPortraitSet();
            yield return new WaitForSeconds(0.5f);
            AttackAction(id, isEnemyAttack: true);
        }
        public void UseHPPotion()
        {
            if (potion_hp.potionAmount > 0 && overlord.health < overlord.healthMax)
                if (battleState == BattleState.PLAYER)
                {
                    overlord.Heal(Mathf.RoundToInt(overlord.healthMax * 0.2f));
                    potion_hp.InstVFX(overlord.persPos);
                    potion_hp.potionAmount--;
                    potion_hp.StatUpdate();
                    usedHP++;
                }
        }
        public void UseMPPotion()
        {
            if (potion_mp.potionAmount > 0 && overlord.mana < overlord.manaMax)
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
        public void WinOrLose(bool isWin)
        {
            foreach (var cc in charControllerList)
                if (cc.isEnemy != isWin)
                {
                    cc.PowerBuff();
                }
        }

        public void BattleOut()
        {
            UnselectButtons();
            ani.SetTrigger("BattleOut");
            if (battleState != BattleState.LOSE && battleState != BattleState.WIN && battleState != BattleState.NEXTWAVE)
            {
                Step();
                unselect?.Invoke();
                charControllerList[step].Highlight();
            }
            charOnSelect = -1;
            ccTarget = null;
        }

        public void StateUpdate(CharController invoker) //call when any character is dead
        {
            var index = charControllerList.FindIndex(x => x == invoker);
            QueueElements[index].SetActive(false); //disable queue portrait

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
                    if (battleScene != null)
                        battleScene.EndBattle(new BattleManagerOutData
                        {
                            battleWin = true,
                            manaSpent = usedMP,
                            hpSpent = usedHP
                        });
                    Debug.Log("WINNIG");
                }
                else
                {
                    battleState = BattleState.NEXTWAVE;
                    wave++;
                    NextWave();
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
                    //battleList[1].powerBuff = true; BUFF ON
                    Debug.Log("LOOSING");
            }
        }

        private void Step()
        {
            log.Add("Battle Manager: Step");
            var qe = QueueElements[step];
            qe.transform.SetSiblingIndex(maxStep);
            qe.transform.localScale = Vector3.one; //Reset Scale and push back portrait
            if (++step == maxStep)
            {
                if (round == 1)
                {
                    BattleNotif("chapter1", "battle1", "battletutor2"); //one round later
                    BattleNotif("chapter1", "battle4", "bosstutor");
                }
                roundEnd?.Invoke(); //снимает два статуса почему-то
                round++;
                roundTMP.text = $"Round {round}";
                log.Add($"Round {round}");
                step = 0;
            }
            if (castAOEForNotify == true)
            {
                BattleNotif("chapter1", "battle5", "potionstutor3"); //AOE Cast
            }
            qe.transform.SetSiblingIndex(maxStep); //Push element to back after Step++
            QueueElements[step].transform.localScale *= portraitScale; //Scale Up First Portrait

            var cc = charControllerList[step];
            if (cc.isDead || cc.stun) //skip dead/stun character
            {
                if (cc.stun)
                    cc.stun = false;
                Step();
            }
            else
            {
                ccOnSelect = charControllerList[step];
                bPosEnemy.SetSiblingIndex(siblingEnemy);
                bPosPlayer.SetSiblingIndex(siblingPlayer);
                if (ccOnSelect.isEnemy)
                {
                    battleState = BattleState.ENEMY;
                    StartCoroutine(EnemyAttack());
                    if (!ccOnSelect.isBoss)
                        bPosEnemy.SetSiblingIndex(siblingEnemy + 1);
                }
                else
                {
                    SetSkillCtrl(ccOnSelect);
                    ccOnSelect.CharPortraitSet();
                    battleState = BattleState.PLAYER;
                    bPosPlayer.SetSiblingIndex(siblingPlayer + 1);
                }
            }
        }

        private void SetSkillCtrl(CharController cc)
        {
            skillPanelWidthScale.sizeDelta = cc.isOverlord ? new Vector2(518, 144) : new Vector2(406, 144);
            var i = cc.skill?.Count;
            var j = 0;
            bool silent = cc.silence > 0;
            foreach (var sk in skillControllers)
            {
                sk.gameObject.SetActive(true);
                if (i > 0)
                {
                    sk.ReplaceSkill(cc.skill[j], cc.characterRes.skill[j]);
                    if (j != 0) sk.silence = silent; //silence realisation
                }
                else
                    sk.gameObject.SetActive(false);
                i--;
                j++;
            }

            i = cc.passiveSkill?.Count;
            j = 0;
            foreach (var sk in passiveControllers)
            {
                sk.gameObject.SetActive(true);
                if (i > 0)
                    sk.ReplaceSkill(cc.passiveSkill[j], cc.characterRes.pSkill[j]);
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
#if UNITY_EDITOR
                    Debug.Log($"{chapterID} {battleID} {notifID}");
#else
                    battleScene.OnBattleNotification(battleID, chapterID, notifID);
#endif
                }

        }
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
        }
        public void AddStatus(string effect)
        {
            if (ccTarget != null)
            {
                ccTarget.AddEffectManual(effect);
            }
            else
            {
                log.Add("Please select character");
            }
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
    }
}