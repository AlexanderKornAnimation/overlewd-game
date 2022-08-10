using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSEventOverlay
    {
        public class BattlePassLevel : MonoBehaviour
        {
            public int battlePassId { get; set; }
            public AdminBRO.BattlePass passData =>
                GameData.battlePass.GetById(battlePassId);
            public AdminBRO.BattlePass.Level levelData { get; set; }

            private TextMeshProUGUI level;
            private GameObject levelReached;

            private Image[] freeRewards = new Image[2];
            private TextMeshProUGUI[] freeRewardsAmounts = new TextMeshProUGUI[2];
            private Button freeClaimButton;

            private Image[] premRewards = new Image[2];
            private TextMeshProUGUI[] premRewardsAmounts = new TextMeshProUGUI[2];
            private Button premClaimButton;

            private Button claimAllButton;
            private Button upgradeButton;

            private Transform canvas;
            
            private void Awake()
            {
                canvas = transform.Find("Canvas");
                var levelBackground = canvas.Find("LevelBackground");
                var freeRewardsTr = canvas.Find("FreeRewards");
                var premiumRewardsTr = canvas.Find("PremiumRewards");
                
                level = levelBackground.Find("Level").GetComponent<TextMeshProUGUI>();
                levelReached = levelBackground.Find("LevelReached").gameObject;
                freeClaimButton = freeRewardsTr.Find("ClaimButton").GetComponent<Button>();
                premClaimButton = premiumRewardsTr.Find("ClaimButton").GetComponent<Button>();
                claimAllButton = canvas.Find("ClaimAllButton").GetComponent<Button>();
                upgradeButton = canvas.Find("UpgradeButton").GetComponent<Button>();

                for (int i = 0; i < freeRewards.Length; i++)
                {
                    freeRewards[i] = freeRewardsTr.Find($"Reward{i + 1}").GetComponent<Image>();
                    freeRewardsAmounts[i] = freeRewards[i].transform.Find("Count").GetComponent<TextMeshProUGUI>();
                    freeRewards[i].gameObject.SetActive(false);

                    premRewards[i] = premiumRewardsTr.Find($"Reward{i + 1}").GetComponent<Image>();
                    premRewardsAmounts[i] = premRewards[i].transform.Find("Count").GetComponent<TextMeshProUGUI>();
                    premRewards[i].gameObject.SetActive(false);
                }
            }

            private void Start()
            {
                Customize();
            }

            public void Customize()
            {
                var battlePassData = passData;

                level.text = levelData.pointsThreshold.ToString();
                levelReached.SetActive(levelData.pointsThreshold <= battlePassData.currentPointsCount);

                for (int i = 0; i < 2; i++)
                {
                    if (i < levelData.defaultReward.Count)
                    {
                        freeRewards[i].gameObject.SetActive(true);
                        freeRewards[i].sprite = ResourceManager.LoadSprite(levelData.defaultReward[i].icon);
                        freeRewardsAmounts[i].text = levelData.defaultReward[i].amount?.ToString();
                    }

                    if (i < levelData.premiumReward.Count)
                    {
                        premRewards[i].gameObject.SetActive(true);
                        premRewards[i].sprite = ResourceManager.LoadSprite(levelData.premiumReward[i].icon);
                        premRewardsAmounts[i].text = levelData.premiumReward[i].amount?.ToString();
                    }
                }
            }
            
            private void FreeClaimButtonClick()
            {
                
            }

            private void PremClaimButtonClick()
            {
                
            }

            private void ClaimAllButtonClick()
            {
                
            }

            private void UpgradeButtonClick()
            {
                
            }
            
            public static BattlePassLevel GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<BattlePassLevel>(
                    "Prefabs/UI/Overlays/EventOverlay/BattlePassLevel", parent);
            }
        }
    }
}