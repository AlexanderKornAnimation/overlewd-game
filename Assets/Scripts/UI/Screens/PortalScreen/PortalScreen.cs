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
        private const int buttonBattleGirls = 0;
        private const int buttonBattleGirlsEquip = 1;
        private const int buttonOverlordEquip = 2;
        private const int buttonShards = 3;
        private const int buttonsCount = 4;

        private int activeTabId;

        private string[] buttonsNames = { "BattleGirls", "BattleGirlsEquip", "OverlordEquip", "Shards" };

        private int[] buttonsIds = { buttonBattleGirls, buttonBattleGirlsEquip, buttonOverlordEquip, buttonShards };
        private Button[] buttons = new Button[buttonsCount];
        private GameObject[] pressedButtons = new GameObject[buttonsCount];
        private GameObject[] contents = new GameObject[buttonsCount];
        
        private NSPortalScreen.BaseTab selectedTab;

        private List<NSPortalScreen.BaseTab> battleGirlsTabs = new List<NSPortalScreen.BaseTab>();
        private List<NSPortalScreen.BaseTab> battleGirlsEquipTabs = new List<NSPortalScreen.BaseTab>();
        private List<NSPortalScreen.BaseTab> overlordTabs = new List<NSPortalScreen.BaseTab>();
        private List<NSPortalScreen.BaseTab> shardsTabs = new List<NSPortalScreen.BaseTab>();

        private Button backButton;

        private void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/PortalScreen/PortalScreen", transform);

            var canvas = screenInst.transform.Find("Canvas");
            var tabArea = canvas.Find("TabArea");
            var bannersBack = canvas.Find("BannersBackground");

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);
            
            foreach (var i in buttonsIds)
            {
                buttons[i] = tabArea.Find(buttonsNames[i]).GetComponent<Button>();
                pressedButtons[i] = buttons[i].transform.Find("ButtonSelected").gameObject;
                buttons[i].onClick.AddListener(() =>
                {
                    ButtonClick(i);
                });

                contents[i] = bannersBack.Find(buttonsNames[i] + "Content").gameObject;
            }
            
        }

        public override async Task BeforeShowMakeAsync()
        {
            Customize();
            activeTabId = inputData?.activeButtonId ?? buttonBattleGirls;
            ButtonClick(activeTabId);

            await Task.CompletedTask;
        }

        private void AddTab(NSPortalScreen.BaseTab newTab, List<NSPortalScreen.BaseTab> tabs)
        {
            newTab.selectTab += SelectTab;
            
            SetPosition(newTab, tabs);
            tabs.Add(newTab);
        }

        private void SetPosition(NSPortalScreen.BaseTab tab, List<NSPortalScreen.BaseTab> tabs)
        {
            const float offsetX = 586f;

            if (tabs.Count == 0)
                return;
            
            var lastTabPos = tabs.Last().GetComponent<RectTransform>().anchoredPosition;
            var tabRect = tab.GetComponent<RectTransform>();
            tabRect.anchoredPosition = new Vector2(lastTabPos.x + offsetX, lastTabPos.y);
        }
        
        private void Customize()
        {
            foreach (var gacha in GameData.gacha.items)
            {
                switch (gacha.tabType)
                {
                    case AdminBRO.GachItem.TabType_Matriachs:
                        {
                            NSPortalScreen.BaseTab newTab = gacha.type switch
                            {
                                AdminBRO.GachItem.Type_Linear => NSPortalScreen.TabLinear.GetInstance(contents[buttonBattleGirls].transform),
                                AdminBRO.GachItem.Type_Stepwise => NSPortalScreen.TabStepwise.GetInstance(contents[buttonBattleGirls].transform),
                                _ => null
                            };
                            newTab.gachaId = gacha.id;
                            AddTab(newTab, battleGirlsTabs);
                        }
                        break;
                    case AdminBRO.GachItem.TabType_CharactersEquipment:
                        {
                            NSPortalScreen.BaseTab newTab = gacha.type switch
                            {
                                AdminBRO.GachItem.Type_Linear => NSPortalScreen.TabLinear.GetInstance(contents[buttonBattleGirlsEquip].transform),
                                AdminBRO.GachItem.Type_Stepwise => NSPortalScreen.TabStepwise.GetInstance(contents[buttonBattleGirlsEquip].transform),
                                _ => null
                            };
                            newTab.gachaId = gacha.id;
                            AddTab(newTab, battleGirlsEquipTabs);
                        }
                        break;
                    case AdminBRO.GachItem.TabType_OverlordEquipment:
                        {
                            NSPortalScreen.BaseTab newTab = gacha.type switch
                            {
                                AdminBRO.GachItem.Type_Linear => NSPortalScreen.TabLinear.GetInstance(contents[buttonOverlordEquip].transform),
                                AdminBRO.GachItem.Type_Stepwise => NSPortalScreen.TabStepwise.GetInstance(contents[buttonOverlordEquip].transform),
                                _ => null
                            };
                            newTab.gachaId = gacha.id;
                            AddTab(newTab, overlordTabs);
                        }
                        break;
                    case AdminBRO.GachItem.TabType_Shards:
                        {
                            NSPortalScreen.BaseTab newTab = gacha.type switch
                            {
                                AdminBRO.GachItem.Type_Linear => NSPortalScreen.TabLinear.GetInstance(contents[buttonShards].transform),
                                AdminBRO.GachItem.Type_Stepwise => NSPortalScreen.TabStepwise.GetInstance(contents[buttonShards].transform),
                                _ => null
                            };
                            newTab.gachaId = gacha.id;
                            AddTab(newTab, shardsTabs);
                        }
                        break;
                }
            }
            
            foreach (var i in buttonsIds)
            {
                pressedButtons[i].gameObject.SetActive(false);
                contents[i].gameObject.SetActive(false);
            }
        }
        
        private void MoveTabs(NSPortalScreen.BaseTab tab, List<NSPortalScreen.BaseTab> tabs)
        {
            var isLeftDirection = tab.GetComponent<RectTransform>().anchoredPosition.x >
                                  selectedTab?.GetComponent<RectTransform>().anchoredPosition.x;

            var tabRect = tab.GetComponent<RectTransform>();
            
            while (tabRect.anchoredPosition.x != 0)
            {
                foreach (var t in tabs)
                {
                    if(isLeftDirection)
                        t.MoveLeft();
                    else
                        t.MoveRight();
                }
            }
        }
        
        private void SelectTab(NSPortalScreen.BaseTab tab)
        {
            selectedTab?.Deselect();

            switch (tab.gachaData.tabType)
            {
                case AdminBRO.GachItem.TabType_Matriachs:
                    MoveTabs(tab, battleGirlsTabs);
                    break;
                case AdminBRO.GachItem.TabType_CharactersEquipment:
                    MoveTabs(tab, battleGirlsEquipTabs);
                    break;
                case AdminBRO.GachItem.TabType_OverlordEquipment:
                    MoveTabs(tab, overlordTabs);
                    break;
                case AdminBRO.GachItem.TabType_Shards:
                    MoveTabs(tab, shardsTabs);
                    break;
            }
            
            selectedTab = tab;
            selectedTab?.Select();
        }
        
        private void ButtonClick(int buttonId)
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            LeaveTab(activeTabId);
            EnterTab(buttonId);
        }
        
        private void EnterTab(int buttonId)
        {
            activeTabId = buttonId;
            pressedButtons[buttonId].SetActive(true);
            contents[buttonId].SetActive(true);
            
            switch (buttonId)
            {
                case buttonBattleGirls:
                    if(battleGirlsTabs.Any())
                        SelectTab(battleGirlsTabs.First());
                    break;
                case buttonBattleGirlsEquip:
                    if(battleGirlsEquipTabs.Any())
                        SelectTab(battleGirlsEquipTabs.First());
                    break;
                case buttonOverlordEquip:
                    if(overlordTabs.Any())
                        SelectTab(overlordTabs.First());
                    break;
                case buttonShards:
                    if(shardsTabs.Any())
                        SelectTab(shardsTabs.First());
                    break;
            }
        }

        private void LeaveTab(int buttonId)
        {
            pressedButtons[buttonId].SetActive(false);
            contents[buttonId].SetActive(false);
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

        public const int battleGirlsButtonId = 0;
        public const int battleGirlsEquipButtonId = 1;
        public const int overlordEquipButtonId = 2;
        public const int shardsButtonId = 3;
    }
}
