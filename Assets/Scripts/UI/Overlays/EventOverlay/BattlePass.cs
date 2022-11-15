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

            private Transform canvas;

            private void Awake()
            {
                canvas = transform.Find("Canvas");
                var finalRewards = canvas.Find("FinalRewards");
                
                freeFinalReward = finalRewards.Find("FreeReward").GetComponent<Image>();
                premiumFinalReward = finalRewards.Find("PremiumReward").GetComponent<Image>();
                titleFinalReward = finalRewards.Find("Title").GetComponent<TextMeshProUGUI>();

                content = canvas.Find("ScrollView").Find("Viewport").Find("Content");
                progressBarRect = content.Find("ProgressBar").GetComponent<RectTransform>();
            }

            private void Start()
            {
                Customize();
            }

            private void Customize()
            {
                var data = passData;
                foreach (var level in data.levels.GetRange(0, data.levels.Count - 1))
                {
                    var newLevel = BattlePassLevel.GetInstance(content);
                    newLevel.battlePassId = data.id;
                    newLevel.levelData = level;
                }

                var finalLevel = data.levels.Last();
                freeFinalReward.sprite = ResourceManager.LoadSprite(finalLevel?.defaultReward?.First()?.icon);
                premiumFinalReward.sprite = ResourceManager.LoadSprite(finalLevel?.premiumReward?.First()?.icon);
                titleFinalReward.text = $"Reach {finalLevel?.pointsThreshold} points to get final reward!";

                StartCoroutine(CalcProgressBarWidth());
            }

            public void SetCanvasActive(bool value)
            {
                if (value != canvas.gameObject.activeSelf)
                {
                    canvas.gameObject.SetActive(value);
                }
            }

            private IEnumerator CalcProgressBarWidth()
            {
                for (int i = 0; i < 2; i++)
                {
                    yield return new WaitForEndOfFrame();
                }

                var levels = content.GetComponentsInChildren<BattlePassLevel>();
                var scrollContentRT = content.GetComponent<RectTransform>();
                var progressBarWidth = 0.0f;
                for (int levelId = 0; levelId < levels.Length; levelId++)
                {
                    var level = levels[levelId];
                    if (level.reached)
                    {
                        if (levelId == (levels.Length - 1))
                        {
                            progressBarWidth = scrollContentRT.rect.width + scrollContentRT.sizeDelta.x;
                        }
                        else
                        {
                            progressBarWidth = 40.0f //offset left
                                + (404.0f + 25.0f) * levelId // (itemWidth + spacing)
                                + 191.0f; // half itemWidth to center of levelBack pivot
                        }
                        
                    }
                }
                progressBarRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, progressBarWidth);
            }

            public static BattlePass GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<BattlePass>(
                    "Prefabs/UI/Overlays/EventOverlay/BattlePass", parent);
            }
        }
    }
}
