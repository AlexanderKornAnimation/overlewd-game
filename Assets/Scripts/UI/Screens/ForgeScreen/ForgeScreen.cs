using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class ForgeScreen : BaseFullScreenParent<ForgeScreenInData>
    {
        private Transform infoBlock;
        private Transform consumeCell;
        private TextMeshProUGUI consumeCount;
        private GameObject consumeItem;
        private Image consumeRarityBack;
        private Image consumeShard;
        private Image consumeShardGirl;
        private Image consumeEquip;
        
        private Transform targetCell;
        private GameObject targetItem;
        private Image targetRarityBack;
        private Image targetShard;
        private Image targetShardGirl;
        private Image targetEquip;
        private TextMeshProUGUI targetCount;
        private Button targetIncreaseButton;
        private Button targetDecreaseButton;
        
        private Transform buttons;
        private Button marketButton;
        private TextMeshProUGUI marketButtonText;
        private Button portalButton;
        private TextMeshProUGUI portalButtonText;
        private Button mergeButton;
        private Image[] mergePrice = new Image[2];
        private TextMeshProUGUI[] mergePriceAmount = new TextMeshProUGUI[2];

        private GameObject shardsContentGO;
        private GameObject battleGirlsEquipContentGO;
        private GameObject overlordContentGO;
        
        private Transform currencyBack;
        private Transform tabArea;
        private Button backButton;

        public const int TabShard = 0;
        public const int TabBattleGirlsEquip = 1;
        public const int TabOverlordEquip = 2;
        public const int SubTabExchangeShard = 3;
        public const int SubTabMergeShard = 4;
        public const int TabsCount = 5;

        private int[] tabIds = {TabShard, TabBattleGirlsEquip, TabOverlordEquip};
        private Button[] tabs = new Button[TabsCount];
        private GameObject[] pressedTabs = new GameObject[TabsCount];
        private string[] tabNames = {"Shards", "BattleGirlsEquip", "OverlordEquip"};
        
        private int activeTabId;
        private int activeSubTabId = SubTabExchangeShard;

        private NSForgeScreen.ShardContent shardsContent;
        private NSForgeScreen.BattleGirlsEquipContent battleGirlsEquipEquipContent;
        private NSForgeScreen.OverlordEquipContent overlordEquipEquipContent;

        private void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/ForgeScreen/ForgeScreen", transform);

            var canvas = screenInst.transform.Find("Canvas");
            tabArea = canvas.Find("TabArea");

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);
            
            infoBlock = canvas.Find("InfoBlock");
            consumeCell = infoBlock.Find("ConsumeCell");
            var counter = consumeCell.Find("Counter");
            consumeCount = counter.Find("Count").GetComponent<TextMeshProUGUI>();
            consumeItem = consumeCell.Find("Item").gameObject;
            consumeRarityBack = consumeItem.transform.Find("RarityBackground").GetComponent<Image>();
            consumeShard = consumeItem.transform.Find("Shard").GetComponent<Image>();
            consumeShardGirl = consumeShard.transform.Find("Girl").GetComponent<Image>();
            consumeEquip = consumeItem.transform.Find("Equip").GetComponent<Image>();

            targetCell = infoBlock.Find("TargetCell");
            counter = targetCell.Find("Counter");
            targetCount = counter.Find("Count").GetComponent<TextMeshProUGUI>();
            
            targetDecreaseButton = counter.Find("DecreaseButton").GetComponent<Button>();
            targetDecreaseButton.onClick.AddListener(DecreaseButtonClick);
            targetIncreaseButton = counter.Find("IncreaseButton").GetComponent<Button>();
            targetIncreaseButton.onClick.AddListener(IncreaseButtonClick);
            
            targetItem = targetCell.Find("Item").gameObject;
            targetRarityBack = targetItem.transform.Find("RarityBackground").GetComponent<Image>();
            targetShard = targetItem.transform.Find("Shard").GetComponent<Image>();
            targetShardGirl = targetShard.transform.Find("Girl").GetComponent<Image>();
            targetEquip = targetItem.transform.Find("Equip").GetComponent<Image>();

            buttons = canvas.Find("Buttons");
            marketButton = buttons.Find("MarketButton").GetComponent<Button>();
            marketButton.onClick.AddListener(MarketButtonClick);
            marketButtonText = marketButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            
            portalButton = buttons.Find("PortalButton").GetComponent<Button>();
            portalButton.onClick.AddListener(PortalButtonClick);
            portalButtonText = portalButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            
            mergeButton = buttons.Find("MergeButton").GetComponent<Button>();
            mergeButton.onClick.AddListener(MergeButtonClick);

            shardsContentGO = canvas.Find("ShardsContent").gameObject;
            battleGirlsEquipContentGO = canvas.Find("BattleGirlsEquipContent").gameObject;
            overlordContentGO = canvas.Find("OverlordContent").gameObject;

            foreach (var i in tabIds)
            {
                tabs[i] = tabArea.Find(tabNames[i]).Find("Button").GetComponent<Button>();
                tabs[i].onClick.AddListener(() => TabClick(i));
                
                pressedTabs[i] = tabs[i].transform.Find("IconBack").Find("IconBackSelected").gameObject;
                pressedTabs[i].SetActive(false);
            }

            var tabOpened = tabArea.Find("Shards").Find("TabOpened");
            tabs[SubTabExchangeShard] = tabOpened.Find("ExchangeTab").GetComponent<Button>();
            tabs[SubTabExchangeShard].onClick.AddListener(() => EnterSubTab(SubTabExchangeShard));
            pressedTabs[SubTabExchangeShard] = tabOpened.Find("ExchangeTabSelected").gameObject;
            pressedTabs[SubTabExchangeShard].SetActive(false);
                    
            tabs[SubTabMergeShard] = tabOpened.Find("MergeTab").GetComponent<Button>();
            tabs[SubTabMergeShard].onClick.AddListener(() => EnterSubTab(SubTabMergeShard));
            pressedTabs[SubTabMergeShard] = tabOpened.Find("MergeTabSelected").gameObject;
            pressedTabs[SubTabMergeShard].SetActive(false);
            
            for (int i = 0; i < mergePrice.Length; i++)
            {
                mergePrice[i] = mergeButton.transform.Find($"Resource{i + 1}").GetComponent<Image>();
                mergePriceAmount[i] = mergePrice[i].transform.Find("Count").GetComponent<TextMeshProUGUI>();
            }
            
            currencyBack = canvas.Find("CurrencyBack");
            UITools.FillWallet(currencyBack);
        }

        public override async Task BeforeShowMakeAsync()
        {
            Customize();
            await Task.CompletedTask;
        }
        
        private void Customize()
        {
            shardsContent = shardsContentGO.AddComponent<NSForgeScreen.ShardContent>();
            battleGirlsEquipEquipContent = battleGirlsEquipContentGO.AddComponent<NSForgeScreen.BattleGirlsEquipContent>();
            overlordEquipEquipContent = overlordContentGO.AddComponent<NSForgeScreen.OverlordEquipContent>();

            activeTabId = inputData.activeTabId ?? TabShard;
            targetItem.SetActive(false);
            consumeItem.SetActive(false);
            EnterTab(activeTabId);
        }

        private void EnterSubTab(int tabId)
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            LeaveSubTab(activeSubTabId);
            pressedTabs[tabId].SetActive(true);
            activeSubTabId = tabId;
            shardsContent.isMergeMode = activeSubTabId == SubTabMergeShard;
            shardsContent.OnModeChanged();
        }

        private void LeaveSubTab(int tabId)
        {
            pressedTabs[tabId].SetActive(false);
        }
        
        private void TabClick(int tabId)
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            LeaveTab(activeTabId);
            EnterTab(tabId);
        }

        private void EnterTab(int tabId)
        {
            var content = tabId switch
            {
                TabShard => shardsContentGO,
                TabBattleGirlsEquip => battleGirlsEquipContentGO,
                TabOverlordEquip => overlordContentGO,
                _ => null
            };
            
            activeTabId = tabId;
            pressedTabs[tabId].SetActive(true);
            content?.SetActive(true);

            switch (tabId)
            {
                case TabOverlordEquip:
                    MoveUpButtonsAndInfoBlockPos();
                    marketButtonText.text = "Go to the Market to\nbuy sets with equipment";
                    portalButtonText.text = "Go to the portal to\nsummon more equipment";
                    break;
                case TabBattleGirlsEquip:
                    MoveUpButtonsAndInfoBlockPos();
                    marketButtonText.text = "Go to the Market\nto buy sets with weapon";
                    portalButtonText.text = "Go to the portal\nto summon more weapon";
                    break;
                case TabShard:
                    SetButtonsAndInfoBlockDefaultPos();
                    marketButtonText.text = "Go to the Market\nto buy sets with shards";
                    portalButtonText.text = "Go to the portal\nto summon more shards";
                    OpenShardTab();
                    break;
            }
        }

        private void LeaveTab(int tabId)
        {
            var content = tabId switch
            {
                TabShard => shardsContentGO,
                TabBattleGirlsEquip => battleGirlsEquipContentGO,
                TabOverlordEquip => overlordContentGO,
                _ => null
            };
            
            pressedTabs[tabId].SetActive(false);

            if (tabId == TabShard)
            {
                CloseShardTab();
            }

            content?.SetActive(false);
        }
        
        private void OpenShardTab()
        {
            var tabShard = tabArea.Find("Shards");
            var openedTab = tabShard.transform.Find("TabOpened");
            
            openedTab.gameObject.SetActive(true);
            tabArea.Find("BattleGirlsEquip").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 451);
            tabArea.Find("OverlordEquip").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 329);
            EnterSubTab(activeSubTabId);
        }

        private void CloseShardTab()
        {
            var tabShard = tabArea.Find("Shards");
            var openedTab = tabShard.Find("TabOpened").gameObject;
            
            openedTab.SetActive(false);
            tabArea.Find("BattleGirlsEquip").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 672);
            tabArea.Find("OverlordEquip").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 550);
        }
        
        private void SetButtonsAndInfoBlockDefaultPos()
        {
            var buttonsRect = buttons.GetComponent<RectTransform>();
            var buttonsPos = buttonsRect.anchoredPosition;
            buttonsRect.anchoredPosition = new Vector2(buttonsPos.x, 367);
            infoBlock.GetComponent<RectTransform>().anchoredPosition = new Vector2(247, -137);
        }
        
        private void MoveUpButtonsAndInfoBlockPos()
        {
            var buttonsRect = buttons.GetComponent<RectTransform>();
            var buttonsPos = buttonsRect.anchoredPosition;
            buttonsRect.anchoredPosition = new Vector2(buttonsPos.x, 438);
            infoBlock.GetComponent<RectTransform>().anchoredPosition = new Vector2(247, -112);
        }

        private void MergeButtonClick()
        {
            
        }
        
        private void PortalButtonClick()
        {
            var portalScreenTab = activeTabId switch
            {
                TabShard => PortalScreen.TabShards,
                TabOverlordEquip => PortalScreen.TabOverlordEquip,
                TabBattleGirlsEquip => PortalScreen.TabBattleGirlsEquip,
                _ => PortalScreen.TabBattleGirls
            };
            
            UIManager.MakeScreen<PortalScreen>().
                SetData(new PortalScreenInData
            {
                activeButtonId = portalScreenTab
            }).RunShowScreenProcess();
        }
        
        private void MarketButtonClick()
        {
            UIManager.ShowOverlay<MarketOverlay>();
        }
        
        private void DecreaseButtonClick()
        {
            
        }
        
        private void IncreaseButtonClick()
        {
            
        }
        
        private void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<CastleScreen>();
        }
    }

    public class ForgeScreenInData : BaseFullScreenInData
    {
        public int? activeTabId;
    }
}
