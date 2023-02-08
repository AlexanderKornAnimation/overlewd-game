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
        private Image background;
        private Button sexSceneButton;

        private TextMeshProUGUI basicShardAmount;
        private TextMeshProUGUI advancedShardsAmount;
        private TextMeshProUGUI epicShardsAmount;
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

            background = canvas.Find("Background").GetComponent<Image>();
            sexSceneButton = background.GetComponent<Button>();
            sexSceneButton.onClick.AddListener(SexSceneButtonClick);
            
            bagTitle = bag.Find("TitleBack").Find("Title").GetComponent<TextMeshProUGUI>();
            basicShardAmount = bag.transform.Find("BasicShard/Count").GetComponent<TextMeshProUGUI>();
            advancedShardsAmount = bag.transform.Find("AdvancedShard/Count").GetComponent<TextMeshProUGUI>();
            epicShardsAmount = bag.transform.Find("EpicShard/Count").GetComponent<TextMeshProUGUI>();
            heroicShardsAmount = bag.transform.Find("HeroicShard/Count").GetComponent<TextMeshProUGUI>();
            
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
                    UITools.DisableButton(portalButton, UIManager.currentState.prevState.ScreenTypeIs<SummoningScreen>());
                    break;
            }

            await Task.CompletedTask;
        }

        private void Customize()
        {
            RefreshBag();
            CheckSexSceneButtonState();

            marketButtonText.text = $"Buy {inputData?.girlKey + "'s"} Heirloom\nto get more shards";
            bagTitle.text = $"{inputData?.girlKey + "'s"} shards\nin your bag";
            background.sprite = ResourceManager.LoadSprite(inputData?.memoryData?.memoryBackArt);
            
            girlContent = inputData?.girlKey switch
            {
                AdminBRO.MatriarchItem.Key_Ulvi => NSMemoryScreen.UlviContent.GetInstance(contentPos),
                AdminBRO.MatriarchItem.Key_Adriel => NSMemoryScreen.AdrielContent.GetInstance(contentPos),
                AdminBRO.MatriarchItem.Key_Inge => NSMemoryScreen.IngieContent.GetInstance(contentPos),
                AdminBRO.MatriarchItem.Key_Faye => NSMemoryScreen.FayeContent.GetInstance(contentPos),
                AdminBRO.MatriarchItem.Key_Lili => NSMemoryScreen.LiliContent.GetInstance(contentPos),
                _ => null
            };

            if (girlContent != null)
            {
                girlContent.memoryId = inputData?.memoryId;
            }
        }

        private void SexSceneButtonClick()
        {
            UIManager.MakeScreen<SexScreen>().
                SetData(new SexScreenInData
                {
                    dialogId = inputData?.memoryData?.sexSceneId,
                }).
                DoShow();
        }
        
        private void RefreshBag()
        {
            basicShardAmount.text = inputData?.girlData?.basicShard?.amount.ToString();
            advancedShardsAmount.text = inputData?.girlData?.advancedShard?.amount.ToString();
            epicShardsAmount.text = inputData?.girlData?.epicShard?.amount.ToString();
            heroicShardsAmount.text = inputData?.girlData?.heroicShard?.amount.ToString();
        }
        
        public override void OnGameDataEvent(GameDataEvent eventData)
        {
            switch (eventData.id)
            {
                case GameDataEventId.PieceOfMemoryBuy:
                    RefreshBag();
                    CheckSexSceneButtonState();
                    break;
            }
        }

        private void CheckSexSceneButtonState()
        {
            sexSceneButton.interactable = inputData?.memoryData?.isOpen ?? false;
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
        public AdminBRO.MatriarchItem girlData => GameData.matriarchs.GetMatriarchByKey(girlKey);
        public int? memoryId;
        public AdminBRO.MemoryItem memoryData => GameData.matriarchs.GetMemoryById(memoryId);
    }
}