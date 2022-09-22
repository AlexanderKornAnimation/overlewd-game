using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

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
        private Slider whiteSlider;
        private Image icon;
        private Image BattlePortraitIco;

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

        public void InitUI(CharController charC)
        {
            cc = charC;
            //BattleBackgroundIco = GetComponent<Image>();
            //icon = GetComponent<Image>();
            BattlePortraitIco = transform.Find("Portrait")?.GetComponent<Image>();
            sliderHP = transform.Find("sliderHP")?.GetComponent<Slider>();
            sliderMP = transform.Find("sliderMP")?.GetComponent<Slider>();
            whiteSlider = sliderHP.transform.Find("WhiteSlider")?.GetComponent<Slider>();
            textHP = transform.Find("sliderHP/Text")?.GetComponent<TextMeshProUGUI>();
            textMP = transform.Find("sliderMP/Text")?.GetComponent<TextMeshProUGUI>();

            if (sliderHP) sliderHP.maxValue = maxHp;
            if (sliderMP) sliderMP.maxValue = maxMp;

            if (cc && status_bar)
            {
                status_bar.cc = cc;
                status_bar.UpdateStatuses();
            }
            if (cc != null && button != null)
                button.onClick.AddListener(cc.Select);

            if (bigPortrait)
            {
                BattlePortraitIco.sprite = cc.bigIcon;
                BattlePortraitIco.SetNativeSize();
                transform.SetSiblingIndex(0);
            }
            else
            {
                BattlePortraitIco.sprite = cc.icon;
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
            if (cc.bigIcon != null) BattlePortraitIco.sprite = cc.bigIcon;
            if (sliderHP) sliderHP.maxValue = maxHp;
            sliderMP?.gameObject.SetActive(cc.isOverlord);
            status_bar.cc = cc;
            status_bar?.UpdateStatuses();
            UpdateUI();
        }

        public void UpdateUI()
        {
            textHP.text = $"{hp}/{maxHp}";
            sliderHP.DOValue(hp, 0.3f).SetEase(Ease.OutQuint);
            if (textMP) textMP.text = $"{mp}/{maxMp}";
            if (sliderMP) sliderMP.value = mp;
            status_bar?.UpdateStatuses();
            if (whiteSlider) StartCoroutine(HPChangePause());
        }
        IEnumerator HPChangePause()
        {
            whiteSlider.fillRect.GetComponent<Image>().enabled = true;
            whiteSlider.maxValue = maxHp;
            yield return new WaitForSeconds(1.1f);
            whiteSlider.DOValue(hp, 0.75f).SetEase(Ease.OutQuint);
            yield return new WaitForSeconds(0.75f);
            whiteSlider.fillRect.GetComponent<Image>().enabled = false;
        }
    }
}