using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Threading.Tasks;
using TMPro;

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

        private Button backButton;
        private Transform tabArea;
        private Transform walletWidgetPos;
        private WalletWidget walletWidget;
        private Image backgroundImg;

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

            walletWidgetPos = canvas.Find("WalletWidgetPos");
            backgroundImg = canvas.Find("Background").GetComponent<Image>();
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

        public override async Task BeforeShowAsync()
        {
            SoundManager.GetEventInstance(FMODEventPath.Castle_Screen_BGM_Attn);
            await Task.CompletedTask;
        }

        public override async Task BeforeHideAsync()
        {
            SoundManager.StopAll();
            await Task.CompletedTask;
        }

        private void Customize()
        {
            foreach (var gacha in GameData.gacha.items)
            {
                if (!gacha.timePeriodIsActive)
                {
                    continue;
                }

                var offerButton = gacha.tabType switch
                {
                    AdminBRO.GachaItem.TabType_Characters =>
                        NSPortalScreen.OfferButton.GetInstance(contents[TabBattleGirls]),
                    AdminBRO.GachaItem.TabType_OverlordEquipment =>
                        NSPortalScreen.OfferButton.GetInstance(contents[TabOverlordEquip]),
                    AdminBRO.GachaItem.TabType_MatriachsShards =>
                        NSPortalScreen.OfferButton.GetInstance(contents[TabShards]),
                    AdminBRO.GachaItem.TabType_CharactersEquipment =>
                        NSPortalScreen.OfferButton.GetInstance(contents[TabBattleGirlsEquip]),
                        _ => null
                };
                
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

            walletWidget = WalletWidget.GetInstance(walletWidgetPos);
        }

        private void SelectOffer(NSPortalScreen.OfferButton offerButton)
        {
            selectedOffer?.Deselect();
            selectedOffer = offerButton;
            selectedOffer?.Select();
            CustomizeByGachaData(offerButton?.gachaData);
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
            contents[buttonId].gameObject.SetActive(true);
            OpenTab(buttonId);
        }

        private void LeaveTab(int buttonId)
        {
            pressedTabs[buttonId].SetActive(false);
            contents[buttonId].gameObject.SetActive(false);
            CloseTab(buttonId);
        }
        
        private void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<CastleScreen>();
        }

        private void CustomizeByGachaData(AdminBRO.GachaItem gachaData)
        {
            backgroundImg.sprite = ResourceManager.LoadSprite(gachaData?.backgroundImage);
        }
    }

    public class PortalScreenHelper
    {
        public static void MakeSummonButton(AdminBRO.GachaItem gachaData, bool many,
            Button button, TextMeshProUGUI title)
        {
            button.gameObject.SetActive(many ? gachaData?.priceForMany?.Count > 0 :
                gachaData?.priceForOne?.Count > 0);
            if (button.gameObject.activeSelf)
            {
                var price = many ? gachaData.priceForMany : gachaData.priceForOne;

                title.text = (many ? "Summon 5 " : "Summon 1 ") + gachaData.tabType switch
                {
                    AdminBRO.GachaItem.TabType_Characters => many ? "battle girls for " : "battle girl for ",
                    AdminBRO.GachaItem.TabType_CharactersEquipment => many ? "pieces of equipment for " : "piece of equipment for ",
                    AdminBRO.GachaItem.TabType_OverlordEquipment => many ? "pieces of equipment for " : "piece of equipment for ",
                    AdminBRO.GachaItem.TabType_MatriachsShards => many ? "shards of memories for " : "shard of memories for ",
                    _ => "- for "
                } + UITools.PriceToString(price);

                var canSummon = GameData.player.CanBuy(price) && gachaData.available;
                UITools.DisableButton(button, !canSummon);
            }
        }
    }


    public class PortalScreenInData : BaseFullScreenInData
    {
        public int? activeButtonId;
    }
}