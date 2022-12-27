using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Overlewd
{
    public class SkillController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public AdminBRO.CharacterSkill skill;
        public bool isHeal => skill.actionType == "heal";
        //private AdminBRO.Animation vfx => skill.vfxAnimation;
        //private AdminBRO.Sound sfx => skill.sfxAttack1;
        private CharDescription charDescription => FindObjectOfType<CharDescription>();

        public Image image;
        public Image effectSlot;
        private List<Sprite> effectIcons;
        private GameObject selectBorder, goManaCost;
        public bool select;
        private Slider slider;
        private TextMeshProUGUI textCount, textManaCost;
        public UnityEvent OnClickAction = new UnityEvent();

        public Sprite standartIco, overlordIco;

        public bool potion;
        public int potionAmount;

        [HideInInspector]
        public Transform vfxSpawnPoint;
        [HideInInspector]
        public int damage, cooldown, cooldownCount = 0;

        //public int amount => skill.amount; //for info maybe
        public int manaCost => skill.manaCost;
        public string Name => skill.name;
        public string description => skill.GetDescription(skill.amount, skill.effectAmount);
        public int level => skill.level;

        public bool selectable = true;
        public bool isSelected = false;
        public bool silence = false;

        private bool pressed = false;
        private float pressTimer = .5f, pressTime = 0f;

        private SkillDescription skillDescription => FindObjectOfType<SkillDescription>();

        private void Awake()
        {
            effectIcons = new List<Sprite>(Resources.LoadAll<Sprite>("Battle/Images/UI/Status/Big"));
            if (!potion && effectSlot == null) effectSlot = transform.Find("status").GetComponent<Image>();
            image = GetComponent<Image>();
            slider = GetComponentInChildren<Slider>();
            textManaCost = GetComponent<TextMeshProUGUI>();
            selectBorder = transform.Find("select")?.gameObject;
            goManaCost = transform.Find("manaCost")?.gameObject;
            if (potion && textCount == null)
                textCount = transform.Find("text").GetComponent<TextMeshProUGUI>();
            if (goManaCost != null)
                textManaCost = goManaCost.transform.Find("text").GetComponent<TextMeshProUGUI>();
        }
        private void Start() => StatInit();

        private void StatInit()
        {
            if (slider != null)
            {
                cooldown = Mathf.RoundToInt(skill.effectCooldownDuration);
                slider.maxValue = cooldown;
                slider.value = cooldownCount;
            }
            if (goManaCost != null)
            {
                goManaCost.SetActive(manaCost > 0);
                if (textManaCost != null)
                    textManaCost.text = manaCost.ToString();
            }
            if (textCount != null)
                if (potion)
                    textCount.text = $"{potionAmount}";
                else
                    textCount.text = $"{cooldownCount}";
        }

        public void ReplaceSkill(AdminBRO.CharacterSkill sk, Dictionary<AdminBRO.CharacterSkill, int> cd, bool isOverlord = false)
        {
            if (sk != null)
            {
                if (!cd.TryGetValue(sk, out cooldownCount))
                    cooldownCount = 0;
            }
            else
                sk = new AdminBRO.CharacterSkill();
            skill = sk;
            if (!potion)
            {
                if (isOverlord)
                    image.sprite = overlordIco;
                else
                    image.sprite = standartIco;
                image.SetNativeSize();
                SetEffectIco();
            }
            StatInit();
        }

        public void StatUpdate()
        {
            if (slider != null)
            {
                slider.maxValue = cooldown;
                slider.value = cooldownCount;
            }
            if (textCount != null)
                if (potion)
                    textCount.text = $"{potionAmount}";
                else
                    textCount.text = $"{cooldownCount}";
        }

        public bool Press()
        {
            isSelected = !isSelected;
            selectBorder?.SetActive(isSelected);
            return isSelected;
        }
        public void Unselect()
        {
            isSelected = false;
            selectBorder?.SetActive(false);
        }

        public void InstVFX(Transform target)
        {
            return;//if (vfx) SpineWidget.Instantiate(vfx, target);
        }

        public void Disable()
        {
            selectable = false;
            image.color = Color.red;
        }
        public void Enable()
        {
            selectable = true;
            image.color = Color.white;
        }
        public void BlinkDisable()
        {
            image.color = Color.white;
            image.DOColor(Color.red, 0.1f).SetLoops(4, LoopType.Yoyo).SetEase(Ease.InOutExpo);
        }

        public void ShowDiscription()
        {
            skillDescription.Open(this);
        }
        public bool CheckMana(float mana) => manaCost <= mana;

        void SetEffectIco()
        {
            effectSlot.gameObject.SetActive(true);
            effectSlot.sprite = effectIcons.Find(i => i.name == skill?.effect);
            if (effectSlot.sprite == null)
            {
                effectSlot.gameObject.SetActive(false);
            }
        }
        public bool SkillOnCD() => cooldownCount > 0;

        private void Update()
        {
            if (pressed && !potion)
            {
                if (pressTime < pressTimer)
                {
                    pressTime += Time.deltaTime;
                }
                else
                {
                    ShowDiscription();
                    pressTime = 0f;
                    pressed = false;
                }
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            eventData.clickTime = 0f;
            pressTime = 0f;
            pressed = true;
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            skillDescription?.Close();
            if (pressed)
            {
                charDescription.Close();
                OnClickAction.Invoke();
            }
            pressTime = 0f;
            pressed = false;
        }
    }
}