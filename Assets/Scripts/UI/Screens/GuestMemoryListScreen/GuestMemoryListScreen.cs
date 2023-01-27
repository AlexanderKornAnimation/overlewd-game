using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class GuestMemoryListScreen : BaseFullScreenParent<GuestMemoryListScreenInData>
    {
        private Button backButton;
        private Transform scrollContent;
        private Transform tabContentPos;
        private GameObject notificationScroll;
        private NSGuestMemoryListScreen.Tab selectedTab;

        private void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/GuestMemoryListScreen/GuestMemoriesListScreen", transform);

            var canvas = screenInst.transform.Find("Canvas");

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);
            scrollContent = canvas.Find("TabScroll/Viewport/Content");
            tabContentPos = canvas.Find("TabContentPos");
            notificationScroll = canvas.Find("NotificationScroll").gameObject;
        }

        public override async Task BeforeShowMakeAsync()
        {
            Customize();
            await Task.CompletedTask;
        }

        private void Customize()
        {
            var memories = GameData.matriarchs.memories.FindAll(m => m.matriarchId == inputData?.girlId);
            foreach (var memoryData in memories)
            {
                var tab = NSGuestMemoryListScreen.Tab.GetInstance(scrollContent);
                tab.tabContentPos = tabContentPos;
                tab.memoryId = memoryData.id;
                tab.Customize();
                tab.OnClick += SelectTab;
            }
            
            TrySelectDefaultTab();
            notificationScroll.SetActive(scrollContent.childCount > 4);
        }

        private void TrySelectDefaultTab()
        {
            var tab = scrollContent.GetComponentsInChildren<NSGuestMemoryListScreen.Tab>().FirstOrDefault();
            
            if(tab != null)
                SelectTab(tab);
        }
        
        private void SelectTab(NSGuestMemoryListScreen.Tab tab)
        {
            selectedTab?.Deselect();
            selectedTab = tab;
            selectedTab?.Select();
        }
        
        private void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ToPrevScreen();
        }
    }

    public class GuestMemoryListScreenInData : BaseFullScreenInData
    {
        public int? girlId { get; set; }
        public AdminBRO.MatriarchItem girlData => GameData.matriarchs.GetMatriarchById(girlId);
    }
}
