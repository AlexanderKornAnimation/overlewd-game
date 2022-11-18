using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public List<NSQuestOverlay.QuestButton> questButtons =>
            questScrollView.GetComponentsInChildren<NSQuestOverlay.QuestButton>(true).ToList();
        public NSQuestOverlay.QuestButton selectedQuest =>
            questButtons.Find(qb => qb.isSelected);

        public List<AdminBRO.QuestItem> actualQuests =>
            GameData.quests.ftueQuests.
            Where(q => GameData.ftue.mapChapter.id == q.ftueChapterId).
            Where(q => !q.isClaimed).ToList();

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
            foreach (var questItem in actualQuests)
            {
                var grid = GetQuestGridByType(questItem.ftueQuestType);

                if (grid != null)
                {
                    var quest = NSQuestOverlay.QuestButton.GetInstance(grid);
                    quest.questId = questItem.id;
                    quest.questContentPos = questContentScrollViewPos;
                    quest.questOverlay = this;
                    quest.Customize();
                }
            }

            var questForSelect = questButtons.Find(qb => qb.questId == inputData?.questId) ?? questButtons.FirstOrDefault();
            questForSelect?.Select();
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

        public override async Task AfterShowAsync()
        {
            switch (GameData.ftue.stats.lastEndedStageData?.lerningKey)
            {
                case (FTUE.CHAPTER_2, FTUE.DIALOGUE_2):
                    GameData.ftue.chapter2.ShowNotifByKey("ch2empathytutor2");
                    break;
            }

            await Task.CompletedTask;
        }

        public void SelectQuest(NSQuestOverlay.QuestButton quest)
        {
            headlineTitle.text = quest?.questData?.name;
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
