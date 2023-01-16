using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class MatriarchMemoryListScreen : BaseFullScreenParent<MatriarchMemoryListScreenInData>
    {
        public const int TabStoryMemory = 0;
        public const int TabMainMemory = 1;
        public const int TabEventMemory = 2;
        private const int TabsCount = 3;
        private int activeTabId = TabStoryMemory;

        private string[] tabNames = {"StoryMemories", "MainMemories", "EventMemories"};
        private int[] tabIds = {TabMainMemory, TabStoryMemory, TabEventMemory};
        private GameObject[] scrolls = new GameObject[TabsCount];
        private Transform[] scrollsContent = new Transform[TabsCount];
        private Button[] tabs = new Button[TabsCount];
        private Button backButton;
        private TextMeshProUGUI backButtonTitle;

        private void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab(
                    "Prefabs/UI/Screens/MatriarchMemoryListScreen/MatriarchMemoryListScreen", transform);

            var canvas = screenInst.transform.Find("Canvas");

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);
            backButtonTitle = backButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();

            foreach (var i in tabIds)
            {
                tabs[i] = canvas.Find($"Tabs/{tabNames[i]}").GetComponent<Button>();
                tabs[i].onClick.AddListener(() => TabClick(i));
                scrolls[i] = canvas.Find($"Scrolls/Scroll_{tabNames[i]}").gameObject;
                scrollsContent[i] = scrolls[i].transform.Find("Viewport/Content");
                scrolls[i].gameObject.SetActive(false);
            }
        }

        public override async Task BeforeShowMakeAsync()
        {
            Customize();
            EnterTab(activeTabId);
            await Task.CompletedTask;
        }

        private void Customize()
        {
            backButtonTitle.text = $"Back to\n {inputData.girlKey}'s room";

            var memories = GameData.matriarchs.memories.Where(m => m.matriarchId == inputData?.girlData?.id).ToList();
            
            var tabStoryImage = tabs[TabStoryMemory].GetComponent<Image>();
            tabStoryImage.sprite = ResourceManager.LoadSprite(memories.FirstOrDefault(m => m.isStory)?.sexScenePreview);
            
            var tabMainImage = tabs[TabMainMemory].GetComponent<Image>();
            tabMainImage.sprite = ResourceManager.LoadSprite(memories.FirstOrDefault(m => m.isMain)?.sexScenePreview);
            
            var tabEventImage = tabs[TabEventMemory].GetComponent<Image>();
            tabEventImage.sprite = ResourceManager.LoadSprite(memories.FirstOrDefault(m => m.isEvent)?.sexScenePreview);
            
            foreach (var memoryItem in memories)
            {
                if(!memoryItem.isVisible || !memoryItem.isOpen)
                    continue;
                
                var content = memoryItem.memoryType switch
                {
                    AdminBRO.MemoryItem.MemoryType_Story => scrollsContent[TabStoryMemory],
                    AdminBRO.MemoryItem.MemoryType_Main => scrollsContent[TabMainMemory],
                    AdminBRO.MemoryItem.MemoryType_Event => scrollsContent[TabEventMemory],
                    _ => null,
                };
                
                NSMatriarchMemoryListScreen.BaseMemoryBanner memoryBanner = memoryItem.memoryType switch
                {
                    AdminBRO.MemoryItem.MemoryType_Story => NSMatriarchMemoryListScreen.MemoryBanner.GetInstance(content),
                    AdminBRO.MemoryItem.MemoryType_Main => NSMatriarchMemoryListScreen.MemoryBannerWithShards.GetInstance(content),
                    AdminBRO.MemoryItem.MemoryType_Event => NSMatriarchMemoryListScreen.MemoryBannerWithShards.GetInstance(content),
                    _ => null,
                };

                if (memoryBanner != null)
                {
                    memoryBanner.memoryId = memoryItem.id;
                }
            }
        }
        
        private void TabClick(int tabId)
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            LeaveTab(activeTabId);
            EnterTab(tabId);
        }

        private void EnterTab(int tabId)
        {
            scrolls[tabId].SetActive(true);
            var selected = tabs[tabId].transform.Find("Selected").gameObject;
            selected.SetActive(true);
            MoveTabHorizontal(tabId);
            activeTabId = tabId;
        }

        private void LeaveTab(int tabId)
        {
            scrolls[tabId].SetActive(false);
            var selected = tabs[tabId].transform.Find("Selected").gameObject;
            selected.SetActive(false);
            MoveTabHorizontal(tabId);
        }

        private void MoveTabHorizontal(int tabId)
        {
            var tab = tabs[tabId];
            var tabRectTr = tab.GetComponent<RectTransform>();
            var currentPos = tabRectTr.anchoredPosition;
            var isSelected = tabs[tabId].transform.Find("Selected").gameObject.activeSelf;

            tabRectTr.anchoredPosition = isSelected ? new Vector2(0, currentPos.y) : new Vector2(-62, currentPos.y);
        }

        private void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ToPrevScreen();
        }
    }

    public class MatriarchMemoryListScreenInData : BaseFullScreenInData
    {
        public string girlKey;

        public AdminBRO.MatriarchItem girlData =>
            GameData.matriarchs.GetMatriarchByKey(girlKey);
    }
}