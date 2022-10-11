using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class QuestOverlay : BaseOverlayParent<QuestOverlayInData>
    {
        private Button backButton;

        private Transform questScrollView;

        private Transform mainQuestGrid;
        private GameObject mainQuestGridMark;

        private Transform matriarchQuestGrid;
        private GameObject matriarchQuestGridMark;

        private Transform sideQuestGrid;
        private GameObject sideQuestGridMark;

        private TextMeshProUGUI headlineTitle;
        private TextMeshProUGUI headlineQuestMark;

        private Transform questContentScrollViewPos;

        private NSQuestOverlay.QuestButton selectedQuest;
        private List<NSQuestOverlay.QuestButton> quests = new List<NSQuestOverlay.QuestButton>();

        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Overlays/QuestOverlay/QuestOverlay", transform);

            var canvas = screenInst.transform.Find("Canvas");

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);

            questScrollView = canvas.Find("QuestScrollView");
            var questScrollView_content = questScrollView.Find("Viewport").Find("Content");

            mainQuestGrid = questScrollView_content.Find("MainQuestGrid");
            mainQuestGridMark = mainQuestGrid.Find("QuestHead").Find("QuestMark").gameObject;

            matriarchQuestGrid = questScrollView_content.Find("MatriarchQuestGrid");
            matriarchQuestGridMark = matriarchQuestGrid.Find("QuestHead").Find("QuestMark").gameObject;

            sideQuestGrid = questScrollView_content.Find("SideQuestGrid");
            sideQuestGridMark = sideQuestGrid.Find("QuestHead").Find("QuestMark").gameObject;

            var headline = canvas.Find("Headline");
            headlineTitle = headline.Find("Title").GetComponent<TextMeshProUGUI>();
            headlineQuestMark = headline.Find("QuestMark").GetComponent<TextMeshProUGUI>();

            questContentScrollViewPos = canvas.Find("QuestContentScrollViewPos");
        }

        public override void StartShow()
        {
            if (UIManager.HasScreen<CastleScreen>())
                SoundManager.PlayOneShot(FMODEventPath.UI_QuestOverlayShow);
        }

        public override void StartHide()
        {
            if (UIManager.HasScreen<CastleScreen>())
                SoundManager.PlayOneShot(FMODEventPath.UI_QuestOverlayHide);
        }

        private void Start()
        {
            Customize();
        }

        private void Customize()
        {
            foreach (var questItem in GameData.quests.ftueQuests)
            {
                if (GameData.ftue.mapChapter.id == questItem.ftueChapterId)
                {
                    var grid = GetQuestGridByType(questItem.ftueQuestType);

                    if (grid != null)
                    {
                        var quest = NSQuestOverlay.QuestButton.GetInstance(grid);
                        quest.questId = questItem.id;
                        quest.questContentPos = questContentScrollViewPos;
                        quest.buttonPressed += SelectQuest;
                        quest.Customize();
                        quests.Add(quest);
                    }
                }
            }

            var selectedQuest = quests?.FirstOrDefault(q => q.questId == inputData?.questId);
            SelectQuest(selectedQuest != null ? selectedQuest : quests?.FirstOrDefault());
        }

        private Transform GetQuestGridByType(string type)
        {
            return type switch
            {
                AdminBRO.QuestItem.QuestType_Main => mainQuestGrid,
                AdminBRO.QuestItem.QuestType_Matriarch => matriarchQuestGrid,
                AdminBRO.QuestItem.QuestType_Side => sideQuestGrid,
                _ => null,
            };
        }
        
        private void SelectQuest(NSQuestOverlay.QuestButton quest)
        {
            selectedQuest?.Deselect();
            quest?.Select();
            selectedQuest = quest;
            headlineTitle.text = selectedQuest?.questData?.name;
        }

        private void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.HideOverlay();
        }
    }

    public class QuestOverlayInData : BaseOverlayInData
    {
        public int? questId;
        public AdminBRO.QuestItem questData => GameData.quests.GetById(questId);
    }
}
