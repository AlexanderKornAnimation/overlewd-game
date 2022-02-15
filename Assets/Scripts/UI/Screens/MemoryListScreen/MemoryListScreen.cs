using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class MemoryListScreen : BaseScreen
    {
        private Dictionary<string, List<NSMemoryListScreen.BaseScroll>> content =
            new Dictionary<string, List<NSMemoryListScreen.BaseScroll>>();

        private Button backButton;
        private TextMeshProUGUI backButtonGirlName;

        private Button ulviTab;
        private Button adrielTab;
        private Button fayeTab;
        private Button ingieTab;
        private Button liliTab;
        private Button prevTab;

        private void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/MemoryListScreen/MemoryListScreen", transform);

            var canvas = screenInst.transform.Find("Canvas");
            var tabArea = canvas.Find("TabArea");

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButtonGirlName = backButton.transform.Find("GirlName").GetComponent<TextMeshProUGUI>();
            backButton.onClick.AddListener(BackButtonClick);

            ulviTab = tabArea.Find("Ulvi").GetComponent<Button>();
            ulviTab.onClick.AddListener(UlviButtonClick);

            adrielTab = tabArea.Find("Adriel").GetComponent<Button>();
            adrielTab.onClick.AddListener(AdrielButtonClick);

            fayeTab = tabArea.Find("Faye").GetComponent<Button>();
            fayeTab.onClick.AddListener(FayeButtonClick);

            ingieTab = tabArea.Find("Ingie").GetComponent<Button>();
            ingieTab.onClick.AddListener(IngieButtonClick);

            liliTab = tabArea.Find("Lili").GetComponent<Button>();
            liliTab.onClick.AddListener(LiliButtonClick);

            var selectedGirl = GameGlobalStates.haremGirlNameSelected switch
            {
                "Ulvi" => ulviTab,
                "Adriel" => adrielTab,
                "Faye" => fayeTab,
                "Ingie" => ingieTab,
                "Lili" => liliTab,
                _ => ulviTab
            };

            EnterTab(selectedGirl);
        }

        private void EnterTab(Button girlTab)
        {
            var canvas = transform.GetChild(0).transform.Find("Canvas");
            
            var closedEventMemoryScrollPos = canvas.Find("ClosedEventMemoryScrollPos");
            var openedEventMemoryScrollPos = canvas.Find("OpenedEventMemoryScrollPos");
            var mainMemoryScrollPos = canvas.Find("MainMemoryScrollPos");
            
            if (!content.ContainsKey(girlTab.name))
            {
                var closedEventMemoryScroll = NSMemoryListScreen.ClosedEventMemoryScroll.GetInstance(closedEventMemoryScrollPos);
                var openedEventMemoryScroll = NSMemoryListScreen.OpenedEventMemoryScroll.GetInstance(openedEventMemoryScrollPos);
                var mainMemoryScroll = NSMemoryListScreen.MainMemoryScroll.GetInstance(mainMemoryScrollPos);

                content.Add(girlTab.name, new List<NSMemoryListScreen.BaseScroll>());
                
                content[girlTab.name].Add(closedEventMemoryScroll);
                content[girlTab.name].Add(openedEventMemoryScroll);
                content[girlTab.name].Add(mainMemoryScroll);
            }

            foreach (var scroll in content)
            {
                if (scroll.Key == girlTab.name)
                {
                    for (int i = 0; i < scroll.Value.Count; i++)
                    {
                        scroll.Value[i].Show();
                    }
                }
            }
            
            Select(girlTab);
        }
        
        private void Select(Button girlTab)
        {
            prevTab = girlTab;
            girlTab.transform.Find("SelectedTab").gameObject.SetActive(true);

            backButtonGirlName.text = girlTab.name + "`s";
        }

        private void LeaveTab(Button girlTab)
        {
            Deselect();

            foreach (var scroll in content)
            {
                if (scroll.Key == girlTab.name)
                {
                    for (int i = 0; i < scroll.Value.Count; i++)
                    {
                        scroll.Value[i].Hide();
                    }
                }
            }
        }
        
        private void Deselect()
        {
            var selectedTab = prevTab.transform.Find("SelectedTab");
            selectedTab.gameObject.SetActive(false);
        }

        private void UlviButtonClick()
        {
            SoundManager.PlayOneShoot(SoundPath.UI.GenericButtonClick);
            LeaveTab(prevTab);
            EnterTab(ulviTab);
        }

        private void AdrielButtonClick()
        {
            SoundManager.PlayOneShoot(SoundPath.UI.GenericButtonClick);
            LeaveTab(prevTab);
            EnterTab(adrielTab);
        }

        private void FayeButtonClick()
        {
            SoundManager.PlayOneShoot(SoundPath.UI.GenericButtonClick);
            LeaveTab(prevTab);
            EnterTab(fayeTab);
        }

        private void IngieButtonClick()
        {
            SoundManager.PlayOneShoot(SoundPath.UI.GenericButtonClick);
            LeaveTab(prevTab);
            EnterTab(ingieTab);
        }

        private void LiliButtonClick()
        {
            SoundManager.PlayOneShoot(SoundPath.UI.GenericButtonClick);
            LeaveTab(prevTab);
            EnterTab(liliTab);
        }

        private void BackButtonClick()
        {
            SoundManager.PlayOneShoot(SoundPath.UI.GenericButtonClick);
            UIManager.ShowScreen<GirlScreen>();
        }

        private void Start()
        {
            Customize();
        }

        private void Customize()
        {
        }
    }
}