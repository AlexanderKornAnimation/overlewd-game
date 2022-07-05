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
        private Image BattlePortraitIco;

        private StatusEffects status_bar;

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
            BattlePortraitIco = GetComponent<Image>();
            sliderHP = transform.Find("sliderHP")?.GetComponent<Slider>();
            sliderMP = transform.Find("sliderMP")?.GetComponent<Slider>();
            textHP = transform.Find("sliderHP/Text")?.GetComponent<TextMeshProUGUI>();
            textMP = transform.Find("sliderMP/Text")?.GetComponent<TextMeshProUGUI>();

            if (sliderHP) sliderHP.maxValue = maxHp;
            if (sliderMP) sliderMP.maxValue = maxMp;

            if (cc)
            {
                status_bar.cc = cc;
                status_bar?.UpdateStatuses();
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
                BattlePortraitIco.sprite = cc.characterRes.battlePortrait;
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