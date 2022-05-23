using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Threading.Tasks;

namespace Overlewd
{
    public class PortalScreen : BaseFullScreen
    {
        private Button shardsButton;
        private Image shardsButtonSelected;
        private Button battleGirlsButton;
        private Image battleGirlsButtonSelected;
        private Button battleGirlsEquipButton;
        private Image battleGirlsEquipButtonSelected;
        private Button overlordEquipButton;
        private Image overlordEquipButtonSelected;
        private Button backButton;

        private GameObject battleGirlsEquipContent;
        private GameObject shardsContent;
        private GameObject battleGirlsContent;
        private GameObject overlordEquipContent;

        private NSPortalScreen.BaseTab selectedTab;

        private List<NSPortalScreen.BaseTab> battleGirlsTabs = new List<NSPortalScreen.BaseTab>();
        private List<NSPortalScreen.BaseTab> battleGirlsEquipTabs = new List<NSPortalScreen.BaseTab>();
        private List<NSPortalScreen.BaseTab> overlordTabs = new List<NSPortalScreen.BaseTab>();
        private List<NSPortalScreen.BaseTab> memoryTabs = new List<NSPortalScreen.BaseTab>();

        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/PortalScreen/PortalScreen", transform);

            var canvas = screenInst.transform.Find("Canvas");
            var tabArea = canvas.Find("TabArea");
            var bannersBack = canvas.Find("BannersBackground");

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);
            
            shardsButton = tabArea.Find("ShardsButton").GetComponent<Button>();
            shardsButton.onClick.AddListener(ShardsButtonClick);
            shardsButtonSelected = shardsButton.transform.Find("ButtonSelected").GetComponent<Image>();
            
            battleGirlsButton = tabArea.Find("BattleGirlsButton").GetComponent<Button>();
            battleGirlsButton.onClick.AddListener(BattleGirlsButtonClick);
            battleGirlsButtonSelected = battleGirlsButton.transform.Find("ButtonSelected").GetComponent<Image>();

            battleGirlsEquipButton = tabArea.Find("BattleGirlsEquipButton").GetComponent<Button>();
            battleGirlsEquipButton.onClick.AddListener(BattleGirlsEquipButtonClick);
            battleGirlsEquipButtonSelected = battleGirlsEquipButton.transform.Find("ButtonSelected").GetComponent<Image>();
            
            overlordEquipButton = tabArea.Find("OverlordEquipButton").GetComponent<Button>();
            overlordEquipButton.onClick.AddListener(OverlordEquipButtonClick);
            overlordEquipButtonSelected = overlordEquipButton.transform.Find("ButtonSelected").GetComponent<Image>();

            shardsContent = bannersBack.Find("ShardsContent").gameObject;
            battleGirlsContent = bannersBack.Find("BattleGirlsContent").gameObject;
            overlordEquipContent = bannersBack.Find("OverlordEquipContent").gameObject;
            battleGirlsEquipContent = bannersBack.Find("BattleGirlsEquipContent").gameObject;
        }

        public override async Task BeforeShowMakeAsync()
        {
            Customize();

            BattleGirlsButtonClick();

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
                                AdminBRO.GachItem.Type_Linear => NSPortalScreen.TabLinear.GetInstance(battleGirlsContent.transform),
                                AdminBRO.GachItem.Type_Stepwise => NSPortalScreen.TabStepwise.GetInstance(battleGirlsContent.transform),
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
                                AdminBRO.GachItem.Type_Linear => NSPortalScreen.TabLinear.GetInstance(battleGirlsEquipContent.transform),
                                AdminBRO.GachItem.Type_Stepwise => NSPortalScreen.TabStepwise.GetInstance(battleGirlsEquipContent.transform),
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
                                AdminBRO.GachItem.Type_Linear => NSPortalScreen.TabLinear.GetInstance(overlordEquipContent.transform),
                                AdminBRO.GachItem.Type_Stepwise => NSPortalScreen.TabStepwise.GetInstance(overlordEquipContent.transform),
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
                                AdminBRO.GachItem.Type_Linear => NSPortalScreen.TabLinear.GetInstance(shardsContent.transform),
                                AdminBRO.GachItem.Type_Stepwise => NSPortalScreen.TabStepwise.GetInstance(shardsContent.transform),
                                _ => null
                            };
                            newTab.gachaId = gacha.id;
                            AddTab(newTab, memoryTabs);
                        }
                        break;
                }
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
                    MoveTabs(tab, memoryTabs);
                    break;
            }
            
            selectedTab = tab;
            selectedTab?.Select();
        }
        
        private void ShardsButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            shardsButtonSelected.gameObject.SetActive(true);
            shardsContent.SetActive(true);
            
            battleGirlsButtonSelected.gameObject.SetActive(false);
            battleGirlsContent.SetActive(false);
            
            overlordEquipButtonSelected.gameObject.SetActive(false);
            overlordEquipContent.SetActive(false);
            
            battleGirlsEquipButtonSelected.gameObject.SetActive(false);
            battleGirlsEquipContent.SetActive(false);
            
            
            if (memoryTabs.Any())
            {
                SelectTab(memoryTabs.First());
            }
        }

        private void BattleGirlsEquipButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            battleGirlsEquipButtonSelected.gameObject.SetActive(true);
            battleGirlsContent.SetActive(true);
            
            shardsButtonSelected.gameObject.SetActive(false);
            shardsContent.SetActive(false);

            battleGirlsButtonSelected.gameObject.SetActive(false);
            battleGirlsContent.SetActive(false);

            overlordEquipButtonSelected.gameObject.SetActive(false);
            overlordEquipContent.SetActive(false);
            
            if (battleGirlsTabs.Any())
            {
                SelectTab(battleGirlsTabs.First());
            }
        }
        
        private void BattleGirlsButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            battleGirlsButtonSelected.gameObject.SetActive(true);
            battleGirlsContent.SetActive(true);
            
            shardsButtonSelected.gameObject.SetActive(false);
            shardsContent.SetActive(false);

            overlordEquipButtonSelected.gameObject.SetActive(false);
            overlordEquipContent.SetActive(false);
            
            battleGirlsEquipButtonSelected.gameObject.SetActive(false);
            battleGirlsContent.SetActive(false);
            
            if (battleGirlsEquipTabs.Any())
            {
                SelectTab(battleGirlsEquipTabs.First());
            }
        }

        private void OverlordEquipButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            overlordEquipButtonSelected.gameObject.SetActive(true);
            overlordEquipContent.SetActive(true);
            
            shardsButtonSelected.gameObject.SetActive(false);
            shardsContent.SetActive(false);

            battleGirlsButtonSelected.gameObject.SetActive(false);
            battleGirlsContent.SetActive(false);

            battleGirlsEquipButtonSelected.gameObject.SetActive(false);
            battleGirlsContent.SetActive(false);
            
            if (overlordTabs.Any())
            {
                SelectTab(overlordTabs.First());
            }
        }

        private void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<CastleScreen>();
        }

    }
}
