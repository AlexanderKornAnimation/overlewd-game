using DG.Tweening;
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
        [HideInInspector] public List<Character> characters;
        [HideInInspector] public List<CharController> charControllerList;
        [HideInInspector] public List<CharController> enemyTargetList;
        [HideInInspector] public List<GameObject> QueueElements;
        [SerializeField] private float portraitScale = 1.5f;
        [Tooltip("selected as current turn")] public CharController ccOnSelect;
        [Tooltip("selected as target")] public CharController ccOnClick = null;
        public Animator ani;

        public GameObject portraitPrefab; //attach to BM in inspector; "content" is ui spawn point
        public Transform QueueUIContent;
        public GameObject EnemyStats;
        public Transform EnemyStatsContent;
        public GameObject PlayerStats;
        public Transform PlayerStatsContent;

        private SkillController[] skillControllers = new SkillController[3];
        private SkillController potion_mp, potion_hp;
        public GameObject healVFX;

        private int enemyNum = 0, enemyIsDead = 0, charNum = 0, charIsDead = 0;
        private bool battleStart = false;
        public bool wannaWin = true;
        private int step = 0; //current characters list step
        private int maxStep = 2; //max count of battle queue, take from character's list
        private int wave = 1, maxWave = 2; //Wave count
        public delegate void Unselect();
        public Unselect unselect;
        public bool castAOE = false;
        [SerializeField]
        private Color redColor;


        private void Start()
        {
            battleScene = FindObjectOfType<BaseBattleScreen>();

            if (QueueUIContent == null) QueueUIContent = transform.Find("BattleUICanvas/QueueUI/Content");
            if (PlayerStatsContent == null) PlayerStatsContent = transform.Find("BattleUICanvas/Character");
            if (EnemyStatsContent == null) EnemyStatsContent = transform.Find("BattleUICanvas/Enemys/Content.enemy");
            if (portraitPrefab == null) portraitPrefab = Resources.Load("Battle/Prefabs/Battle/Portrait") as GameObject;
            if (EnemyStats == null) EnemyStats = Resources.Load("Battle/Prefabs/Battle/EnemyStats") as GameObject;
            if (PlayerStats == null) PlayerStats = Resources.Load("Battle/Prefabs/Battle/PlayerStats") as GameObject;

            for (int i = 0; i < skillControllers.Length; i++)
            {
                var x = i; //Captured variable issue
                skillControllers[x] = transform.Find($"BattleUICanvas/Character/Buttons/Button_{x}").GetComponent<SkillController>();
                skillControllers[x].button.onClick.AddListener(delegate { ButtonClick(x); });
            }

            if (potion_mp == null) potion_mp = transform.Find("BattleUICanvas/Character/Buttons/Potion_mp").GetComponent<SkillController>();
            if (potion_hp == null) potion_hp = transform.Find("BattleUICanvas/Character/Buttons/Potion_hp").GetComponent<SkillController>();
            potion_mp?.button.onClick.AddListener(UseMPPotion);
            potion_hp?.button.onClick.AddListener(UseHPPotion);

            characters = new List<Character>(Resources.LoadAll<Character>("Battle/BattlePersonages/Profiles"));
            characters.Sort(SortByInitiative);
           // ColorUtility.TryParseHtmlString("#A64646", out redColor);

            var pStats = Instantiate(PlayerStats, PlayerStatsContent);
            pStats.GetComponent<CharacterPortrait>().isPlayer = true;
            foreach (var c in characters)
            {
                if (c.Order > 0 && c.wave == 1)
                {
                    var charGO = new GameObject(c.name);
                    var cc = charGO.AddComponent<CharController>();
                    cc.character = c;
                    charControllerList.Add(cc);
                    cc.bm = this; //Link battle manager
                    unselect += cc.UnHiglight; //Add Listener to delegate
                    if (cc.isOverlord) overlord = cc;
                    if (c.isEnemy)
                    {
                        var pos = EnemyStatsContent.Find($"portrait_pos_{c.Order}"); //Drop portrait on Content Queue and turn on
                        pos.gameObject.SetActive(true);
                        var eStats = Instantiate(EnemyStats, pos);
                        cc.charStats = eStats.GetComponent<CharacterPortrait>();
                        enemyNum++;
                    }
                    else
                    {
                        enemyTargetList.Add(cc);
                        cc.charStats = pStats.GetComponent<CharacterPortrait>();
                        charNum++;
                    }
                    /*Fill QueueUI characters icons
                    var portraitQ = Instantiate(portraitPrefab, QueueUIContent);
                    portraitQ.name = "Portrait_" + c.name;
                    portraitQ.GetComponent<Image>().sprite = c.ico;
                    portraitQ.GetComponent<Button>().onClick.AddListener(cc.Select);
                    if (c.isEnemy)
                        portraitQ.transform.Find("color").GetComponent<Image>().color = redColor;  //Switch color on portrait indicator from blue to red
                    QueueElements.Add(portraitQ);*/
                }
            }
            CreatePortraitQueue();
            maxStep = charControllerList.Count;
            QueueElements[step].transform.localScale *= portraitScale; //Scale Up First Element
            if (charControllerList[0].isEnemy)
                battleState = BattleState.ENEMY;
            else
                battleState = BattleState.PLAYER;
            ccOnSelect = charControllerList[step];
            StartCoroutine(LateInit());
            BattleNotif("chapter1", "battle1", "battletutor1");
            BattleNotif("chapter1", "battle3", "battletutor4");
            BattleNotif("chapter1", "battle5", "potionstutor1");
            BattleNotif("chapter1", "battle5", "potionstutor2");
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
            int childs = QueueUIContent.childCount;
            foreach(Transform child in QueueUIContent.transform)
                Destroy(child.gameObject);
            QueueElements.Clear();
            //create new enemys
            foreach (var c in characters)
            {
                if (c.Order > 0 && c.wave == wave)
                {
                    var charGO = new GameObject(c.name);
                    var cc = charGO.AddComponent<CharController>();
                    cc.character = c;
                    charControllerList.Add(cc);
                    cc.bm = this; //Link battle manager
                    unselect += cc.UnHiglight; //Add Listener to delegate
                    if (c.isEnemy)
                    {
                        var pos = EnemyStatsContent.Find($"portrait_pos_{c.Order}"); //Drop portrait on Content Queue and turn on
                        pos.gameObject.SetActive(true);
                        var eStats = Instantiate(EnemyStats, pos);
                        cc.charStats = eStats.GetComponent<CharacterPortrait>();
                        enemyNum++;
                    }
                }
            }
            CreatePortraitQueue();
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
                portraitQ.name = "Portrait_" + cc.character.name;
                portraitQ.GetComponent<Image>().sprite = cc.character.ico;
                portraitQ.GetComponent<Button>().onClick.AddListener(cc.Select);
                if (cc.character.isEnemy)
                    portraitQ.transform.Find("color").GetComponent<Image>().color = redColor;  //Switch color on portrait indicator from blue to red
                QueueElements.Add(portraitQ);
            }
        }

        IEnumerator LateInit()
        {
            yield return new WaitForSeconds(0.01f);
            SetSkillCtrl(ccOnSelect);
            charControllerList[step].Select();
            WinOrLose(wannaWin);
        }

        private void ButtonClick(int id)
        {
            unselect?.Invoke();
            skillControllers[id].Select();
            bool AOE = ccOnSelect.skill[id].attackType == Skill.AttackType.AOE;

            if (battleState == BattleState.PLAYER)
            {
                ani.SetTrigger("Player");
                GameObject vfx = ccOnSelect.skill[id].vfxOnTarget;
                if (AOE)
                {
                    ccOnClick = null;
                    ccOnSelect.Attack(id, ccOnClick);
                    foreach (var cc in charControllerList)
                        if (cc.isEnemy && !cc.isDead)
                        {
                            cc.Defence(ccOnSelect, vfx);
                            cc.UnHiglight();
                        }
                    if (ccOnSelect.isOverlord)
                    {
                        castAOE = true; //for battle notify
                    }
                }
                else
                {
                    ccOnSelect.Attack(id, ccOnClick);
                    ccOnClick.Defence(ccOnSelect, vfx);
                }
                battleState = BattleState.ANIMATION;
                BattleStart();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z)) ButtonClick(0);
            if (Input.GetKeyDown(KeyCode.X)) ButtonClick(1);
            if (Input.GetKeyDown(KeyCode.C) && ccOnClick.isOverlord) ButtonClick(2);
            if (Input.GetKeyDown(KeyCode.R))
                foreach (var cc in charControllerList)
                    if (cc.isEnemy == true && !cc.isDead)
                    {
                        cc.hp = cc.maxHp;
                        cc.UpdateUI();
                    }
        }

        IEnumerator EnemyAttack1()
        {
            yield return new WaitForSeconds(0.5f);
            var ct = enemyTargetList.Count;
            ccOnClick = enemyTargetList[Random.Range(0, ct)];
            yield return new WaitForSeconds(0.5f);

            ccOnClick.CharPortraitSet(); //Show target stats
            ani.SetTrigger("Enemy");
            int attackSkill = Random.Range(0, 2);
            ccOnSelect.Attack(attackSkill, ccOnClick);
            GameObject vfx = ccOnSelect.skill[attackSkill].vfxOnTarget;
            ccOnClick.Defence(ccOnSelect, vfx);
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
            for (int i = 0; i < skillControllers.Length; i++)
                skillControllers[i].Unselect();
        }
        public void WinOrLose(bool isWin)
        {
            foreach (var cc in charControllerList)
                if (cc.isEnemy == isWin)
                {
                    cc.hp = 10;
                    cc.UpdateUI();
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
                        battleScene.BattleWin();
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
                    battleScene.BattleDefeat();
                BattleNotif("chapter1", "battle2", "bufftutor1");
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
                BattleNotif("chapter1", "battle1", "battletutor2");
                BattleNotif("chapter1", "battle4", "bosstutor");
                step = 0;
            }
            if (castAOE == true)
            {
                BattleNotif("chapter1", "battle5", "potionstutor3");
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
                    StartCoroutine(EnemyAttack1());
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

        private void SetSkillCtrl(CharController character)
        {
            for (int i = 0; i < ccOnSelect.skill.Length; i++)
            {
                var x = i;
                skillControllers[x].ReplaceSkill(character.skill[x]); //add selected character skill on button controller
            }
        }

        private void BattleStart()
        {
            if (!battleStart)
            {
                if (battleScene != null)
                    battleScene.StartBattle();
                battleStart = true;
            }
        }

        public void BattleNotif(string chapterID, string battleID, string notifID = null)
        {
            if (battleScene.GetBattleData() != null)
                if (chapterID == battleScene.GetBattleData().ftueChapterKey && battleID == battleScene.GetBattleData().ftueStageKey)
                    battleScene.OnBattleNotification(notifID);
        }

        private void OnDestroy() { DOTween.Clear(true); } //Destroy DOTween

        private int SortByInitiative(Character a, Character b)
        {
            if (a.speed < b.speed)
                return 1;
            else if (a.speed > b.speed)
                return -1;
            else if (a.isEnemy)
                return -1;
            else if (b.isEnemy)
                return 1;
            return 0;
        }

        public enum BattleState { PLAYER, ENEMY, ANIMATION, INIT, WIN, LOSE, NEXTWAVE }
        public BattleState battleState = BattleState.INIT;
    }
}
