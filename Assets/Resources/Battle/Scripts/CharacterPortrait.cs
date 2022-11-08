using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class CharacterPortrait : MonoBehaviour
    {
        public CharController cc;
        public bool isBoss = false; //activate mask
        private TextMeshProUGUI textHP;
        private TextMeshProUGUI textMP;
        private Slider sliderHP;
        private Slider sliderMP;
        private Slider whiteSlider;
        private Image icon;
        private Image BattlePortraitIco;

        private StatusEffects status_bar => transform.Find("status_bar")?.GetComponent<StatusEffects>();

        private int level => cc.level;
        private string rarity => cc.rarity;
        private float hp => cc.health;
        private float maxHp => cc.healthMax;
        private float mp => cc.mana;
        private float maxMp => cc.manaMax;

        public void InitUI(CharController charC)
        {
            cc = charC;
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

            BattlePortraitIco.sprite = cc.bigIcon;
            BattlePortraitIco.SetNativeSize();
            transform.SetSiblingIndex(0);

            UpdateUI();
        }

        public void SetUI(CharController replaceCharacter = null)
        {
            if (replaceCharacter)
                cc = replaceCharacter;
            if (cc.bigIcon != null) BattlePortraitIco.sprite = cc.bigIcon;
            BattlePortraitIco.SetNativeSize();
            if (sliderHP) sliderHP.maxValue = maxHp;
            status_bar.cc = cc;
            UpdateUI();
        }

        public void UpdateUI()
        {
            //sliderMP?.gameObject.SetActive(cc.isOverlord);
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
            yield return new WaitForSeconds(0.6f);
            whiteSlider.DOValue(hp, 0.3f).SetEase(Ease.OutQuint);
            yield return new WaitForSeconds(0.6f);
            whiteSlider.fillRect.GetComponent<Image>().enabled = false;
        }
    }
}