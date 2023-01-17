using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSMatriarchMemoryListScreen
    {
        public abstract class BaseMemoryBanner : MonoBehaviour
        {
            protected Image background;
            protected Image closed;
            protected Button watchButton;
            protected TextMeshProUGUI title;
            protected Transform canvas;
            
            public int? memoryId { get; set; }
            public AdminBRO.MemoryItem memoryData => GameData.matriarchs.GetMemoryById(memoryId);

            protected virtual void Awake()
            {
                canvas = transform.Find("Canvas");

                watchButton = canvas.Find("WatchButton").GetComponent<Button>();
                watchButton.onClick.AddListener(WatchButtonClick);
                background = canvas.Find("Background").GetComponent<Image>();
                closed = canvas.transform.Find("Closed").GetComponent<Image>();
                title = canvas.Find("TitleBack/Title").GetComponent<TextMeshProUGUI>();
            }

            private void Start()
            {
                Customize();
            }

            protected virtual void Customize()
            {
                if (memoryData != null)
                {
                    background.sprite = ResourceManager.LoadSprite(memoryData.sexScenePreview);
                    closed.gameObject.SetActive(!memoryData.isOpen);
                    closed.sprite = GetMatriarchClosedBackground();
                    title.text = memoryData.title;
                    watchButton.gameObject.SetActive(memoryData.isOpen);
                }
            }

            private Sprite GetMatriarchClosedBackground()
            {
                return memoryData?.matriarchData?.key switch
                {
                    AdminBRO.MatriarchItem.Key_Ulvi => ResourceManager.LoadSprite(
                        "Prefabs/UI/Screens/MatriarchMemoryListScreen/Images/LockedMemoryBannerUlvi"),
                    AdminBRO.MatriarchItem.Key_Adriel => ResourceManager.LoadSprite(
                        "Prefabs/UI/Screens/MatriarchMemoryListScreen/Images/LockedMemoryBannerAdriel"),
                    AdminBRO.MatriarchItem.Key_Ingie => ResourceManager.LoadSprite(
                        "Prefabs/UI/Screens/MatriarchMemoryListScreen/Images/LockedMemoryBannerIngie"),
                    AdminBRO.MatriarchItem.Key_Faye => ResourceManager.LoadSprite(
                        "Prefabs/UI/Screens/MatriarchMemoryListScreen/Images/LockedMemoryBannerFaye"),
                    AdminBRO.MatriarchItem.Key_Lili => ResourceManager.LoadSprite(
                        "Prefabs/UI/Screens/MatriarchMemoryListScreen/Images/LockedMemoryBannerLili"),
                    _ => null,
                };
            }
            
            private void WatchButtonClick()
            {
                UIManager.MakeScreen<SexScreen>().
                    SetData(new SexScreenInData
                    {
                        dialogId = memoryData.sexSceneId,
                    }).DoShow();
            }
        }
    }
}
