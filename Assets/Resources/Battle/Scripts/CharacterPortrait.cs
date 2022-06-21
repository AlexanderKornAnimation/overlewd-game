using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class CharacterPortrait : MonoBehaviour
    {
        public CharController charCtrl;
        public Button button;
        [HideInInspector] public bool bigPortrait = false;
        private Transform attack, defence, bleeding, burning, poison;
        private TextMeshProUGUI attackScale, defenceScale, bleedingScale, burningScale, poisonScale;
        private TextMeshProUGUI textHP;
        private TextMeshProUGUI textMP;
        private Slider sliderHP;
        private Slider sliderMP;
        private Image BattlePortraitIco;

        private float hp => charCtrl.health;
        private float maxHp => charCtrl.healthMax;
        private float mp => charCtrl.mana;
        private float maxMp => charCtrl.manaMax;

        public void InitUI()
        {
            BattlePortraitIco = GetComponent<Image>();
            sliderHP = transform.Find("sliderHP")?.GetComponent<Slider>();
            sliderMP = transform.Find("sliderMP")?.GetComponent<Slider>();
            textHP = transform.Find("sliderHP/Text")?.GetComponent<TextMeshProUGUI>();
            textMP = transform.Find("sliderMP/Text")?.GetComponent<TextMeshProUGUI>();

            if (sliderHP) sliderHP.maxValue = maxHp;
            if (sliderMP) sliderMP.maxValue = maxMp;

            if (charCtrl != null && button != null)
                button.onClick.AddListener(charCtrl.Select);

            if (bigPortrait)
            {
                BattlePortraitIco.sprite = charCtrl.characterRes.bigPortrait;
                transform.SetSiblingIndex(0);
            }
            else
            {
                BattlePortraitIco.sprite = charCtrl.characterRes.battlePortrait;
                if (transform.GetSiblingIndex() > charCtrl.battleOrder) 
                {
                    transform.SetSiblingIndex(5);
                }
            }
            sliderMP?.gameObject.SetActive(charCtrl.isOverlord);
            UpdateUI();
        }

        public void SetUI(CharController replaceCharacter = null)
        {
            if (replaceCharacter)
                charCtrl = replaceCharacter;
            BattlePortraitIco.sprite = charCtrl.characterRes.bigPortrait;
            if (sliderHP) sliderHP.maxValue = maxHp;
            sliderMP?.gameObject.SetActive(charCtrl.isOverlord);
            UpdateUI();
        }

        public void UpdateUI()
        {
            textHP.text = $"{hp}/{maxHp}";
            sliderHP.value = hp;
            if (textMP) textMP.text = $"{mp}/{maxMp}";
            if (sliderMP) sliderMP.value = mp;
        }
    }
}