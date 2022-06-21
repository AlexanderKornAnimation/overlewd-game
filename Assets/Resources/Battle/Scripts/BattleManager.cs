using System.Collections;
using System.Collections.Generic;
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
        [HideInInspector] public List<GameObject> QueueElements;
        [SerializeField] private float portraitScale = 1.5f;
        [Tooltip("selected as current turn")] public CharController ccOnSelect;
        [Tooltip("selected as target")] public CharController ccOnClick = null;
        public Animator ani;

        public List<CharacterRes> characterResList; //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        //new init
        private AdminBRO.Battle battleData => battleScene.GetBattleData().battleData;
        private List<AdminBRO.Character> playerTeam => battleScene.GetBattleData().myTeam;
        private List<AdminBRO.Character> enemyTeam;

        public GameObject portraitPrefab; //attach to BM in inspector; "content" is ui spawn point
        public Transform QueueUIContent;
        public GameObject EnemyStats;
        public Transform EnemyStatsContent;
        [Tooltip("add this prefab here BattleUICanvas/Character/PlayerStats")]
        public CharacterPortrait PlayerStats;

        private SkillController[] skillControllers = new SkillController[3];
        private SkillController potion_mp, potion_hp;
        private int skillOnSelect = -1; //-1 unselect
        private int charOnSelect = -1; // 0 - allay 1 - enemy
        public GameObject healVFX;

        private int enemyNum = 0, enemyIsDead = 0, charNum = 0, charIsDead = 0;
        private int step = 0; //current characters list step
        private int maxStep = 2; //max count of battle queue, take from character's list
        private int wave = 0, maxWave = 2; //Wave count
        private int round = 1;
        public delegate void Unselect();
        public Unselect unselect;
        public bool castAOEForNotify = false;
        private Color redColor;

        //statistic, trackers, notif flags
        private bool battleStart = false;
        private bool hidePotion = false;
        private bool hideAOE = false;
        private bool notifIsShow = false;

        public int debug = 0;

        private void Start()
        {
            battleScene = FindObjectOfType<BaseBattleScreen>();

            enemyTeam = battleScene.GetBattleData().enemyWaves[wave].enemyTeam; //wave 0
            maxWave = battleScene.GetBattleData().enemyWaves.Count - 1;
            Debug.Log($"Battle ID: {battleData.id}");

            if (QueueUIContent == null) QueueUIContent = transform.Find("BattleUICanvas/QueueUI/Content");
            if (EnemyStatsContent == null) EnemyStatsContent = transform.Find("BattleUICanvas/Enemys/Content.enemy");
            if (PlayerStats == null) PlayerStats = transform.Find("BattleUICanvas/Character/PlayerStats").GetComponent<CharacterPortrait>();
            PlayerStats.bigPortrait = true;

            if (portraitPrefab == null) portraitPrefab = Resources.Load("Battle/Prefabs/Battle/Portrait") as GameObject;
            if (EnemyStats == null) EnemyStats = Resources.Load("Battle/Prefabs/Battle/EnemyStats") as GameObject;

            for (int i = 0; i < skillControllers.Length; i++)
            {
                var x = i; //Captured variable issue
                skillControllers[x] = transform.Find($"BattleUICanvas/Character/Buttons/Button_{x}").GetComponent<SkillController>();
                skillControllers[x].button.onClick.AddListener(delegate { ButtonPress(x); });
            }

            if (potion_mp == null) potion_mp = transform.Find("BattleUICanvas/Character/Buttons/Potion_mp").GetComponent<SkillController>();
            if (potion_hp == null) potion_hp = transform.Find("BattleUICanvas/Character/Buttons/Potion_hp").GetComponent<SkillController>();
            potion_mp?.button.onClick.AddListener(UseMPPotion);
            potion_hp?.button.onClick.AddListener(UseHPPotion);

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
                StartCoroutine(EnemyAttack());
            }
            else
                battleState = BattleState.PLAYER;
            ccOnSelect = charControllerList[step];
            StartCoroutine(LateInit());
        }
        IEnumerator LateInit()
        {
            yield return new WaitForSeconds(0.01f);
            charControllerList[step].Highlight();
            SetSkillCtrl(ccOnSelect);
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
                cc.characterRes = cRes == null ? characterResList[2] : cRes;

                charControllerList.Add(cc);
                cc.bm = this;
                unselect += cc.UnHiglight;
                cc.battleOrder = orderCount;

                if (isEnemy)
                {
                    cc.isEnemy = true; //Add Enemy flag
                    var pos = EnemyStatsContent.Find($"portrait_pos_{orderCount}"); //Drop portrait on Content Queue and turn on
                    pos.gameObject.SetActive(true);
                    var eStats = Instantiate(EnemyStats, pos);
                    cc.charStats = eStats.GetComponent<CharacterPortrait>();
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
                portraitQ.GetComponent<Image>().sprite = cc.characterRes.ico;
                portraitQ.GetComponent<Button>().onClick.AddListener(cc.Select);
                if (cc.isEnemy)
                    portraitQ.transform.Find("color").GetComponent<Image>().color = redColor;  //Switch color on portrait indicator from blue to red
                QueueElements.Add(portraitQ);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z)) ButtonPress(0);
            if (Input.GetKeyDown(KeyCode.X)) ButtonPress(1);
            if (Input.GetKeyDown(KeyCode.C) && ccOnClick.isOverlord) ButtonPress(2);
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
                if (charOnSelect != -1 && ccOnClick == ccOnPress)
                {
                    unselect?.Invoke();
                    charOnSelect = -1;
                    ccOnClick = null;
                    foreach (var sc in skillControllers)
                        sc.Enable();
                    return false;
                }
                else
                {
                    ccOnClick = ccOnPress;
                    charOnSelect = ccOnPress.isEnemy ? 1 : 0;
                    foreach (var sc in skillControllers)
                    {
                        sc.Enable();
                        if (ccOnClick.isEnemy == sc.isHeal || !ccOnClick.isEnemy == !sc.isHeal)
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
                    ccOnClick = ccOnPress;
                    charOnSelect = ccOnPress.isEnemy ? 1 : 0;
                    AttackCheck();
                    return true;
                }
            }
        }

        private void ButtonPress(int id)
        {
            if (skillControllers[id].selectable)
            {
                if (skillOnSelect > -1 && skillOnSelect != id)
                    skillControllers[skillOnSelect].Unselect();
                skillOnSelect = skillControllers[id].Press() ? id : -1;
                AttackCheck();
            }
        }

        private void AttackCheck()
        {
            if (skillOnSelect != -1 && charOnSelect != -1)
            {
                AttackAction(skillOnSelect);
            }
        }

        private void AttackAction(int id)
        {
            skillControllers[skillOnSelect].Unselect();
            skillOnSelect = -1;
            charOnSelect = -1;
            unselect?.Invoke();
            bool AOE = ccOnSelect.skill[id].AOE;

            if (battleState == BattleState.PLAYER)
            {
                ani.SetTrigger("Player");
                GameObject vfx = ccOnSelect.characterRes.skill[id].vfxOnTarget;
                if (AOE)
                {
                    ccOnClick = null;
                    ccOnSelect.Attack(id, ccOnClick);
                    foreach (var cc in charControllerList)
                        if (cc.isEnemy && !cc.isDead)
                        {
                            cc.Defence(ccOnSelect, id, vfx);
                            cc.UnHiglight();
                        }
                    if (ccOnSelect.skill[id].name == "Firestorm" || ccOnSelect.skill[id].name == "Fireballs")
                    {
                        castAOEForNotify = true; //for battle notify
                    }
                }
                else
                {
                    ccOnSelect.Attack(id, ccOnClick);
                    ccOnClick.Defence(ccOnSelect, id, vfx);
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

        IEnumerator EnemyAttack()
        {
            yield return new WaitForSeconds(0.5f);
            ccOnClick = enemyTargetList[Random.Range(0, enemyTargetList.Count)];
            ccOnClick.Highlight();
            ccOnClick.CharPortraitSet(); //Show target stats
            yield return new WaitForSeconds(0.5f);
            ani.SetTrigger("Enemy");
            int id = Random.Range(0, 1);
            bool AOE = ccOnSelect.skill[id].AOE;
            GameObject vfx = ccOnSelect.characterRes.skill[id].vfxOnTarget;

            if (AOE)
            {
                ccOnClick = null;
                ccOnSelect.Attack(id, ccOnClick);
                foreach (var cc in enemyTargetList)
                {
                    cc.Defence(ccOnSelect, id, vfx);
                    cc.UnHiglight();
                }
            }
            else
            {
                ccOnSelect.Attack(id, ccOnClick);
                ccOnSelect.UnHiglight();
                ccOnClick.Defence(ccOnSelect, id, vfx);
            }

            battleState = BattleState.ANIMATION;
        }
        public void UseMPPotion()
        {
            if (battleState == BattleState.PLAYER)
            {
                overlord.HealMP(25);
                potion_mp.InstVFX(overlord.persPos);
                potion_mp.amount--;
                potion_mp.StatUpdate();
            }
        }
        public void UseHPPotion()
        {
            if (battleState == BattleState.PLAYER && ccOnClick)
            {
                ccOnClick.Heal(25);
                potion_hp.InstVFX(ccOnClick.persPos);
                potion_hp.amount--;
                potion_hp.StatUpdate();
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
            ccOnClick = null;
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
                            manaSpent = 0,
                            hpSpent = 0
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
                        manaSpent = 0,
                        hpSpent = 0
                    });
                if (CheckBattleGameData("chapter1", "battle2"))
                    //battleList[1].powerBuff = true; BUFF ON
                    Debug.Log("LOOSING");
            }
        }

        private void Step()
        {
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
                round++;
                step = 0;
            }
            if (castAOEForNotify == true)
            {
                BattleNotif("chapter1", "battle5", "potionstutor3"); //AOE Cast
            }
            qe.transform.SetSiblingIndex(maxStep); //Push element to back after Step++
            QueueElements[step].transform.localScale *= portraitScale; //Scale Up First Portrait

            if (charControllerList[step].isDead) //skip dead character
                Step();
            else
            {
                ccOnSelect = charControllerList[step];

                if (ccOnSelect.isEnemy)
                {
                    battleState = BattleState.ENEMY;
                    StartCoroutine(EnemyAttack());
                }
                else
                {
                    SetSkillCtrl(ccOnSelect);
                    battleState = BattleState.PLAYER;
                    skillControllers[2].gameObject.SetActive(ccOnSelect.isOverlord); //Turn Off Attack 3 button on screen
                    ccOnSelect.CharPortraitSet();
                }
            }
        }

        private void SetSkillCtrl(CharController cc)
        {
            var i = cc.skill?.Count-1;
            var j = 0;
            foreach (var sk in skillControllers)
            {
                if (i > 0)
                    sk.ReplaceSkill(cc.skill[j]);
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
                    battleScene.OnBattleNotification(battleID, chapterID, notifID);
                    //Debug.Log($"{chapterID} {battleID} {notifID}");
                }
        }
        public bool CheckBattleGameData(string chapterID, string battleID)
        {
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

        private void OnGUI()
        {
            if (GUI.Button(new Rect(Screen.width / 2, Screen.height - 64, 72, 32), "Debug"))
                if (debug < 2) debug++; else debug = 0;
            if (debug > 0)
            {
                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.white;
                style.fontSize = 22;

                GUI.Label(new Rect(Screen.width / 2 - 80, 64, 300, 500), $"Battle ID: {battleData.id}\n" +
                    $"is BossLevel {battleData.isTypeBoss}", style);
            }
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