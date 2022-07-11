using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class CharacterPortrait : MonoBehaviour
    {
        public CharController cc;
        public Button button;
        [HideInInspector] public bool bigPortrait = false;
        private TextMeshProUGUI textHP;
        private TextMeshProUGUI textMP;
        private Slider sliderHP;
        private Slider sliderMP;
        private Image BattleBackgroundIco;
        private Image BattlePortraitIco;
        [SerializeField]
        private Sprite[] backRarityIcons;

        private StatusEffects status_bar;

        private int level => cc.level;
        private string rarity => cc.rarity;
        private float hp => cc.health;
        private float maxHp => cc.healthMax;
        private float mp => cc.mana;
        private float maxMp => cc.manaMax;

        private void Awake()
        {
            status_bar = transform.Find("status_bar").GetComponent<StatusEffects>();
            button = GetComponent<Button>();
        }

        public void InitUI()
        {
            BattleBackgroundIco = GetComponent<Image>();
            BattlePortraitIco = transform.Find("Portrait")?.GetComponent<Image>();
            sliderHP = transform.Find("sliderHP")?.GetComponent<Slider>();
            sliderMP = transform.Find("sliderMP")?.GetComponent<Slider>();
            textHP = transform.Find("sliderHP/Text")?.GetComponent<TextMeshProUGUI>();
            textMP = transform.Find("sliderMP/Text")?.GetComponent<TextMeshProUGUI>();

            if (sliderHP) sliderHP.maxValue = maxHp;
            if (sliderMP) sliderMP.maxValue = maxMp;

            if (BattleBackgroundIco != null && backRarityIcons.Length != 0)
                switch (rarity)
                {
                    case AdminBRO.Character.Rarity_Basic:
                        BattleBackgroundIco.sprite = backRarityIcons[0];
                        break;
                    case AdminBRO.Character.Rarity_Advanced:
                        BattleBackgroundIco.sprite = backRarityIcons[1];
                        break;
                    case AdminBRO.Character.Rarity_Epic:
                        BattleBackgroundIco.sprite = backRarityIcons[2];
                        break;
                    case AdminBRO.Character.Rarity_Heroic:
                        BattleBackgroundIco.sprite = backRarityIcons[3];
                        break;
                    default:
                        BattleBackgroundIco.sprite = backRarityIcons[0];
                        break;
                }
            if (cc && status_bar)
            {
                status_bar.cc = cc;
                status_bar.UpdateStatuses();
            }
            if (cc != null && button != null)
                button.onClick.AddListener(cc.Select);

            if (bigPortrait)
            {
                BattlePortraitIco.sprite = cc.characterRes.bigPortrait;
                transform.SetSiblingIndex(0);
            }
            else
            {
                BattlePortraitIco.sprite = cc.characterRes.icoPortrait;
                if (transform.GetSiblingIndex() > cc.battleOrder)
                {
                    transform.SetSiblingIndex(5);
                }
            }
            sliderMP?.gameObject.SetActive(cc.isOverlord);
            UpdateUI();
        }

        public void SetUI(CharController replaceCharacter = null)
        {
            if (replaceCharacter)
                cc = replaceCharacter;
            BattlePortraitIco.sprite = cc.characterRes.bigPortrait;
            if (sliderHP) sliderHP.maxValue = maxHp;
            sliderMP?.gameObject.SetActive(cc.isOverlord);
            status_bar.cc = cc;
            status_bar?.UpdateStatuses();
            UpdateUI();
        }

        public void UpdateUI()
        {
            textHP.text = $"{hp}/{maxHp}";
            sliderHP.value = hp;
            if (textMP) textMP.text = $"{mp}/{maxMp}";
            if (sliderMP) sliderMP.value = mp;
            status_bar?.UpdateStatuses();
        }
    }
}