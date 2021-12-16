using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSQuestOverlay
    {
        public class SideQuestInfo : MonoBehaviour
        {
            private TextMeshProUGUI title;
            private TextMeshProUGUI progress;

            private Image[] rewardResource = new Image[6];
            private TextMeshProUGUI[] rewardCount = new TextMeshProUGUI[6];

            private Button toQuestButton;
            private TextMeshProUGUI toQuestButtonText;

            void Awake()
            {
                var canvas = transform.Find("Canvas");

                var rewardWindow = canvas.Find("RewardWindow");

                title = rewardWindow.Find("Title").GetComponent<TextMeshProUGUI>();
                progress = rewardWindow.Find("Progress").GetComponent<TextMeshProUGUI>();

                var rewardGrid = rewardWindow.Find("RewardGrid");
                for (int i = 0; i < 6; i++)
                {
                    var reward = rewardGrid.Find($"Reward{i + 1}");
                    rewardResource[i] = reward.Find("Resource").GetComponent<Image>();
                    rewardCount[i] = reward.Find("Count").GetComponent<TextMeshProUGUI>();
                }

                toQuestButton = rewardWindow.Find("ToQuestButton").GetComponent<Button>();
                toQuestButton.onClick.AddListener(ToQuestButtonClick);
                toQuestButtonText = toQuestButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            }

            private void Start()
            {
                    Customize();
            }

            private void Customize()
            {
                rewardResource[0].sprite = Resources.Load<Sprite>("Prefabs/UI/Common/Images/Recources/Crystal");
                rewardResource[1].sprite = Resources.Load<Sprite>("Prefabs/UI/Common/Images/Recources/Wood");
                rewardResource[2].sprite = Resources.Load<Sprite>("Prefabs/UI/Common/Images/Recources/Gold");
                rewardResource[3].sprite = Resources.Load<Sprite>("Prefabs/UI/Common/Images/Recources/Stone");
                rewardResource[4].sprite = Resources.Load<Sprite>("Prefabs/UI/Common/Images/Recources/Copper");
                rewardResource[5].sprite = Resources.Load<Sprite>("Prefabs/UI/Common/Images/Recources/Gem");

            }
            
            private void ToQuestButtonClick()
            {

            }

            public static SideQuestInfo GetInstance(Transform parent)
            {
                var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Overlays/QuestOverlay/SideQuestInfo"), parent);
                newItem.name = nameof(SideQuestInfo);
                return newItem.AddComponent<SideQuestInfo>();
            }
        }
    }
}
