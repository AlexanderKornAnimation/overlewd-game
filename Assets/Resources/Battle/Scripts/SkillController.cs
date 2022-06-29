using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class SkillController : MonoBehaviour
    {
        public Skill oldSkill;
        public AdminBRO.CharacterSkill skill;
        public bool isHeal => skill.actionType == "heal";
        private GameObject vfx => oldSkill.vfx;
        private AudioClip sfx => oldSkill.sfx;

        private Image image;
        [SerializeField] private Image effectSlot;
        [SerializeField] private List<Sprite> effectIcons;
        private GameObject selectBorder;
        public bool select;
        private Slider slider;
        private TextMeshProUGUI textCount;
        public Button button;

        public bool potion => oldSkill.potion;
        public int potionAmount;

        [HideInInspector]
        public Transform vfxSpawnPoint;
        [HideInInspector]
        public int damage, cooldown, cooldownCount = 0;

        //public int amount => skill.amount; //for info maybe
        public int manaCost => skill.manaCost;


        public bool selectable = true;
        public bool isSelected = false;
        public bool silence = false;

        private void Awake()
        {
            if (!potion && effectSlot == null) effectSlot = transform.Find("status").GetComponent<Image>();
            button = GetComponent<Button>();
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
            damage = oldSkill.damage; // old

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

        void SetEffectIco()
        {
            switch (skill.effect)
            {
                case "defense_up":
                    effectSlot.sprite = effectIcons[0];
                    break;
                case "defense_down":
                    effectSlot.sprite = effectIcons[1];
                    break;
                case "focus":
                    effectSlot.sprite = effectIcons[2];
                    break;
                case "blind":
                    effectSlot.sprite = effectIcons[3];
                    break;
                case "regeneration":
                    effectSlot.sprite = effectIcons[4];
                    break;
                case "poison":
                    effectSlot.sprite = effectIcons[5];
                    break;
                case "bless":
                    effectSlot.sprite = effectIcons[6];
                    break;
                case "heal_block":
                    effectSlot.sprite = effectIcons[7];
                    break;
                case "silence":
                    effectSlot.sprite = effectIcons[8];
                    break;
                case "immunity":
                    effectSlot.sprite = effectIcons[9];
                    break;
                case "stun":
                    effectSlot.sprite = effectIcons[10];
                    break;
                case "curse":
                    effectSlot.sprite = effectIcons[11];
                    break;
                case "dispel":
                    effectSlot.sprite = effectIcons[12];
                    break;
                case "safeguard":
                    effectSlot.sprite = effectIcons[13];
                    break;
                default:
                    effectSlot.sprite = effectIcons[0];
                    break;
            }
        }

    }
}