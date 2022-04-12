using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class MemoryListScreen : BaseFullScreen
    {
        protected Dictionary<string, List<NSMemoryListScreen.BaseScroll>> content =
            new Dictionary<string, List<NSMemoryListScreen.BaseScroll>>();

        protected Button backButton;
        protected TextMeshProUGUI backButtonGirlName;

        protected Button ulviTab;
        protected Button adrielTab;
        protected Button fayeTab;
        protected Button ingieTab;
        protected Button liliTab;
        protected Button prevTab;

        protected virtual void Awake()
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

        protected virtual void EnterTab(Button girlTab)
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

            var girlContent = content[girlTab.name];
            
            foreach (var _content in girlContent)
            {
                _content.Show();
            }
            
            Select(girlTab);
        }
        
        protected virtual void Select(Button girlTab)
        {
            prevTab = girlTab;
            girlTab.transform.Find("SelectedTab").gameObject.SetActive(true);

            backButtonGirlName.text = girlTab.name + "`s";
        }

        protected virtual void LeaveTab(Button girlTab)
        {
            Deselect();

            var girlContent = content[girlTab.name];
            
            foreach (var _content in girlContent)
            {
                _content.Hide();
            }
        }
        
        protected virtual void Deselect()
        {
            var selectedTab = prevTab.transform.Find("SelectedTab");
            selectedTab.gameObject.SetActive(false);
        }

        protected virtual void UlviButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            LeaveTab(prevTab);
            EnterTab(ulviTab);
        }

        protected virtual void AdrielButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            LeaveTab(prevTab);
            EnterTab(adrielTab);
        }

        protected virtual void FayeButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            LeaveTab(prevTab);
            EnterTab(fayeTab);
        }

        protected virtual void IngieButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            LeaveTab(prevTab);
            EnterTab(ingieTab);
        }

        protected virtual void LiliButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            LeaveTab(prevTab);
            EnterTab(liliTab);
        }

        protected virtual void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<GirlScreen>();
        }

        protected virtual void Start()
        {
            Customize();
        }

        protected virtual void Customize()
        {
        }
    }
}