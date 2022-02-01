using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class QuestOverlay : BaseOverlay
    {
        private Button backButton;

        private Transform questScrollView;

        private Transform mainQuestGrid;
        private TextMeshProUGUI mainQuestGridTitle;
        private GameObject mainQuestGridMark;

        private Transform matriarchQuestGrid;
        private TextMeshProUGUI matriarchQuestGridTitle;
        private GameObject matriarchQuestGridMark;

        private Transform sideQuestGrid;
        private TextMeshProUGUI sideQuestGridTitle;
        private GameObject sideQuestGridMark;

        private TextMeshProUGUI headlineTitle;
        private GameObject headlineMainQuestMark;
        private GameObject headlineSideQuestMark;

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
            mainQuestGridTitle = mainQuestGrid.Find("QuestHead").Find("Title").GetComponent<TextMeshProUGUI>();
            mainQuestGridMark = mainQuestGrid.Find("QuestHead").Find("MainQuestMark").gameObject;

            matriarchQuestGrid = questScrollView_content.Find("MatriarchQuestGrid");
            matriarchQuestGridTitle = matriarchQuestGrid.Find("QuestHead").Find("Title").GetComponent<TextMeshProUGUI>();
            matriarchQuestGridMark = matriarchQuestGrid.Find("QuestHead").Find("SideQuestMark").gameObject;

            sideQuestGrid = questScrollView_content.Find("SideQuestGrid");
            sideQuestGridTitle = sideQuestGrid.Find("QuestHead").Find("Title").GetComponent<TextMeshProUGUI>();
            sideQuestGridMark = sideQuestGrid.Find("QuestHead").Find("SideQuestMark").gameObject;

            var headline = canvas.Find("Headline");
            headlineTitle = headline.Find("Title").GetComponent<TextMeshProUGUI>();
            headlineMainQuestMark = headline.Find("MainQuestMark").gameObject;
            headlineSideQuestMark = headline.Find("SideQuestMark").gameObject;

            questContentScrollViewPos = canvas.Find("QuestContentScrollViewPos");
        }

        void Start()
        {
            Customize();
        }

        private void Customize()
        {
            AddMainQuest();
            AddMainQuest();

            AddMatriarchQuest();
            AddMatriarchQuest();
            AddMatriarchQuest();

            AddSideQuest();
            AddSideQuest();

            if (quests.Any())
            {
                SelectQuest(quests.First());
            }
        }

        private void AddMainQuest()
        {
            var newQuest = NSQuestOverlay.MainQuestButton.GetInstance(mainQuestGrid);
            newQuest.contentScrollView = NSQuestOverlay.QuestContentScrollView.
                GetInstance(questContentScrollViewPos);
            newQuest.buttonPressed += SelectQuest;

            quests.Add(newQuest);
        }

        private void AddMatriarchQuest()
        {
            var newQuest = NSQuestOverlay.MatriarchQuestButton.GetInstance(matriarchQuestGrid);
            newQuest.contentScrollView = NSQuestOverlay.QuestContentScrollView.
                GetInstance(questContentScrollViewPos);
            newQuest.buttonPressed += SelectQuest;

            quests.Add(newQuest);
        }

        private void AddSideQuest()
        {
            var newQuest = NSQuestOverlay.SideQuestButton.GetInstance(sideQuestGrid);
            newQuest.contentScrollView = NSQuestOverlay.QuestContentScrollView.
                GetInstance(questContentScrollViewPos);
            newQuest.buttonPressed += SelectQuest;

            quests.Add(newQuest);
        }

        public void SelectQuest(NSQuestOverlay.QuestButton quest)
        {
            selectedQuest?.Deselect();
            selectedQuest = quest;
            selectedQuest?.Select();
        }

        private void BackButtonClick()
        {
            SoundManager.PlayOneShoot(SoundManager.SoundPath.UI.ButtonClick);
            UIManager.HideOverlay();
        }
    }
}
