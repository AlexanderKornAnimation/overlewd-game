using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class BattleManager : MonoBehaviour
    {
        private TempBattleScreen tempBattleScene; //In Out of Stage

        [HideInInspector] public List<Character> characters;
        [HideInInspector] public List<CharController> charControllerList;
        [HideInInspector] public List<CharController> targetsForEnemy;
        [HideInInspector] public List<GameObject> QueueElements;
        [SerializeField] private float portraitScale = 1.5f;
        public CharController ccOnSelect;
        public CharController ccOnClick;
        public Animator ani;

        public GameObject portraitPrefab; //attach to BM in inspector; "content" is ui spawn point
        public Transform QueueUIContent;
        public GameObject EnemyStats;
        public Transform EnemyStatsContent;
        public GameObject PlayerStats;
        public Transform PlayerStatsContent;

        public Button attack_1, attack_2, attack_3;

        private bool battleStart = false;
        public bool wannaWin = true;
        private int step = 0; //current characters list step
        private int maxStep = 2; //max count of battle queue, take from character's list
        private int wave = 1, maxWave = 3;

        private void Start() => Initialize();

        private void Initialize()
        {
            tempBattleScene = FindObjectOfType<TempBattleScreen>();
            if (attack_1 != null)
                attack_1.onClick.AddListener(Button1);
            else
                Debug.LogError($"Lost Button 1 prefab on {this.name}");
            if (attack_2 != null)
                attack_2.onClick.AddListener(Button2);
            else
                Debug.LogError($"Lost Button 2 prefab on {this.name}");
            if (attack_3 != null) attack_3.onClick.AddListener(Button2);
            else
                Debug.LogError($"Lost Button 3 prefab on {this.name}");

            if (QueueUIContent == null)
                QueueUIContent = transform.Find("BattleUICanvas/QueueUI/Content");
            if (portraitPrefab == null)
                portraitPrefab = Resources.Load("Battle/Prefabs/Battle/Portrait") as GameObject;
            if (EnemyStatsContent == null)
                EnemyStatsContent = transform.Find("BattleUICanvas/Enemys/Content.enemy");
            if (EnemyStats == null)
                EnemyStats = Resources.Load("Battle/Prefabs/Battle/EnemyStats") as GameObject;
            if (PlayerStatsContent == null)
                PlayerStatsContent = transform.Find("BattleUICanvas/Character");
            if (PlayerStats == null)
                PlayerStats = Resources.Load("Battle/Prefabs/Battle/PlayerStats") as GameObject;

            characters = new List<Character>(Resources.LoadAll<Character>("Battle/BattlePersonages/Profiles"));
            characters.Sort(SortByInitiative);
            Color redColor;
            ColorUtility.TryParseHtmlString("#A64646", out redColor);

            var pStats = Instantiate(PlayerStats, PlayerStatsContent);
            pStats.GetComponent<CharacterPortrait>().isPlayer = true;
            foreach (var c in characters)
            {
                if (c.Order > 0)
                {
                    var charGO = new GameObject(c.name);
                    var cc = charGO.AddComponent<CharController>();
                    cc.character = c;
                    charControllerList.Add(cc);
                    cc.bm = this;
                    if (!c.isEnemy)
                    {
                        targetsForEnemy.Add(cc);
                        cc.charStats = pStats.GetComponent<CharacterPortrait>();
                    }
                    else
                    {
                        var eStats = Instantiate(EnemyStats, EnemyStatsContent);
                        cc.charStats = eStats.GetComponent<CharacterPortrait>();
                    }
                    //Fill QueueUI characters icons
                    var portraitQ = Instantiate(portraitPrefab, QueueUIContent);
                    portraitQ.name = "Portrait_" + c.name;
                    portraitQ.GetComponent<Image>().sprite = c.ico;
                    portraitQ.GetComponent<Button>().onClick.AddListener(cc.Select);
                    if (c.isEnemy)
                        portraitQ.transform.Find("color").GetComponent<Image>().color = redColor;  //Switch color on portrait indicator from blue to red
                    QueueElements.Add(portraitQ);
                }
            }

            maxStep = charControllerList.Count - 1;
            QueueElements[step].transform.localScale *= portraitScale; //Scale Up First Element
            if (charControllerList[0].isEnemy)
                battleState = BattleState.ENEMY;
            else
                battleState = BattleState.PLAYER;
            ccOnSelect = charControllerList[step];
            StartCoroutine(LateInit());
        }

        IEnumerator LateInit()
        {
            yield return new WaitForSeconds(0.01f);
            charControllerList[step].Select();
            WinOrLose(wannaWin);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
                Button1();
            if (Input.GetKeyDown(KeyCode.X))
                Button2();
            if (Input.GetKeyDown(KeyCode.C))
                Button3();
            if (Input.GetKeyDown(KeyCode.W))
                WinOrLose(true);
            if (Input.GetKeyDown(KeyCode.L))
                WinOrLose(false);

        }

        public void Button1()
        {
            if (ccOnClick != null && battleState == BattleState.PLAYER) //|| onClick.isEnemy == false
            {
                ani.SetTrigger("Player");
                ccOnSelect.Attack(0, ccOnClick);
                ccOnClick.Defence(ccOnSelect);
                battleState = BattleState.ANIMATION;
                BattleStart();
            }
        }
        public void Button2()
        {
            if (ccOnClick != null && battleState == BattleState.PLAYER)
            {
                ani.SetTrigger("Player");
                ccOnSelect.Attack(1, ccOnClick);
                ccOnClick.Defence(ccOnSelect);
                battleState = BattleState.ANIMATION;
                BattleStart();
            }
        }
        public void Button3()
        {
            if (ccOnClick != null && battleState == BattleState.PLAYER && ccOnClick.isOverlord)
            {
                ani.SetTrigger("Player");
                ccOnSelect.Attack(3, ccOnClick);
                ccOnClick.Defence(ccOnSelect);
                battleState = BattleState.ANIMATION;
                BattleStart();
            }
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

        IEnumerator EnemyAttack1()
        {
            yield return new WaitForSeconds(0.5f);
            var ct = targetsForEnemy.Count;
            ccOnClick = targetsForEnemy[Random.Range(0, ct)];
            while (ccOnClick.isDead)
                ccOnClick = targetsForEnemy[Random.Range(0, ct)];
            yield return new WaitForSeconds(0.5f);

            ccOnClick.CharPortraitSet(); //Show target stats
            ani.SetTrigger("Enemy");
            ccOnSelect.Attack(Random.Range(0, 2), ccOnClick);
            ccOnClick.Defence(ccOnSelect);
            battleState = BattleState.ANIMATION;
        }

        public void BattleOut()
        {
            ani.SetTrigger("BattleOut");
            if (battleState != BattleState.LOSE && battleState != BattleState.WIN)
                Step();
            charControllerList[step].Highlight();
            ccOnClick = null;
        }

        public void StateUpdate() //call when any character is dead
        {
            int enemy = 0, enemyIsDead = 0, character = 0, charIsDead = 0;
            foreach (var item in charControllerList)
            {
                if (item.isEnemy)
                {
                    enemy++;
                    if (item.isDead)
                        enemyIsDead++;
                }
                else
                {
                    character++;
                    if (item.isDead)
                        charIsDead++;
                }
            }
            if (enemy == enemyIsDead) { 
                battleState = BattleState.WIN;
                if (tempBattleScene != null)
                    tempBattleScene.BattleWin();
                Debug.Log("WINNIG");
            }
            if (character == charIsDead) { 
                battleState = BattleState.LOSE;
                if (tempBattleScene!=null)
                    tempBattleScene.BattleDefeat();
                Debug.Log("LOOSING");
            }
        }

        private void Step()
        {
            var qe = QueueElements[step];
            qe.transform.SetSiblingIndex(maxStep);
            qe.transform.localScale = Vector3.one; //Reset Scale and push back portrait
            if (step < maxStep) step++; else step = 0;
            qe.transform.SetSiblingIndex(maxStep); //Push element to back after Step++
            QueueElements[step].transform.localScale *= portraitScale; //Scale Up First Portrait
            if (charControllerList[step].isDead) //check & skip dead character
            {
                if (qe.activeSelf) qe.SetActive(false); //turn off portrait from queue
                Step();
            }

            if (charControllerList[step].isEnemy)
            {
                battleState = BattleState.ENEMY;
                StartCoroutine(EnemyAttack1());
            }
            else
            {
                battleState = BattleState.PLAYER;
                attack_3.gameObject.SetActive(charControllerList[step].isOverlord); //Turn Off Attack 3 button on screen
                charControllerList[step].CharPortraitSet();
            }
            ccOnSelect = charControllerList[step];
        }

        private void BattleStart()
        {
            if (!battleStart)
            {
                tempBattleScene.StartBattle();
                battleStart = true;
            }
        }

        private int SortByInitiative(Character a, Character b)
        {
            if (a.initiative < b.initiative)
                return 1;
            else if (a.initiative > b.initiative)
                return -1;
            else if (a.isEnemy)
                return -1;
            else if (b.isEnemy)
                return 1;
            return 0;
        }

        public enum BattleState { PLAYER, ENEMY, ANIMATION, INIT, WIN, LOSE }
        public BattleState battleState = BattleState.INIT;
    }
}
