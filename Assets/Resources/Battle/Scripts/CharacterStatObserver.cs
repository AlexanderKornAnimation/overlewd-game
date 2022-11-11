using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Overlewd
{
    public class CharacterStatObserver : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {

        public CharController cc;
        public Transform persPos => cc.persPos;
        public CharacterPortrait charStats => cc.charStats;

        //Char UI
        public Image charClass;
        public Slider sliderHP;
        public Slider sliderMP;
        public Slider whiteSlider;
        public bool showMP => cc.isOverlord;
        public TextMeshProUGUI hpTMP;
        public TextMeshProUGUI mpTMP;
        public StatusEffects status_bar;
        public GameObject border;
        private CharDescription charDescription => FindObjectOfType<CharDescription>();

        public float hp => cc.health;
        public float maxHp => cc.healthMax;
        public float mana => cc.mana;
        public float manaMax => cc.manaMax;
        public bool isEnemy => cc.isEnemy;
        public bool isOverlord => cc.isOverlord;


        [SerializeField]
        private List<Sprite> classIcons;
        private void Start()
        {
            status_bar = transform.Find("status_bar").GetComponent<StatusEffects>();
            status_bar.cc = cc;
            border = transform.Find("button/border").gameObject;
            border?.SetActive(false);
            border.GetComponent<RectTransform>().anchoredPosition = (cc.isOverlord) ? new Vector2(0, 0) : new Vector2(0, -60);
            if (sliderHP) sliderHP.maxValue = maxHp;
            if (sliderMP) sliderMP.maxValue = manaMax;
            sliderMP?.gameObject.SetActive(showMP);
            if (charClass && classIcons != null) SetClass();
            UpdateUI();
            UpdateStatuses();
            //sliderHP.onValueChanged.AddListener(delegate { ChangeHP(); });
        }

        public void UpdateUI()
        {
            string hpTxt = $"{hp}/{maxHp}";
            if (hpTMP != null) hpTMP.text = hpTxt; else Debug.Log("hpTMP = null");
            if (sliderHP != null) sliderHP.DOValue(hp, 0.3f).SetEase(Ease.OutQuint); //value = health;
            if (isOverlord)
            {
                string mpTxt = $"{mana}/{manaMax}";
                if (mpTMP != null) mpTMP.text = mpTxt;
                if (sliderMP != null) sliderMP.value = mana;
            }
            if (whiteSlider)
                StartCoroutine(HPChangePause());
            charStats?.UpdateUI();
        }
        IEnumerator HPChangePause()
        {
            whiteSlider.fillRect.GetComponent<Image>().enabled = true;
            whiteSlider.maxValue = maxHp;
            yield return new WaitForSeconds(0.6f);
            whiteSlider.DOValue(hp, 0.3f).SetEase(Ease.OutQuint);
            yield return new WaitForSeconds(0.6f);
            whiteSlider.fillRect.GetComponent<Image>().enabled = false;
        }

        public void UpdateStatuses() => status_bar?.UpdateStatuses();
        public void Border(bool brd) => border?.SetActive(brd);
        void SetClass()
        {
            switch (cc.character.characterClass)
            {
                case AdminBRO.CharacterClass.Assassin:
                    charClass.sprite = classIcons[1];
                    break;
                case AdminBRO.CharacterClass.Bruiser:
                    charClass.sprite = classIcons[2];
                    break;
                case AdminBRO.CharacterClass.Caster:
                    charClass.sprite = classIcons[3];
                    break;
                case AdminBRO.CharacterClass.Healer:
                    charClass.sprite = classIcons[4];
                    break;
                case AdminBRO.CharacterClass.Tank:
                    charClass.sprite = classIcons[5];
                    break;
                default:
                    charClass.sprite = classIcons[0];
                    break;
            }
            charClass.SetNativeSize();
        }

        private bool pressed = false;
        private float pressTimer = .5f, pressTime = 0f;
        private void Update()
        {
            if (pressed)
            {
                if (pressTime < pressTimer)
                {
                    pressTime += Time.deltaTime;
                }
                else
                {
                    Debug.Log("Descr is open");
                    charDescription.Open(cc);
                    pressTime = 0f;
                    pressed = false;
                }
            }
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            pressTime = 0f;
            pressed = true;
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            if (pressed)
            {
                charDescription.Close();
                cc.Select();
            }
            pressTime = 0f;
            pressed = false;
        }
        private void OnGUI()
        {
            if (cc.bm.debug > 0)
            {
                Vector3 pos = cc.persPos.position;
                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.black;
                style.fontSize = 18;

                if (cc.bm.debug == 1)
                {
                    bool skills = cc.character.skills != null;

                    //GUI.Box(new Rect(pos.x + 34, pos.y - 156, 184, 404), GUIContent.none);
                    for (int i = 0; i < 3; i++)
                    {
                        if (i == 2)
                            style.normal.textColor = Color.white;

                        GUI.Label(new Rect(pos.x + 40, pos.y - 250 - i * 2, 180, 400),
                            $"Name: {cc.Name}\n" +
                            $"Level: {cc.character.level}\n" +
                            $"Rarity: {cc.character.rarity}\n" +
                            $"Class: {cc.character.characterClass}\n\n" +

                            $"Sp: {cc.speed}  Pw: {cc.power} \nCn: {cc.constitution}  Ag: {cc.agility}\n\n" +

                            $"Damage: {cc.character.damage}\n" +
                            $"Max Mana: {cc.character.mana}\n" +
                            $"Damage: {cc.character.damage}\n\n" +

                            $"PSR\n"+
                            $"accyracy: {cc.psr.accyracy}\n" +
                            $"crit: {cc.psr.crit}\n" +
                            $"dodge: {cc.psr.dodge}\n" +
                            $"effectProb: {cc.psr.effectProb}\n",
                            style);

                    }
                }
                else if (cc.bm.debug == 2)
                {
                    //GUI.Box(new Rect(pos.x + 34, pos.y - 156, 184, 404), GUIContent.none);
                    for (int i = 0; i < 3; i++)
                    {
                        if (i == 2)
                            style.normal.textColor = Color.white;

                        var k = 0;
                        foreach (var item in cc.skill)
                        {
                            GUI.Label(new Rect(pos.x + 40, pos.y - 250 + k * 180 - i * 2, 180, 480),
                            $"Skill {k} Name: {item.name}\n" +
                            $"Damage Amount: {item.amount}\n" + //scale in % - (amount/100)
                            $"AOE: {item.AOE}   EffectProb: {item.effectProb}\n" +
                            $"isHeal: {item.actionType}\n" +
                            $"Type: {item.type}\n" +
                            $"Effect: {item.effect}", style);
                            k++;
                        }
                        foreach (var item in cc.passiveSkill)
                        {
                            GUI.Label(new Rect(pos.x + 60, pos.y - 250 + k * 180 - i * 2, 180, 480),
                            $"Skill {k} Name: {item.name}\n" +
                            $"Damage Amount: {item.amount}\n" + //scale in % - (amount/100)
                            $"AOE: {item.AOE}   EffectProb: {item.effectProb}\n" +
                            $"isHeal: {item.actionType}\n" +
                            $"Type: {item.type}\n" +
                            $"Trigger: {item.trigger}\n" +
                            $"Effect: {item.effect}", style);
                            k++;
                        }
                    }
                }
            }
        }

    }
}