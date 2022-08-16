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
        private Button portalButton;
        private TextMeshProUGUI portalButtonText;
        private Button forgeButton;
        private TextMeshProUGUI forgeButtonText;
        private Button marketButton;
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
            portalButtonText = portalButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            
            forgeButton = canvas.Find("ForgeButton").GetComponent<Button>();
            forgeButton.onClick.AddListener(ForgeButtonClick);
            forgeButtonText = forgeButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            
            marketButton = canvas.Find("MarketButton").GetComponent<Button>();
            marketButton.onClick.AddListener(MarketButtonClick);
            marketButtonText = marketButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            
            contentPos = canvas.Find("ContentPos");
        }

        public override async Task BeforeShowMakeAsync()
        {
            Customize();

            await Task.CompletedTask;
        }

        private void Customize()
        {
            portalButtonText.text = inputData?.girlKey switch
            {
                AdminBRO.MatriarchItem.Key_Ulvi => "Go to portal to summon\nmissing Ulvi`s shards",
                AdminBRO.MatriarchItem.Key_Adriel => "Go to portal to summon\nmissing Adriel`s shards",
                AdminBRO.MatriarchItem.Key_Ingie => "Go to portal to summon\nmissing Ingie`s shards",
                AdminBRO.MatriarchItem.Key_Faye => "Go to portal to summon\nmissing Faye`s shards",
                AdminBRO.MatriarchItem.Key_Lili => "Go to portal to summon\nmissing Lili`s shards",
                _ => null
            };
            
            forgeButtonText.text = inputData?.girlKey switch
            {
                AdminBRO.MatriarchItem.Key_Ulvi => "Go to forge to merge\nmissing Ulvi`s shards",
                AdminBRO.MatriarchItem.Key_Adriel => "Go to forge to merge\nmissing Adriel`s shards",
                AdminBRO.MatriarchItem.Key_Ingie => "Go to forge to merge\nmissing Ingie`s shards",
                AdminBRO.MatriarchItem.Key_Faye => "Go to forge to merge\nmissing Faye`s shards",
                AdminBRO.MatriarchItem.Key_Lili => "Go to forge to merge\nmissing Lili`s shards",
                _ => null
            };
            
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
        
        private void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            if (inputData == null)
            {
                UIManager.ShowScreen<MemoryListScreen>();
            }
            else
            {
                UIManager.MakeScreen<MemoryListScreen>().
                    SetData(inputData.prevScreenInData as MemoryListScreenInData)
                    .RunShowScreenProcess();
            }
        }

        private void PortalButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakeScreen<PortalScreen>().
                SetData(new PortalScreenInData
            {
                activeButtonId = PortalScreen.TabShards
            }).RunShowScreenProcess();
        }

        private void ForgeButtonClick()
        {
            UIManager.MakeScreen<ForgeScreen>().
                SetData(new ForgeScreenInData
            {
                activeTabId = ForgeScreen.TabShard
            }).RunShowScreenProcess();
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