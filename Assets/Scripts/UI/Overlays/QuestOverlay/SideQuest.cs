using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSQuestOverlay
    {
        public class SideQuest : MonoBehaviour
        {
            private Text title;
            private Text progress;

            private Image[] revardRecource = new Image[6];
            private Text[] revardCount = new Text[6];

            private Button toQuestButton;
            private Text toQuestButtonText;

            void Start()
            {
                var canvas = transform.Find("Canvas");

                var rewardWindow = canvas.Find("RewardWindow");

                title = rewardWindow.Find("Title").GetComponent<Text>();
                progress = rewardWindow.Find("Progress").GetComponent<Text>();

                var rewardGrid = rewardWindow.Find("RewardGrid");
                for (int i = 0; i < 6; i++)
                {
                    var reward = rewardGrid.Find("Reward" + i.ToString());
                    revardRecource[i] = reward.Find("Recource").GetComponent<Image>();
                    revardCount[i] = reward.Find("Count").GetComponent<Text>();
                }

                toQuestButton = rewardWindow.Find("ToQuestButton").GetComponent<Button>();
                toQuestButton.onClick.AddListener(ToQuestButtonClick);
                toQuestButtonText = toQuestButton.transform.Find("Text").GetComponent<Text>();
            }

            void Update()
            {

            }

            private void ToQuestButtonClick()
            {

            }

            public static SideQuest GetInstance(Transform parent)
            {
                var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Overlays/QuestOverlay/SideQuest"), parent);
                newItem.name = nameof(SideQuest);
                return newItem.AddComponent<SideQuest>();
            }
        }
    }
}
