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
        public class BattlePass : MonoBehaviour
        {
            private Image freeFinalReward;
            private Image premiumFinalReward;
            private TextMeshProUGUI points;

            private Transform content;
            private Image progressBar;

            private List<BattlePassLevel> levels = new List<BattlePassLevel>();

            private void Awake()
            {
                var canvas = transform.Find("Canvas");
                var finalRewards = canvas.Find("FinalRewards");
                
                freeFinalReward = finalRewards.Find("FreeReward").GetComponent<Image>();
                premiumFinalReward = finalRewards.Find("PremiumReward").GetComponent<Image>();
                points = finalRewards.GetComponent<TextMeshProUGUI>();
                content = canvas.Find("ScrollView").Find("Viewport").Find("Content");
                progressBar = content.Find("ProgressBar").GetComponent<Image>();
            }

            private void Start()
            {
                Customize();

                StartCoroutine(ContentVisibleOptimize());
            }

            private void Customize()
            {
                for (int i = 0; i <= 20; i++)
                {
                    levels.Add(BattlePassLevel.GetInstance(content));
                }
            }

            private IEnumerator ContentVisibleOptimize()
            {
                while (true)
                {
                    var screenRect = UIManager.GetScreenWorldRect();
                    foreach (var level in levels)
                    {
                        var itemRect = level.transform as RectTransform;
                        level.SetCanvasActive(itemRect.WorldRect().Overlaps(screenRect));
                    }
                    yield return null;
                }
            }
            
            public static BattlePass GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<BattlePass>(
                    "Prefabs/UI/Overlays/EventOverlay/BattlePass", parent);
            }
        }
    }
}
