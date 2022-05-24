using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class SkillController : MonoBehaviour
    {
        public Skill skill;
        private string skillName;
        private string discription;
        private Image image;
        private GameObject selectBorder;
        public bool select;
        private Slider slider;
        private TextMeshProUGUI textCount;
        public Button button;
        private GameObject vfx;
        private AudioClip sfx;
        [HideInInspector]
        public Transform vfxSpawnPoint;
        [HideInInspector]
        public int power, manaCost, amount, cooldown, cooldownCount = 0;

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
            skillName = skill.skillName;
            discription = skill.discription;
            image.sprite = skill.battleIco;
            image.SetNativeSize();
            vfx = skill.vfx;
            sfx = skill.sfx;
            power = skill.power;
            manaCost = skill.manaCost;
            amount = skill.amount;

            if (slider != null)
            {
                cooldown = skill.cooldown;
                slider.maxValue = cooldown;
                cooldownCount = skill.cooldownCount;
                slider.value = cooldownCount;
            }
            if (textCount != null) 
                if (skill.attackType == Skill.AttackType.POTION)
                    textCount.text = $"{amount}";
                else
                    textCount.text = $"{cooldownCount}";
        }

        public void ReplaceSkill(Skill sk)
        {
            SaveSkill();
            skill = sk;
            StatInit();
        }
        private void SaveSkill()
        {
            if (amount > 0) skill.SaveAmount(amount);
            skill.SaveCDcount(cooldownCount);
        }
        public void StatUpdate()
        {
            if (slider != null)
            {
                slider.maxValue = cooldown;
                slider.value = cooldownCount;
            }
            if (textCount != null)
                if (skill.attackType == Skill.AttackType.POTION)
                    textCount.text = $"{amount}";
                else
                    textCount.text = $"{cooldownCount}";
            SaveSkill();
        }

        public void Select() {
            isSelected = true;
            selectBorder?.SetActive(true);
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
        public void UseSkill()
        {
            if (skill.attackType == Skill.AttackType.POTION)
            {

            }
        }
        
    }
}