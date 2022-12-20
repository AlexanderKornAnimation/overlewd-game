using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class MemoryScreen : BaseFullScreenParent<MemoryScreenInData>
    {
        private Button marketButton;
        private Button portalButton;
        private TextMeshProUGUI marketButtonText;
        private Button backButton;

        private Image basicShard;
        private TextMeshProUGUI basicShardAmount;
        private Image advancedShard;
        private TextMeshProUGUI advancedShardsAmount;
        private Image epicShard;
        private TextMeshProUGUI epicShardsAmount;
        private Image heroicShard;
        private TextMeshProUGUI heroicShardsAmount;

        private TextMeshProUGUI bagTitle;
        private Transform contentPos;
        private NSMemoryScreen.BaseContent girlContent;
        
        private void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/MemoryScreen/MemoryScreen", transform);

            var canvas = screenInst.transform.Find("Canvas");
            var bag = canvas.Find("Bag");

            bagTitle = bag.Find("TitleBack").Find("Title").GetComponent<TextMeshProUGUI>();
            basicShard = bag.Find("BasicShard").GetComponent<Image>();
            basicShardAmount = basicShard.transform.Find("Count").GetComponent<TextMeshProUGUI>();
            
            advancedShard = bag.Find("AdvancedShard").GetComponent<Image>();
            advancedShardsAmount = advancedShard.transform.Find("Count").GetComponent<TextMeshProUGUI>();
            
            epicShard = bag.Find("EpicShard").GetComponent<Image>();
            epicShardsAmount = epicShard.transform.Find("Count").GetComponent<TextMeshProUGUI>();
            
            heroicShard = bag.Find("HeroicShard").GetComponent<Image>();
            heroicShardsAmount = heroicShard.transform.Find("Count").GetComponent<TextMeshProUGUI>();
            
            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);
            
            portalButton = canvas.Find("PortalButton").GetComponent<Button>();
            portalButton.onClick.AddListener(PortalButtonClick);
            
            marketButton = canvas.Find("MarketButton").GetComponent<Button>();
            marketButton.onClick.AddListener(MarketButtonClick);
            marketButtonText = marketButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            
            contentPos = canvas.Find("ContentPos");
        }

        public override async Task BeforeShowMakeAsync()
        {
            Customize();

            switch (GameData.ftue.stats.lastEndedStageData?.lerningKey)
            {
                case (FTUE.CHAPTER_2, FTUE.DIALOGUE_3):
                    UITools.DisableButton(backButton, !UIManager.currentState.prevState.ScreenTypeIs<SummoningScreen>());
                    break;
            }

            await Task.CompletedTask;
        }

        private void Customize()
        {
            marketButtonText.text = inputData?.girlKey switch
            {
                AdminBRO.MatriarchItem.Key_Ulvi => "Buy Ulvi`s Heirloom\nto get Ulvi`s shards",
                AdminBRO.MatriarchItem.Key_Adriel => "Buy Adriel`s Heirloom\nto get Adriel`s shards",
                AdminBRO.MatriarchItem.Key_Ingie => "Buy Ingie`s Heirloom\nto get Ingie`s shards",
                AdminBRO.MatriarchItem.Key_Faye => "Buy Faye`s Heirloom\nto get Faye`s shards",
                AdminBRO.MatriarchItem.Key_Lili => "Buy Lili`s Heirloom\nto get Lili`s shards",
                _ => null
            };

            bagTitle.text = inputData?.girlKey switch
            {
                AdminBRO.MatriarchItem.Key_Ulvi => "Ulvi`s shards\nin your bag",
                AdminBRO.MatriarchItem.Key_Adriel => "Adriel`s shards\nin your bag",
                AdminBRO.MatriarchItem.Key_Ingie => "Ingie`s shards\nin your bag",
                AdminBRO.MatriarchItem.Key_Faye => "Faye`s shards\nin your bag",
                AdminBRO.MatriarchItem.Key_Lili => "Lili`s shards\nin your bag",
                _ => null
            };
            
            girlContent = inputData?.girlKey switch
            {
                AdminBRO.MatriarchItem.Key_Ulvi => NSMemoryScreen.UlviContent.GetInstance(contentPos),
                AdminBRO.MatriarchItem.Key_Adriel => NSMemoryScreen.AdrielContent.GetInstance(contentPos),
                AdminBRO.MatriarchItem.Key_Ingie => NSMemoryScreen.IngieContent.GetInstance(contentPos),
                AdminBRO.MatriarchItem.Key_Faye => NSMemoryScreen.FayeContent.GetInstance(contentPos),
                AdminBRO.MatriarchItem.Key_Lili => NSMemoryScreen.UlviContent.GetInstance(contentPos),
                _ => null
            };
        }

        public override async Task AfterShowAsync()
        {
            switch (GameData.ftue.stats.lastEndedStageData?.lerningKey)
            {
                case (FTUE.CHAPTER_2, FTUE.DIALOGUE_3):
                    GameData.ftue.chapter2.ShowNotifByKey("ch2gachatutor4");
                    await UIManager.WaitHideNotifications();
                    break;
            }

            await Task.CompletedTask;
        }

        public override void OnUIEvent(UIEvent eventData)
        {
            switch (eventData.id)
            {
                case UIEventId.ChangeScreenComplete:
                    break;
            }
        }

        private void PortalButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            switch (GameData.ftue.stats.lastEndedStageData?.lerningKey)
            {
                case (FTUE.CHAPTER_2, FTUE.DIALOGUE_3):
                    UIManager.MakeScreen<PortalScreen>().
                        SetData(new PortalScreenInData
                        {
                            activeButtonId = PortalScreen.TabShards,
                        }).DoShow();
                    break;
                default:
                    UIManager.ShowScreen<PortalScreen>();
                    break;
            }
            
        }
        
        private void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            switch (GameData.ftue.stats.lastEndedStageData?.lerningKey)
            {
                case (FTUE.CHAPTER_2, FTUE.DIALOGUE_3):
                   UIManager.ShowScreen<MapScreen>();
                    break;
                default:
                    UIManager.ToPrevScreen();
                    break;
            }
        }

        private void MarketButtonClick()
        {
            UIManager.ShowOverlay<MarketOverlay>();
        }
    }

    public class MemoryScreenInData : BaseFullScreenInData
    {
        public string girlKey;
    }
}