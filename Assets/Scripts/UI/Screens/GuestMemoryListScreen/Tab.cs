using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSGuestMemoryListScreen
    {
        public class Tab : MonoBehaviour
        {
            private Image tabArt;
            private GameObject markerClosed;
            private GameObject markerOpen;
            private Button button;
            private TabContent tabContent;

            public event Action<Tab> OnClick;
            public Transform tabContentPos { get; set; }
            public int? memoryId { get; set; }
            public AdminBRO.MemoryItem memoryData => GameData.matriarchs.GetMemoryById(memoryId);
            
            private void Awake()
            {
                var canvas = transform.Find("Canvas");
                tabArt = canvas.Find("TabArt").GetComponent<Image>();
                markerClosed = canvas.Find("MarkerClosed").gameObject;
                markerOpen = canvas.Find("MarkerOpen").gameObject;
                button = canvas.Find("Button").GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);
            }

            public void Customize()
            {
                if (memoryData != null)
                {
                    tabContent = TabContent.GetInstance(tabContentPos);
                    tabContent.memoryId = memoryId;
                    
                    var art = memoryData.isOpen
                        ? ResourceManager.LoadSprite(memoryData.sexScenePreview)
                        : ResourceManager.LoadSprite(memoryData.sexSceneClosed);
                    tabArt.sprite = art;
                    markerClosed.SetActive(!memoryData.isOpen);
                    markerOpen.SetActive(memoryData.isOpen);
                }
                
                Deselect();
            }

            private void ButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                OnClick?.Invoke(this);
            }

            public void Select()
            {
                var rectTr = GetComponent<RectTransform>();
                rectTr.sizeDelta = new Vector2(447, rectTr.sizeDelta.y);
                tabContent?.gameObject.SetActive(true);
            }

            public void Deselect()
            {
                var rectTr = GetComponent<RectTransform>();
                rectTr.sizeDelta = new Vector2(484, rectTr.sizeDelta.y);
                tabContent?.gameObject.SetActive(false);
            }
            
            public static Tab GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<Tab>("Prefabs/UI/Screens/GuestMemoryListScreen/Tab",
                    parent);
            }
        }
    }
}
