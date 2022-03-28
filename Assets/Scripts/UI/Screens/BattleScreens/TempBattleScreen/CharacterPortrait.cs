using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class CharacterPortrait : MonoBehaviour
    {
        public CharController charCtrl;
        public Button button;
        [HideInInspector] public bool isPlayer = false;
        private Transform attack, defence, bleeding, burning, poison;
        private TextMeshProUGUI attackScale, defenceScale, bleedingScale, burningScale, poisonScale;
        private TextMeshProUGUI textHP;
        private Slider sliderHP;
        private Image BattlePortraitIco;

        private int hp, maxHp;

        public void InitUI()
        {
            BattlePortraitIco = GetComponent<Image>();
            sliderHP = transform.Find("sliderHP").GetComponent<Slider>();
            textHP = transform.Find("sliderHP/Text").GetComponent<TextMeshProUGUI>();

            hp = charCtrl.hp;
            maxHp = charCtrl.maxHp;
            sliderHP.maxValue = maxHp;

            if (charCtrl != null && button != null)
                button.onClick.AddListener(charCtrl.Select);

            if (isPlayer)
            {
                BattlePortraitIco.sprite = charCtrl.character.bigPortrait;
                transform.SetSiblingIndex(0);
            }
            else
            {
                BattlePortraitIco.sprite = charCtrl.character.battlePortrait;
                if (transform.GetSiblingIndex() > charCtrl.character.Order) 
                {
                    transform.SetSiblingIndex(5);
                }
                
            }
            UpdateUI();
        }

        public void SetUI()
        {
            BattlePortraitIco.sprite = charCtrl.character.bigPortrait;
            hp = charCtrl.hp;
            maxHp = charCtrl.maxHp;
            sliderHP.maxValue = maxHp;
            UpdateUI();
        }

        public void UpdateUI()
        {
            textHP.text = $"{hp}/{maxHp}";
            sliderHP.value = hp;
        }
    }
}