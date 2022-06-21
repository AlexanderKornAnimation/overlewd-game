using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class SkillController : MonoBehaviour
    {
        public Skill oldSkill;
        public AdminBRO.CharacterSkill skill;
        public bool isHeal => skill.actionType == "heal"? true : false;
        private GameObject vfx => oldSkill.vfx;

        private Image image;
        private GameObject selectBorder;
        public bool select;
        private Slider slider;
        private TextMeshProUGUI textCount;
        public Button button;

        private AudioClip sfx;
        [HideInInspector]
        public Transform vfxSpawnPoint;
        [HideInInspector]
        public int damage, amount, cooldown, cooldownCount = 0;
        public int manaCost => skill.manaCost;


        public bool selectable = true;
        public bool isSelected = false;

        private void Awake()
        {
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
            sfx = oldSkill.sfx;
            damage = oldSkill.damage; // old 
            amount = skill.amount;

            if (slider != null)
            {
                cooldown = Mathf.RoundToInt(skill.effectCooldownDuration);
                slider.maxValue = cooldown;
                cooldownCount = 0;
                slider.value = cooldownCount;
            }
            if (textCount != null)
                if (oldSkill.attackType == Skill.AttackType.POTION)
                    textCount.text = $"{amount}";
                else
                    textCount.text = $"{cooldownCount}";
        }

        public void ReplaceSkill(AdminBRO.CharacterSkill sk)
        {
            skill = sk;
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
                if (oldSkill.attackType == Skill.AttackType.POTION) //old
                    textCount.text = $"{amount}";
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
            image.DOColor(Color.red, 0.1f).SetLoops(4, LoopType.Yoyo).SetEase(Ease.InOutExpo);
        }
    }
}