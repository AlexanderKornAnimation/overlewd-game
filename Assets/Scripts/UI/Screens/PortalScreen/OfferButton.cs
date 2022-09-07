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
            private TextMeshProUGUI marker;
            private GameObject selectedButton;
            private Image selectedPic;
            private TextMeshProUGUI selectedButtonTitle;
            private TextMeshProUGUI selectedMarker;

            public event Action<OfferButton> selectOffer;

            private BaseContent content;
            public Transform contentPos { get; set; }

            public int gachaId { get; set; }

            public AdminBRO.GachaItem gachaData => GameData.gacha.GetGachaById(gachaId);

            void Awake()
            {
                var canvas = transform.Find("Canvas");

                button = canvas.Find("Button").GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);
                buttonPic = button.GetComponent<Image>();
                title = button.transform.Find("Title").GetComponent<TextMeshProUGUI>();
                marker = button.transform.Find("Marker").GetComponent<TextMeshProUGUI>();

                background = button.gameObject.GetComponent<Image>();
                selectedButton = canvas.Find("Selected").gameObject;
                selectedPic = selectedButton.GetComponent<Image>();
                selectedButtonTitle = selectedButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();
                selectedMarker = selectedButton.transform.Find("Marker").GetComponent<TextMeshProUGUI>();
            }

            void Start()
            {
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

                Customize();
                Deselect();
            }

            private void Customize()
            {
                var _gachaData = gachaData;
                selectedPic.sprite = ResourceManager.LoadSprite(_gachaData.tabImageOn);
                selectedButtonTitle.text = _gachaData.tabTitle;
                buttonPic.sprite = ResourceManager.LoadSprite(_gachaData.tabImageOff);
                title.text = _gachaData.tabTitle;
                selectedMarker.text = marker.text =
                    _gachaData.isTempOffer ? TMPSprite.NotificationTimeLimit : null;
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