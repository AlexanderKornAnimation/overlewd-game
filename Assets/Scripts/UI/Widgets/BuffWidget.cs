using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class BuffWidget : BaseWidget
    {
        private RectTransform backRect;
        private Image icon;
        private Transform iconUlvi;
        private Transform iconAdriel;
        private Transform iconIngie;
        private Transform iconLili;
        private Transform iconFaye;
        private Button button;
        private TextMeshProUGUI title;
        private TextMeshProUGUI description;
        
        void Awake()
        {
            var canvas = transform.Find("Canvas");
            backRect = canvas.Find("BackRect").GetComponent<RectTransform>();
            button = backRect.Find("Button").GetComponent<Button>();
            icon = button.transform.Find("Icon").GetComponent<Image>();
            iconUlvi = button.transform.Find("IconUlvi");
            iconAdriel = button.transform.Find("IconAdriel");
            iconIngie = button.transform.Find("IconIngie");
            iconLili = button.transform.Find("IconLili");
            iconFaye = button.transform.Find("IconFaye");
            title = button.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            description = button.transform.Find("Description").GetComponent<TextMeshProUGUI>();
            
            button.onClick.AddListener(ButtonClick);
        }

        void Start()
        {
            Customize();
        }

        private void Customize()
        {
            icon.sprite = ResourceManager.LoadSprite(GameData.matriarchs.activeBuff?.icon);
            title.text = GameData.matriarchs.activeBuff?.name;
            description.text = GameData.matriarchs.activeBuff?.description;
            iconUlvi.gameObject.SetActive(GameData.matriarchs.activeBuff?.matriarch?.isUlvi ?? false);
            iconAdriel.gameObject.SetActive(GameData.matriarchs.activeBuff?.matriarch?.isAdriel ?? false);
            iconIngie.gameObject.SetActive(GameData.matriarchs.activeBuff?.matriarch?.isIngie ?? false);
            iconLili.gameObject.SetActive(GameData.matriarchs.activeBuff?.matriarch?.isLili ?? false);
            iconFaye.gameObject.SetActive(GameData.matriarchs.activeBuff?.matriarch?.isFaye ?? false);

            UITools.DisableButton(button, GameData.progressFlags.lockBuff);
        }

        protected virtual void ButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakeScreen<HaremScreen>().
                SetData(new HaremScreenInData
                {
                    prevScreenInData = UIManager.prevScreenInData
                }).RunShowScreenProcess();
        }

        public void Show()
        {
            UITools.RightShow(backRect);
        }

        public void Hide()
        {
            UITools.RightHide(backRect);
        }

        public async Task ShowAsync()
        {
            await UITools.RightShowAsync(backRect);
        }

        public async Task HideAsync()
        {
            await UITools.RightHideAsync(backRect);
        }
        
        public static BuffWidget GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateScreenPrefab<BuffWidget>
                ("Prefabs/UI/Widgets/BuffWidget/BuffWidget", parent);
        }
    }
}

