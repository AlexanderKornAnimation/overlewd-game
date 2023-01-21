using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Overlewd
{
    namespace NSEventOverlay
    {
        public class BattlePass : MonoBehaviour
        {
            public int eventId { get; set; }
            public AdminBRO.BattlePass passData =>
                GameData.battlePass.GetByEventId(eventId);

            private Image freeFinalReward;
            private Image premiumFinalReward;
            private TextMeshProUGUI titleFinalReward;

            private Transform content;
            private RectTransform progressBarRect;

            private Button claimAllButton;
            private Button upgradeButton;

            private Transform canvas;

            void Awake()
            {
                canvas = transform.Find("Canvas");
                var finalRewards = canvas.Find("FinalRewards");
                
                freeFinalReward = finalRewards.Find("FreeReward").GetComponent<Image>();
                premiumFinalReward = finalRewards.Find("PremiumReward").GetComponent<Image>();
                titleFinalReward = finalRewards.Find("Title").GetComponent<TextMeshProUGUI>();

                content = canvas.Find("ScrollView").Find("Viewport").Find("Content");
                progressBarRect = content.Find("ProgressBar").GetComponent<RectTransform>();
                claimAllButton = canvas.Find("ClaimAllButton").GetComponent<Button>();
                claimAllButton.onClick.AddListener(ClaimAllButtonClick);
                upgradeButton = canvas.Find("UpgradeButton").GetComponent<Button>();
                upgradeButton.onClick.AddListener(UpgradeButtonClick);
            }

            void Start()
            {
                for (int levelId = 0; levelId < passData.levels.Count - 1; levelId++)
                {
                    var newLevel = BattlePassLevel.GetInstance(content);
                    newLevel.battlePassId = passData.id;
                    newLevel.levelId = levelId;
                }

                Customize();
            }

            private void Customize()
            {
                var finalLevel = passData.levels.Last();
                freeFinalReward.sprite = ResourceManager.LoadSprite(finalLevel?.defaultReward?.First()?.icon);
                premiumFinalReward.sprite = ResourceManager.LoadSprite(finalLevel?.premiumReward?.First()?.icon);
                titleFinalReward.text = $"Reach {finalLevel?.pointsThreshold} points to get final reward!";
            }

            private void Refresh()
            {
                foreach (var level in content.GetComponentsInChildren<BattlePassLevel>())
                {
                    level.Refresh();
                }
            }

            public void SetCanvasActive(bool value)
            {
                if (value != canvas.gameObject.activeSelf)
                {
                    canvas.gameObject.SetActive(value);
                }
            }

            private void Update()
            {
                if (!canvas.gameObject.activeSelf)
                    return;

                CalcProgressBarWidth();
            }

            private void CalcProgressBarWidth()
            {
                var levels = content.GetComponentsInChildren<BattlePassLevel>();
                var scrollContentWorldRT = content.GetComponent<RectTransform>().WorldRect();

                var content_hlg = content.GetComponent<HorizontalLayoutGroup>();
                content_hlg.enabled = false;
                content_hlg.enabled = true;

                var progressBarWidth = 0.0f;
                if (levels.Last().reached)
                {
                    progressBarWidth = 50.0f + scrollContentWorldRT.width + 50.0f;
                }
                else
                {
                    foreach (var level in levels)
                    {
                        if (level.reached)
                        {
                            var reachedImgWorldRect = level.levelReached.WorldRect();
                            progressBarWidth = 50.0f + (reachedImgWorldRect.center.x - scrollContentWorldRT.xMin);
                            continue;
                        }
                        break;
                    }
                }
                progressBarRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, progressBarWidth);
            }

            private async void ClaimAllButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                if (passData != null)
                {
                    await GameData.battlePass.ClaimRewards(passData.id);
                    Refresh();
                }
            }

            private void UpgradeButtonClick()
            {

            }

            public static BattlePass GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<BattlePass>(
                    "Prefabs/UI/Overlays/EventOverlay/BattlePass", parent);
            }
        }
    }
}
