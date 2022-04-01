using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
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
        private float idleScale = .5f, battleScale = .7f;
        [SerializeField] private int battleOrder = -1; //3,2,1 = on the table; -1 = in the deck;
        public int hp = 100, maxHp = 100;
        public int mp = 100, maxMp = 100;
        public int damage_1 = 10;
        public int damage_2 = 10;
        public int defence = 5;

        [HideInInspector] public bool isDamageBuff = false;
        [HideInInspector] public int buffDamageScale = 2;

        public bool isDead = false;

        private Transform battleLayer;
        private Transform persPos;
        private Transform battlePos;
        private List<SpineWidget> spineWidgets;
        private SpineWidget spineWidgetOne;
        public float[] aniDuration = { 2f, 0.9f, 0.9f, 1f, 1f, .9f, 2.333f };
        public float currentPreAttackDuration = 0.9f;
        private int aniID = 0;
        private RectTransform rt;

        public Action setAttackItem;

        private void Start()
        {
            spineWidgets = new List<SpineWidget>();
            StatInit();
            ShapeInit();
            UIInit();
        }

        private void StatInit()
        {
            isOverlord = character.isOverlord;
            isEnemy = character.isEnemy;
            battleOrder = character.Order;
            idleScale = character.idleScale;
            battleScale = character.battleScale;
            hp = character.hp; maxHp = character.maxHp;
            mp = character.mp; maxMp = character.maxMp;
            damage_1 = character.attack;
            damage_2 = character.attack * 2;
        }

        private void ShapeInit()
        {
            rt = gameObject.AddComponent<RectTransform>();
            UITools.SetStretch(GetComponent<RectTransform>());
            rt.localScale *= idleScale;

            battlePos = isEnemy ? GameObject.Find("battlePos2").transform : GameObject.Find("battlePos1").transform;
            battleLayer = GameObject.Find("BattleLayer").transform;

            if (isEnemy)
                persPos = battleLayer.Find("enemy" + battleOrder.ToString()).transform;
            else
                persPos = battleLayer.Find("pers" + battleOrder.ToString()).transform;

            transform.SetParent(persPos, false);
            transform.SetSiblingIndex(0);

            var folder = character.folder;  //0-6
            if (isOverlord)
            {
                var sW = SpineWidget.GetInstance(transform);
                sW.Initialize(folder + character.ani_idle_path);
                sW.transform.localPosition = new Vector3(0, character.yOffset, 0);
                spineWidgetOne = sW;
            }
            else
            { //0-6
                InsAndAddSWToList(folder + character.ani_idle_path);
                InsAndAddSWToList(folder + character.ani_pAttack_1_path);
                InsAndAddSWToList(folder + character.ani_pAttack_2_path);
                InsAndAddSWToList(folder + character.ani_attack_1_path);
                InsAndAddSWToList(folder + character.ani_attack_2_path);
                InsAndAddSWToList(folder + character.ani_defence_path);
                InsAndAddSWToList(folder + character.ani_difeat_path);
            }
            PlayIdle();
        }
        private void InsAndAddSWToList(string path)
        {
            var sW = SpineWidget.GetInstance(transform);
            sW.Initialize(path);
            sW.transform.localPosition = new Vector3(0, character.yOffset, 0);
            spineWidgets.Add(sW); //add sW to list
        }

        private void UIInit()
        {
            charStats.charCtrl = this;
            charStats.InitUI();
            sliderHP = persPos.Find("sliderHP").GetComponent<Slider>();
            if (sliderHP != null) sliderHP.maxValue = maxHp;

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
            UpdateUI();
        }

        public void CharPortraitSet()
        {
            charStats.charCtrl = this;
            charStats.SetUI();
        }

        public void UpdateUI()
        {
            string hpTxt = $"{hp}/{maxHp}";
            if (hpTMP != null) hpTMP.text = hpTxt;
            if (sliderHP != null) sliderHP.value = hp;
            if (isOverlord)
            {
                string mpTxt = $"{mp}/{maxMp}";
                if (mpTMP != null) mpTMP.text = mpTxt;
                if (sliderMP != null) sliderMP.value = mp;
            }
            charStats.UpdateUI();
        }

        private void PlayAnimID(int listID, string name, bool loop)
        {
            spineWidgets[listID].PlayAnimation(name, loop);
            foreach (var item in spineWidgets) item.gameObject.SetActive(false);
            spineWidgets[listID].gameObject.SetActive(true);
        }

        public void PlayIdle()
        {
            if (isOverlord)
                spineWidgetOne.PlayAnimation(character.ani_idle_name, true);
            else
                PlayAnimID(0, character.ani_idle_name, true);
        }



        public void Attack(int attackID, CharController target)
        {
            BattleIn();
            StartCoroutine(PlayAttack(attackID, target));
        }

        IEnumerator PlayAttack(int id, CharController target)
        {
            if (isOverlord)
            {
                currentPreAttackDuration = 1f;
                switch (id)
                {
                    case 1:
                        spineWidgetOne.PlayAnimation(character.ani_pAttack_2_name, false);
                        yield return new WaitForSeconds(currentPreAttackDuration);
                        spineWidgetOne.PlayAnimation(character.ani_attack_2_name, false);
                        yield return new WaitForSeconds(1);
                        target.Damage(damage_1);
                        break;
                    case 2:
                        spineWidgetOne.PlayAnimation("prepair3", false);
                        yield return new WaitForSeconds(currentPreAttackDuration);
                        spineWidgetOne.PlayAnimation("attack3", false);
                        yield return new WaitForSeconds(1f);
                        target.Damage(damage_2);
                        break;
                    case 3:
                        currentPreAttackDuration = 1f;
                        spineWidgetOne.PlayAnimation("prepair4", false);
                        yield return new WaitForSeconds(currentPreAttackDuration);
                        spineWidgetOne.PlayAnimation("attack4", false);
                        yield return new WaitForSeconds(1f);
                        target.Damage(damage_2);
                        break;
                    default: //0 or else...
                        spineWidgetOne.PlayAnimation(character.ani_pAttack_1_name, false);
                        yield return new WaitForSeconds(currentPreAttackDuration);
                        spineWidgetOne.PlayAnimation(character.ani_attack_1_name, false);
                        yield return new WaitForSeconds(1);
                        target.Damage(damage_1);
                        break;
                }
            }
            else
            {

                if (id == 0)
                {
                    currentPreAttackDuration = aniDuration[1]; //send to target
                    PlayAnimID(1, character.ani_pAttack_1_name, false);
                    yield return new WaitForSeconds(currentPreAttackDuration);
                    PlayAnimID(3, character.ani_attack_1_name, false);
                    yield return new WaitForSeconds(aniDuration[3]);
                    target.Damage(damage_1);// * (isDamageBuff ? buffDamageScale : 1)
                }
                else
                {
                    currentPreAttackDuration = aniDuration[2];
                    PlayAnimID(2, character.ani_pAttack_2_name, false);
                    yield return new WaitForSeconds(currentPreAttackDuration);
                    PlayAnimID(4, character.ani_attack_2_name, false);
                    yield return new WaitForSeconds(aniDuration[4]);
                    target.Damage(damage_2);// * (isDamageBuff ? buffDamageScale : 1)
                }
            }

            PlayIdle();
            BattleOut();
            bm.BattleOut();
        }

        IEnumerator PlayDefence(float preAttackDuration)
        {
            BattleIn();
            yield return new WaitForSeconds(preAttackDuration);
            if (isOverlord)
                spineWidgetOne.PlayAnimation(character.ani_defence_name, false);
            else
                PlayAnimID(5, character.ani_defence_name, false);
            yield return new WaitForSeconds(aniDuration[5]);
            PlayIdle();
            BattleOut();
        }

        public void Select()
        {
            if (bm.battleState == BattleManager.BattleState.PLAYER && !isDead)
            {
                bm.ccOnClick = this; //type CharController
                var borderActive = !border.activeSelf;
                border.SetActive(borderActive);
            }
            if (!isEnemy)
                CharPortraitSet();
        }



        public void Highlight() //On UI Border
        {
            border.SetActive(true);
        }

        public void BattleIn()
        {
            border.SetActive(false);
            transform.SetParent(battlePos);
            rt.DOAnchorPos(Vector2.zero, 0.25f);
            rt.DOScale(battleScale, 0.25f);
            //UI.SetActive(false)
            //SetParentTo Battle Pos

            //Change Scale To BattleScale
        }
        public void BattleOut()
        {
            transform.SetParent(persPos);
            transform.SetSiblingIndex(0);
            rt.DOAnchorPos(Vector2.zero, 0.25f);
            rt.DOScale(idleScale, 0.25f);
        }

        public void Defence(CharController attacker)
        {
            StartCoroutine(PlayDefence(attacker.currentPreAttackDuration));
        }

        public void Damage(int value)
        {
            //play animation damage
            //play hp UI animation
            //value = Mathf.Max(value - (isDefence ? defence : 0), 0);
            if (value > 0)
            {
                hp -= value;
                hp = Mathf.Max(hp, 0);
                if (hp <= 0)
                {
                    isDead = true;
                    bt.onClick.RemoveAllListeners();
                    StartCoroutine(PlayDead());
                    bm.StateUpdate();
                }
            }
            UpdateUI();
        }

        IEnumerator PlayDead()
        {
            yield return new WaitForSeconds(0.2f);
            if (isOverlord) { 
                spineWidgetOne.PlayAnimation(character.ani_defeat_name, false);
                yield return new WaitForSeconds(1.333f);
                spineWidgetOne.PlayAnimation("defeat2", true);
            }
            else
                PlayAnimID(6, character.ani_defeat_name, false);
        }

        public void Heal(int value)
        {
            //play animation heal
            hp += value;
            hp = Mathf.Min(hp, maxHp);
        }
        public void CastMahou(int id)
        {
            //drop spell or skill
        }
    }
}