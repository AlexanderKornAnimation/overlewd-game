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
        private TextMeshProUGUI textMP;
        private Slider sliderHP;
        private Slider sliderMP;
        private Image BattlePortraitIco;

        private int hp, maxHp, mp, maxMp;

        public void InitUI()
        {
            BattlePortraitIco = GetComponent<Image>();
            sliderHP = transform.Find("sliderHP")?.GetComponent<Slider>();
            sliderMP = transform.Find("sliderMP")?.GetComponent<Slider>();
            textHP = transform.Find("sliderHP/Text")?.GetComponent<TextMeshProUGUI>();
            textMP = transform.Find("sliderMP/Text")?.GetComponent<TextMeshProUGUI>();

            hp = charCtrl.hp;
            mp = charCtrl.mp;
            maxHp = charCtrl.maxHp;
            maxMp = charCtrl.maxMp;
            if (sliderHP) sliderHP.maxValue = maxHp;
            if (sliderMP) sliderMP.maxValue = maxMp;

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
                if (transform.GetSiblingIndex() > charCtrl.battleOrder) 
                {
                    transform.SetSiblingIndex(5);
                }
            }
            sliderMP?.gameObject.SetActive(charCtrl.isOverlord);
            UpdateUI();
        }

        public void SetUI()
        {
            BattlePortraitIco.sprite = charCtrl.character.bigPortrait;
            hp = charCtrl.hp;
            maxHp = charCtrl.maxHp;
            if (sliderHP) sliderHP.maxValue = maxHp;
            mp = charCtrl.mp;
            maxMp = charCtrl.maxMp;
            sliderMP?.gameObject.SetActive(charCtrl.isOverlord);
            UpdateUI();
        }

        public void UpdateUI()
        {
            hp = charCtrl.hp;
            mp = charCtrl.mp;
            textHP.text = $"{hp}/{maxHp}";
            sliderHP.value = hp;
            if (textMP) textMP.text = $"{mp}/{maxMp}";
            if (sliderMP) sliderMP.value = mp;
        }
    }
}