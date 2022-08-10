using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Threading.Tasks;

namespace Overlewd
{
    public class PortalScreen : BaseFullScreenParent<PortalScreenInData>
    {
        public const int TabBattleGirls = 0;
        public const int TabBattleGirlsEquip = 1;
        public const int TabOverlordEquip = 2;
        public const int TabShards = 3;
        public const int TabsCount = 4;

        private int activeTabId;

        private string[] tabsNames = {"BattleGirls", "BattleGirlsEquip", "OverlordEquip", "Shards"};

        private int[] tabsIds = {TabBattleGirls, TabBattleGirlsEquip, TabOverlordEquip, TabShards};
        private Button[] tabs = new Button[TabsCount];
        private GameObject[] pressedTabs = new GameObject[TabsCount];
        private Transform[] contents = new Transform[TabsCount];

        private NSPortalScreen.OfferButton selectedOffer;

        private List<NSPortalScreen.OfferButton> battleGirlsOffers = new List<NSPortalScreen.OfferButton>();
        private List<NSPortalScreen.OfferButton> battleGirlsEquipOffers = new List<NSPortalScreen.OfferButton>();
        private List<NSPortalScreen.OfferButton> overlordOffers = new List<NSPortalScreen.OfferButton>();
        private List<NSPortalScreen.OfferButton> shardsOffers = new List<NSPortalScreen.OfferButton>();

        private Button backButton;
        private Transform tabArea;
        private Transform currencyBack;

        void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/PortalScreen/PortalScreen", transform);

            var canvas = screenInst.transform.Find("Canvas");
            tabArea = canvas.Find("TabArea");

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);

            foreach (var i in tabsIds)
            {
                tabs[i] = tabArea.Find(tabsNames[i]).Find("Button").GetComponent<Button>();
                pressedTabs[i] = tabs[i].transform.Find("IconBack").Find("IconBackSelected").gameObject;
                tabs[i].onClick.AddListener(() => ButtonClick(i));

                contents[i] = tabArea.Find(tabsNames[i]).Find("TabOpened").Find("ScrollView").Find("Viewport")
                    .Find("Content");

            }

            currencyBack = canvas.Find("CurrencyBack");
        }

        public override async Task BeforeShowMakeAsync()
        {
            Customize();
            activeTabId = inputData?.activeButtonId ?? TabBattleGirls;
            ButtonClick(activeTabId);

            await Task.CompletedTask;
        }

        public override async Task AfterShowAsync()
        {
            switch (GameData.ftue.stats.lastEndedState)
            {
                case (_, _):
                    switch (GameData.ftue.activeChapter.key)
                    {
                        case "chapter1":
                            SoundManager.PlayOneShot(FMODEventPath.VO_Ulvi_Reactions_portal);
                            break;
                        case "chapter2":
                            SoundManager.PlayOneShot(FMODEventPath.VO_Adriel_Reactions_portal);
                            break;
                        case "chapter3":
                            SoundManager.PlayOneShot(FMODEventPath.VO_Ingie_Reactions_portal);
                            break;
                    }
                    break;
            }

            await Task.CompletedTask;
        }

        private void Customize()
        {
            foreach (var gacha in GameData.gacha.items)
            {
                NSPortalScreen.OfferButton offerButton = null;
                
                switch (gacha.tabType)
                {
                    case AdminBRO.GachaItem.TabType_Matriachs:
                        offerButton = NSPortalScreen.OfferButton.GetInstance(contents[TabBattleGirls]);
                        battleGirlsOffers.Add(offerButton);
                        break;
                    case AdminBRO.GachaItem.TabType_OverlordEquipment:
                        offerButton = NSPortalScreen.OfferButton.GetInstance(contents[TabOverlordEquip]);
                        overlordOffers.Add(offerButton);
                        break;
                    case AdminBRO.GachaItem.TabType_Shards:
                        offerButton = NSPortalScreen.OfferButton.GetInstance(contents[TabShards]);
                        shardsOffers.Add(offerButton);
                        break;
                    case AdminBRO.GachaItem.TabType_CharactersEquipment:
                        offerButton = NSPortalScreen.OfferButton.GetInstance(contents[TabBattleGirlsEquip]);
                        battleGirlsEquipOffers.Add(offerButton);
                        break;
                }
                
                if (offerButton != null)
                { 
                    offerButton.gachaId = gacha.id;
                    offerButton.contentPos = transform;
                    offerButton.selectOffer += SelectOffer;
                }
            }

            foreach (var i in tabsIds)
            {
                pressedTabs[i].gameObject.SetActive(false);
                contents[i].gameObject.SetActive(false);
            }
            
            UITools.FillWallet(currencyBack);
        }

        private void SelectOffer(NSPortalScreen.OfferButton offerButton)
        {
            selectedOffer?.Deselect();
            selectedOffer = offerButton;
            selectedOffer?.Select();
        }

        private void ButtonClick(int buttonId)
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            LeaveTab(activeTabId);
            EnterTab(buttonId);
        }

        private void OpenTab(int tabId)
        {
            var tab = tabArea.Find(tabsNames[tabId]);
            var openedTab = tab.Find("TabOpened").gameObject;
            
            openedTab.SetActive(true);
            tab.GetComponent<RectTransform>().sizeDelta =  new Vector2(523, 530);
            
            MoveTabs();
        }

        private void CloseTab(int tabId)
        {
            var tab = tabArea.Find(tabsNames[tabId]);
            var openedTab =  tab.Find("TabOpened").gameObject;
            
            openedTab.SetActive(false);
            tab.GetComponent<RectTransform>().sizeDelta = new Vector2(523, 102);
        }

        private void MoveTabs()
        {
            var prevTabId = 0;
            for (int i = 1; i < tabsIds.Length; i++)
            {
                var prevTabRectTr = tabArea.Find(tabsNames[prevTabId]).GetComponent<RectTransform>();
                var tabRectTr = tabArea.Find(tabsNames[i]).GetComponent<RectTransform>();
                var prevTabPos = prevTabRectTr.anchoredPosition;
                tabRectTr.anchoredPosition = new Vector2(0, prevTabPos.y - (20 + prevTabRectTr.rect.height));
                prevTabId++;
            }
        }
        
        private void EnterTab(int buttonId)
        {
            activeTabId = buttonId;
            pressedTabs[buttonId].SetActive(true);

            switch (buttonId)
            {
                case TabBattleGirls:
                    OpenTab(buttonId);
                    if (battleGirlsOffers.Any())
                        SelectOffer(battleGirlsOffers.First());
                    break;
                case TabBattleGirlsEquip:
                    OpenTab(buttonId);
                    if (battleGirlsEquipOffers.Any())
                        SelectOffer(battleGirlsEquipOffers.First());
                    break;
                case TabOverlordEquip:
                    OpenTab(buttonId);
                    if (overlordOffers.Any())
                        SelectOffer(overlordOffers.First());
                    break;
                case TabShards:
                    OpenTab(buttonId);
                    if (shardsOffers.Any())
                        SelectOffer(shardsOffers.First());
                    break;
            }
        }

        private void LeaveTab(int buttonId)
        {
            pressedTabs[buttonId].SetActive(false);
            CloseTab(buttonId);
        }
        
        private void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<CastleScreen>();
        }
    }

    public class PortalScreenInData : BaseFullScreenInData
    {
        public int? activeButtonId;
    }
}