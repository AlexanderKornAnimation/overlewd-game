using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSGuestMemoryListScreen
    {
        public class TabContent : MonoBehaviour
        {
            private Image art;
            private Button sexSceneButton;
            private TextMeshProUGUI title;
            
            public int? memoryId { get; set; }
            public AdminBRO.MemoryItem memoryData => GameData.matriarchs.GetMemoryById(memoryId);

            private void Awake()
            {
                art = transform.Find("Art").GetComponent<Image>();
                sexSceneButton = transform.Find("SexSceneButton").GetComponent<Button>();
                sexSceneButton.onClick.AddListener(SexSceneButtonClick);
                title = transform.Find("TitleBack/Title").GetComponent<TextMeshProUGUI>();
            }

            private void SexSceneButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                UIManager.MakeScreen<SexScreen>().
                    SetData(new SexScreenInData
                    {
                        dialogId = memoryData?.sexSceneId,
                    }).DoShow();
            }

            private void Start()
            {
                Customize();
            }

            private void Customize()
            {
                if (memoryData != null)
                {
                    art.sprite = ResourceManager.LoadSprite(memoryData.sexScenePreview);
                    title.text = memoryData.title;
                }
            }


            public static TabContent GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<TabContent>(
                    "Prefabs/UI/Screens/GuestMemoryListScreen/TabContent", parent);
            }
        }
    }
}
