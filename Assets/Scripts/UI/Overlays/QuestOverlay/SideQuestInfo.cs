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
                    rewardResource[i] = rewardGrid.Find($"Reward{i + 1}").GetComponent<Image>();
                    rewardCount[i] = rewardResource[i].transform.Find("Count").GetComponent<TextMeshProUGUI>();
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
                rewardResource[0].sprite = ResourceManager.InstantiateAsset<Sprite>("Common/Images/186x186/CopperWithSubstrate186x186");
                rewardResource[1].sprite = ResourceManager.InstantiateAsset<Sprite>("Common/Images/186x186/WoodWithSubstrate186x186");
                rewardResource[2].sprite = ResourceManager.InstantiateAsset<Sprite>("Common/Images/186x186/GoldWithSubstrate186x186");
                rewardResource[3].sprite = ResourceManager.InstantiateAsset<Sprite>("Common/Images/186x186/GemsWithSubstrate186x186");
                rewardResource[4].sprite = ResourceManager.InstantiateAsset<Sprite>("Common/Images/186x186/StoneWithSubstrate186x186");
                rewardResource[5].sprite = ResourceManager.InstantiateAsset<Sprite>("Common/Images/186x186/CopperWithSubstrate186x186");

            }
            
            private void ToQuestButtonClick()
            {

            }

            public static SideQuestInfo GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<SideQuestInfo>
                    ("Prefabs/UI/Overlays/QuestOverlay/SideQuestInfo", parent);
            }
        }
    }
}
