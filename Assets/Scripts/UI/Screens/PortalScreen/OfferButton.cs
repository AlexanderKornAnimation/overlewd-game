using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSPortalScreen
    {
        public class OfferButton : MonoBehaviour
        {
            private Image background;
            private Button button;
            private Image buttonPic;
            private TextMeshProUGUI title;
            private GameObject selectedButton;
            private Image selectedPic;
            private TextMeshProUGUI selectedButtonTitle;

            public event Action<OfferButton> selectOffer;

            private BaseContent content;
            public Transform contentPos { get; set; }

            public int gachaId { get; set; }

            public AdminBRO.GachaItem gachaData => GameData.gacha.GetGachaById(gachaId);

            private bool init = false;

            private void Initialize()
            {
                if (init) return;

                var canvas = transform.Find("Canvas");

                button = canvas.Find("Button").GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);
                buttonPic = button.GetComponent<Image>();
                title = button.transform.Find("Title").GetComponent<TextMeshProUGUI>();

                background = button.gameObject.GetComponent<Image>();
                selectedButton = canvas.Find("Selected").gameObject;
                selectedPic = selectedButton.GetComponent<Image>();
                selectedButtonTitle = selectedButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();

                init = true;
            }

            void Awake()
            {
                Initialize();
            }

            void Start()
            {
                Customize();
                Deselect();
            }

            private void Customize()
            {
                Initialize();

                var _gachaData = gachaData;

                content = _gachaData.type switch
                {
                    AdminBRO.GachaItem.Type_TargetByCount => ContentByCount.GetInstance(contentPos),
                    AdminBRO.GachaItem.Type_TargetByTier => ContentByTier.GetInstance(contentPos),
                    _ => null
                };
                
                if (content != null)
                {
                    content.gachaId = gachaId;
                }

                selectedPic.sprite = ResourceManager.LoadSprite(_gachaData.tabImageOn);
                selectedButtonTitle.text = _gachaData.tabTitle;
                buttonPic.sprite = ResourceManager.LoadSprite(_gachaData.tabImageOff);
                title.text = _gachaData.tabTitle;
            }
            
            protected virtual void ButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                selectOffer?.Invoke(this);
            }

            public virtual void Select()
            {
                selectedButton.SetActive(true);
                content?.gameObject.SetActive(true);
                content?.Customize();
            }

            public virtual void Deselect()
            {
                selectedButton.SetActive(false);
                content?.gameObject.SetActive(false);
            }

            public static OfferButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<OfferButton>(
                    "Prefabs/UI/Screens/PortalScreen/OfferButton", parent);
            }
        }
    }
}