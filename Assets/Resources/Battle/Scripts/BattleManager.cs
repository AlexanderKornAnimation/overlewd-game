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

        [HideInInspector] public List<Character> characters;
        [HideInInspector] public List<CharController> charControllerList;
        [HideInInspector] public List<CharController> targetsForEnemy;
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

        public ButtonSkillController[] skillControllers;
        public ButtonSkillController potion_0, potion_1;
        public GameObject healVFX;

        private int enemyNum = 0, enemyIsDead = 0, charNum = 0, charIsDead = 0;
        private bool battleStart = false;
        public bool wannaWin = true;
        private int step = 0; //current characters list step
        private int maxStep = 2; //max count of battle queue, take from character's list
        //private int wave = 1, maxWave = 3;
        public delegate void Unselect();
        public Unselect unselect;

        private void Start()
        {
            battleScene = FindObjectOfType<BaseBattleScreen>();
            for (int i = 0; i < 3; i++)
                skillControllers[i]?.button?.onClick.AddListener(delegate { ButtonClick(i); });

            potion_0?.button.onClick.AddListener(UseMPPotion);
            potion_1?.button.onClick.AddListener(UseHPPotion);

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
                    unselect += cc.UnHiglight; //подписываем на делегата
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
                        targetsForEnemy.Add(cc);
                        cc.charStats = pStats.GetComponent<CharacterPortrait>();
                        charNum++;
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

            maxStep = charControllerList.Count;
            QueueElements[step].transform.localScale *= portraitScale; //Scale Up First Element
            if (charControllerList[0].isEnemy)
                battleState = BattleState.ENEMY;
            else
            {
                battleState = BattleState.PLAYER;
            }
            ccOnSelect = charControllerList[step];
            StartCoroutine(LateInit());
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
            bool AOE = ccOnClick.skill[id].attackType == Skill.AttackType.AOE;

            if (battleState == BattleState.PLAYER)
            {
                ani.SetTrigger("Player");
                GameObject vfx = ccOnClick.skill[id].vfxOnTarget;
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
            if (Input.GetKeyDown(KeyCode.C)) ButtonClick(2);
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
            var ct = targetsForEnemy.Count;
            ccOnClick = targetsForEnemy[Random.Range(0, ct)];
            while (ccOnClick.isDead) //Death Loop Bug?
                ccOnClick = targetsForEnemy[Random.Range(0, ct)];
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
            if (ccOnClick.isOverlord)
            {
                ccOnClick.HealMP(25);
                potion_0.InstVFX(ccOnClick.persPos);
                potion_0.amount--;
                potion_0.StatUpdate();
            }
            else if (ccOnSelect.isOverlord)
            {
                ccOnSelect.HealMP(25);
                potion_0.InstVFX(ccOnSelect.persPos);
                potion_0.amount--;
                potion_0.StatUpdate();
            }
        }
        public void UseHPPotion()
        {
            ccOnClick.Heal(25);
            potion_1.InstVFX(ccOnClick.persPos);
            potion_1.amount--;
            potion_1.StatUpdate();
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
            if (battleState != BattleState.LOSE && battleState != BattleState.WIN)
                Step();
            unselect?.Invoke();
            charControllerList[step].Highlight();
            ccOnClick = null;
        }

        public void StateUpdate(CharController invoker) //call when any character is dead
        {
            var index = charControllerList.FindIndex(x => x == invoker);
            QueueElements[index].SetActive(false); //disable queue portrait

            if (invoker.isEnemy) enemyIsDead++; else charIsDead++;
            if (enemyNum == enemyIsDead)
            {
                //next wave function or
                battleState = BattleState.WIN;
                if (battleScene != null)
                    battleScene.BattleWin();
                Debug.Log("WINNIG");
            }
            if (charNum == charIsDead)
            {
                battleState = BattleState.LOSE;
                if (battleScene != null)
                    battleScene.BattleDefeat();
                Debug.Log("LOOSING");
            }
        }

        private void Step()
        {
            var qe = QueueElements[step];
            qe.transform.SetSiblingIndex(maxStep);
            qe.transform.localScale = Vector3.one; //Reset Scale and push back portrait
            if (++step >= maxStep) step = 0;
            qe.transform.SetSiblingIndex(maxStep); //Push element to back after Step++
            QueueElements[step].transform.localScale *= portraitScale; //Scale Up First Portrait

            if (charControllerList[step].isDead) //check & skip dead character
                Step();

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

        private void SetSkillCtrl(CharController character)
        {
            for (int i = 0; i < ccOnSelect.skill.Length; i++)
                skillControllers[i].ReplaceSkill(character.skill[i]); //add selected character skill on button controller
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

        public enum BattleState { PLAYER, ENEMY, ANIMATION, INIT, WIN, LOSE }
        public BattleState battleState = BattleState.INIT;
    }
}
