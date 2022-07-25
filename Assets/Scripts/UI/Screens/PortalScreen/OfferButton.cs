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
            private TextMeshProUGUI title;
            private GameObject selectedButton;
            private TextMeshProUGUI selectedButtonTitle;

            public event Action<OfferButton> selectOffer;

            private BaseContent content;
            public Transform contentPos { get; set; }

            public int gachaId { get; set; }

            public AdminBRO.GachItem gachaData => GameData.gacha.GetGachaById(gachaId);

            private void Awake()
            {
                var canvas = transform.Find("Canvas");

                button = canvas.Find("Button").GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);
                title = button.transform.Find("Title").GetComponent<TextMeshProUGUI>();
                
                background = button.gameObject.GetComponent<Image>();
                selectedButton = canvas.Find("Selected").gameObject;
                selectedButtonTitle = selectedButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();
                
                Deselect();
            }

            private void Start()
            {
                Customize();
            }

            private void Customize()
            {
                content = gachaData.type switch
                {
                    AdminBRO.GachItem.Type_TargetByCount => ContentByCount.GetInstance(contentPos),
                    AdminBRO.GachItem.Type_TargetByTier => ContentByTier.GetInstance(contentPos),
                    _ => null
                };

                if (content != null)
                {
                    content.gachaId = gachaId;
                }
            }
            
            protected virtual void ButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                selectOffer?.Invoke(this);
            }

            public virtual void Select()
            {
               selectedButton.SetActive(true);
               content.gameObject.SetActive(true);
            }

            public virtual void Deselect()
            {
                selectedButton.SetActive(false);
                content.gameObject.SetActive(false);
            }

            public static OfferButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<OfferButton>(
                    "Prefabs/UI/Screens/PortalScreen/OfferButton", parent);
            }
        }
    }
}