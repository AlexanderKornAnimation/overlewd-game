using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Overlewd
{
    public class SkillController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public Skill oldSkill;
        public AdminBRO.CharacterSkill skill;
        public bool isHeal => skill.actionType == "heal";
        private GameObject vfx => oldSkill.vfx;
        private string sfx => oldSkill.sfx;

        public Image image;
        public Image effectSlot;
        private List<Sprite> effectIcons;
        private GameObject selectBorder;
        public bool select;
        private Slider slider;
        private TextMeshProUGUI textCount;
        public UnityEvent OnClickAction = new UnityEvent();

        public bool potion => oldSkill.potion;
        public int potionAmount;

        [HideInInspector]
        public Transform vfxSpawnPoint;
        [HideInInspector]
        public int damage, cooldown, cooldownCount = 0;

        //public int amount => skill.amount; //for info maybe
        public int manaCost => skill.manaCost;
        public string Name => skill.name;
        public string description => skill.description;

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
            textCount = GetComponentInChildren<TextMeshProUGUI>();
            selectBorder = transform.Find("select")?.gameObject;
        }
        private void Start()
        {
            StatInit();
        }
        private void StatInit()
        {
            image.sprite = oldSkill.battleIco;
            image.SetNativeSize();

            if (slider != null)
            {
                cooldown = Mathf.RoundToInt(skill.effectCooldownDuration);
                slider.maxValue = cooldown;
                cooldownCount = 0;
                slider.value = cooldownCount;
            }
            if (textCount != null)
                if (oldSkill.potion)
                    textCount.text = $"{potionAmount}";
                else
                    textCount.text = $"{cooldownCount}";
        }

        public void ReplaceSkill(AdminBRO.CharacterSkill sk, Skill skillSkin)
        {
            skill = sk;
            oldSkill = skillSkin;
            if (!potion) SetEffectIco();
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
                if (oldSkill.potion) //old
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
            if (vfx) Instantiate(vfx, target);
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

        void SetEffectIco()
        {
            effectSlot.gameObject.SetActive(true);
            effectSlot.sprite = effectIcons.Find(i => i.name == skill.effect);
            if (effectSlot.sprite == null)
            {
                effectSlot.gameObject.SetActive(false);
            }
        }
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
                    Debug.Log("Descr is open");
                    ShowDiscription();
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
                OnClickAction.Invoke();
            pressTime = 0f;
            pressed = false;
        }
    }
}