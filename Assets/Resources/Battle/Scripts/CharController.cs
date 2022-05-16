using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Overlewd
{
    public class CharController : MonoBehaviour
    {
        public bool isEnemy = false;
        public bool isBoss = false;
        public bool isOverlord = false;
        //Char UI
        public Button bt;
        public Slider sliderHP;
        public Slider sliderMP;
        public TextMeshProUGUI hpTMP;
        public TextMeshProUGUI mpTMP;
        //Portrait UI
        public Button uiButton;
        public Slider uiHpSlider;
        public TextMeshPro uihpTMP;
        public CharacterPortrait charStats;

        public BattleManager bm; //init on BattleManager Initialize();
        private GameObject border;

        public Character character;
        public Skill[] skill;
        private float idleScale = .5f, battleScale = .7f;
        [SerializeField] private int battleOrder = -1; //3,2,1 = on the table; -1 = in the deck;
        public int hp = 100, maxHp = 100;
        public int mp = 100, maxMp = 100;
        public int attack = 10;
        public int defence = 5;

        [HideInInspector] public bool isDamageBuff = false;
        [HideInInspector] public int buffDamageScale = 2;

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

        public Action setAttackItem;

        private void Start()
        {
            StatInit();
            ShapeInit();
            UIInit();
            PlayIdle();
            UpdateUI();
        }

        private void StatInit()
        {
            character.ApplyStats();
            skill = character.skill;
            isOverlord = character.isOverlord;
            isEnemy = character.isEnemy;
            battleOrder = character.Order;
            idleScale = character.idleScale;
            battleScale = character.battleScale;
            hp = character.hp; maxHp = character.maxHp;
            mp = character.mp; maxMp = character.maxMp;
            attack = (int)Math.Ceiling(character.damage);
        }

        private void ShapeInit()
        {
            rt = gameObject.AddComponent<RectTransform>();
            UITools.SetStretch(rt);
            rt.localScale *= idleScale;

            Transform spawnPos = bm.transform;
            battleLayer = spawnPos.Find("BattleCanvas/BattleLayer").transform;
            battlePos = isEnemy ? battleLayer.Find("battlePos2").transform : battleLayer.Find("battlePos1").transform;
            if (isEnemy)
                persPos = battleLayer.Find("enemy" + battleOrder.ToString()).transform;
            else
                persPos = battleLayer.Find("pers" + battleOrder.ToString()).transform;

            transform.SetParent(persPos, false);
            transform.SetSiblingIndex(0);

            

            var sW = character.characterPrefab ? SpineWidget.GetInstance(character.characterPrefab, transform) : SpineWidget.GetInstance(character.idle_Prefab_Path, transform);
            sW.transform.localPosition = new Vector3(0, character.yOffset, 0);
            spineWidget = sW;
            defenceDuration = spineWidget.GetAnimationDuaration(character.ani_defence_name);
        }

        private void UIInit()
        {
            charStats.charCtrl = this;
            charStats.InitUI();
            sliderHP = persPos.Find("sliderHP").GetComponent<Slider>();
            if (sliderHP != null) sliderHP.maxValue = maxHp; else Debug.Log("sliderHP is null");
            hpTMP = persPos.Find("sliderHP/Text").GetComponent<TextMeshProUGUI>();

            if (isOverlord)
            {
                sliderMP = persPos.Find("sliderMP").GetComponent<Slider>();
                if (sliderMP != null) sliderMP.maxValue = maxMp;
                mpTMP = persPos.Find("sliderMP/Text").GetComponent<TextMeshProUGUI>();
            }

            bt = persPos.Find("button").GetComponent<Button>();
            border = persPos.Find("button/border").gameObject;
            border.SetActive(false);
            bt.onClick.AddListener(Select);
        }

        public void CharPortraitSet()
        {
            charStats.charCtrl = this;
            charStats.SetUI();
        }

        public void UpdateUI()
        {
            string hpTxt = $"{hp}/{maxHp}";
            if (hpTMP != null) hpTMP.text = hpTxt; else Debug.Log("hpTMP = null");
            if (sliderHP != null) sliderHP.value = hp;
            if (isOverlord)
            {
                string mpTxt = $"{mp}/{maxMp}";
                if (mpTMP != null) mpTMP.text = mpTxt;
                if (sliderMP != null) sliderMP.value = mp;
            }
            charStats.UpdateUI();
        }
        public void PlayIdle()
        {
            if (!isDead)
                spineWidget.PlayAnimation(character.ani_idle_name, true);
        }

        public void Attack(int attackID, CharController target = null)
        {
            BattleIn();
            if (target != null) target.BattleIn();
            StartCoroutine(PlayAttack(attackID, target));
        }

        IEnumerator PlayAttack(int id, CharController target = null)
        {
            if (id > character.skill.Length) id = 0; //if id overflow on skill array

            preAttackDuration = spineWidget.GetAnimationDuaration(character.skill[id].prepairAnimationName);  //send to target Defence Animation
            vfxDuration = character.skill[id].vfxDuration;
            attackDuration = spineWidget.GetAnimationDuaration(character.skill[id].attackAnimationName);

            attack = character.skill[id].power + character.power;
            spineWidget.PlayAnimation(character.skill[id].prepairAnimationName, false);
            if (isOverlord) mp -= character.skill[id].manaCost;
            yield return new WaitForSeconds(preAttackDuration);
            if (character.skill[id].vfx != null)
            {
                Instantiate(character.skill[id].vfx, battleLayer);
                yield return new WaitForSeconds(vfxDuration);
            }
            spineWidget.PlayAnimation(character.skill[id].attackAnimationName, false);
            yield return new WaitForSeconds(attackDuration);

            PlayIdle();
            BattleOut();
            bm.BattleOut();
        }
        public void Defence(CharController attacker, GameObject vfx = null) => StartCoroutine(PlayDefence(attacker, vfx));
        IEnumerator PlayDefence(CharController attacker, GameObject vfx = null)
        {
            transform.SetParent(battlePos);
            UnHiglight();
            yield return new WaitForSeconds(attacker.preAttackDuration + attacker.vfxDuration);
            if (vfx != null) Instantiate(vfx, transform);
            spineWidget.PlayAnimation(character.ani_defence_name, false);
            yield return new WaitForSeconds(defenceDuration);
            PlayIdle();
            Damage(attacker.attack);
            BattleOut();
        }

        public void Select()
        {
            if (bm.battleState == BattleManager.BattleState.PLAYER && !isDead)
            {
                bm.unselect?.Invoke();
                bm.ccOnClick = this; //type CharController
                Highlight();
            }
            if (!isEnemy)
                CharPortraitSet();
        }

        public void Highlight() => border.SetActive(true);
        public void UnHiglight() => border.SetActive(false);

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
        public void Damage(int value)
        {
            if (value > 0)
            {
                hp -= value;
                hp = Mathf.Max(hp, 0);
                if (hp <= 0)
                {
                    isDead = true;
                    bt.onClick.RemoveAllListeners();
                    StartCoroutine(PlayDead());
                    bm.StateUpdate(this);
                }
            }
            UpdateUI();
        }

        IEnumerator PlayDead()
        {
            yield return new WaitForSeconds(0.2f); //need for avoid idle animation state if isDead
            if (isOverlord)
            {
                spineWidget.PlayAnimation(character.ani_defeat_name, false);
                yield return new WaitForSeconds(spineWidget.GetAnimationDuaration(character.ani_defeat_name));
                spineWidget.PlayAnimation("defeat2", true); //!костыль
            }
            else
                spineWidget.PlayAnimation(character.ani_defeat_name, false);
        }

        public void Heal(int value)
        {
            //heal fx or animation
            hp += value;
            hp = Mathf.Min(hp, maxHp);
            UpdateUI();
        }
        public void HealMP(int value)
        {
            mp += value;
            mp = Mathf.Min(mp, maxMp);
            UpdateUI();
        }
    }
}