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
        public string Name;
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

        public CharacterRes characterRes;

        public AdminBRO.Character broCharacter;

        public float speed => (float)broCharacter.speed;

        public Skill[] skill;
        private float idleScale = 1f, battleScale = 1.4f;
        public int battleOrder = 1;
        public float health = 100, healthMax = 100;
        public float mana = 100, manaMax = 100;
        public float damage = 10;

        [HideInInspector] public bool isDamageBuff = false;
        [HideInInspector] public int buffDamageScale = 1;

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
            StartCoroutine(LateInint());
        }

        private void StatInit()
        {
            /*
            * class for icons
            * level
            * stats: speed, power etc...
            * items
            * 
            * character key associete:
            * 
            * animations
            * icons
            * battle scills unique animations
            * 
            */
            Name = broCharacter.name;
            isOverlord = broCharacter.characterClass == AdminBRO.Character.Class_Overlord;

            health = (float)broCharacter.health; healthMax = health;
            mana = (float)broCharacter.mana; manaMax = mana;
            damage = (float)broCharacter.damage;

            //characterRes find and attach by broCharacter.key/name;

            idleScale = 1f;
            battleScale = 1.1f;

            /*
            isEnemy = character.isEnemy;
            battleOrder = character.Order;
            characterContent.ApplyStats(); //apply class bonus
            isOverlord = character.isOverlord;
            health = characterContent.hp; healthMax = characterContent.maxHp;
            mana = characterContent.mp; manaMax = characterContent.maxMp;
            idleScale = characterContent.idleScale;
            battleScale = characterContent.battleScale;
            damage = (int)Math.Ceiling(characterContent.damage);
            */
            skill = characterRes.skill;
        }

        public void PowerBuff()
        {
            buffDamageScale *= 2;
        }

        private void ShapeInit()
        {
            rt = gameObject.AddComponent<RectTransform>();
            UITools.SetStretch(rt);
            rt.localScale *= idleScale;

            Transform spawnPos = bm.transform;
            battleLayer = spawnPos.Find("BattleCanvas/BattleLayer").transform;
            if (isEnemy)
            {
                if (isBoss)
                {
                    battlePos = battleLayer.Find("battlePosBoss").transform;
                    persPos = battleLayer.Find("boss").transform;
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

            var sW = characterRes.characterPrefab ?
                SpineWidget.GetInstance(characterRes.characterPrefab, transform) :
                SpineWidget.GetInstance(characterRes.idle_Prefab_Path, transform);
            sW.transform.localPosition = new Vector3(0, characterRes.yOffset, 0);
            spineWidget = sW;
            defenceDuration = spineWidget.GetAnimationDuaration(characterRes.ani_defence_name);
        }

        private void UIInit()
        {
            charStats.charCtrl = this;
            charStats.InitUI();
            sliderHP = persPos.Find("sliderHP").GetComponent<Slider>();
            if (sliderHP != null) sliderHP.maxValue = healthMax; else Debug.Log("sliderHP is null");
            hpTMP = persPos.Find("sliderHP/Text").GetComponent<TextMeshProUGUI>();

            if (isOverlord)
            {
                sliderMP = persPos.Find("sliderMP").GetComponent<Slider>();
                if (sliderMP != null) sliderMP.maxValue = manaMax;
                mpTMP = persPos.Find("sliderMP/Text").GetComponent<TextMeshProUGUI>();
            }

            bt = persPos.Find("button").GetComponent<Button>();
            border = persPos.Find("button/border").gameObject;
            border.SetActive(false);
            bt.onClick.AddListener(Select);
        }

        IEnumerator LateInint()
        {
            yield return new WaitForSeconds(0.01f);
            sliderHP.gameObject.SetActive(true);
            bt.gameObject.SetActive(true);
        }

        public void CharPortraitSet()
        {
            charStats.charCtrl = this;
            charStats.SetUI();
        }

        public void UpdateUI()
        {
            string hpTxt = $"{health}/{healthMax}";
            if (hpTMP != null) hpTMP.text = hpTxt; else Debug.Log("hpTMP = null");
            if (sliderHP != null) sliderHP.value = health;
            if (isOverlord)
            {
                string mpTxt = $"{mana}/{manaMax}";
                if (mpTMP != null) mpTMP.text = mpTxt;
                if (sliderMP != null) sliderMP.value = mana;
            }
            charStats.UpdateUI();
        }
        public void PlayIdle()
        {
            if (!isDead)
                spineWidget.PlayAnimation(characterRes.ani_idle_name, true);
        }

        public void Attack(int attackID, CharController target = null)
        {
            BattleIn();
            if (target != null) target.BattleIn();
            StartCoroutine(PlayAttack(attackID, target));
        }

        IEnumerator PlayAttack(int id, CharController target = null)
        {
            if (id > characterRes.skill.Length) id = 0; //if id overflow on skill array

            vfxDuration = characterRes.skill[id].vfxDuration;
            attackDuration = spineWidget.GetAnimationDuaration(characterRes.skill[id].attackAnimationName);
            damage = Mathf.RoundToInt(characterRes.skill[id].power + (float)broCharacter.damage * buffDamageScale);

            if (characterRes.skill[id].prepairAnimationName == "")
                preAttackDuration = 0f;
            else
            {
                preAttackDuration = spineWidget.GetAnimationDuaration(characterRes.skill[id].prepairAnimationName);  //send to target Defence Animation
                spineWidget.PlayAnimation(characterRes.skill[id].prepairAnimationName, false);
            }

            if (isOverlord) mana -= characterRes.skill[id].manaCost;
            yield return new WaitForSeconds(preAttackDuration);
            if (characterRes.skill[id].vfx != null)
            {
                Instantiate(characterRes.skill[id].vfx, battleLayer);
                yield return new WaitForSeconds(vfxDuration);
            }
            spineWidget.PlayAnimation(characterRes.skill[id].attackAnimationName, false);
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
            spineWidget.PlayAnimation(characterRes.ani_defence_name, false);
            yield return new WaitForSeconds(defenceDuration / 2);
            Damage(attacker.damage);
            yield return new WaitForSeconds(defenceDuration / 2);
            PlayIdle();
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

        public void Highlight() => border?.SetActive(true);
        public void UnHiglight() => border?.SetActive(false);

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
        public void Damage(float value)
        {
            if (value > 0)
            {
                health -= value;
                health = Mathf.Max(health, 0);
                if (health <= 0)
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
                spineWidget.PlayAnimation(characterRes.ani_defeat_name, false);
                yield return new WaitForSeconds(spineWidget.GetAnimationDuaration(characterRes.ani_defeat_name));
                spineWidget.PlayAnimation("defeat2", true); //!костыль
            }
            else
                spineWidget.PlayAnimation(characterRes.ani_defeat_name, false);
        }

        public void Heal(int value)
        {
            //heal fx or animation
            health += value;
            health = Mathf.Min(health, healthMax);
            UpdateUI();
        }
        public void HealMP(int value)
        {
            mana += value;
            mana = Mathf.Min(mana, manaMax);
            UpdateUI();
        }
        private void OnDestroy()
        {
            bm.unselect -= UnHiglight;
            sliderHP.gameObject.SetActive(false);
            bt.gameObject.SetActive(false);
        }
    }
}