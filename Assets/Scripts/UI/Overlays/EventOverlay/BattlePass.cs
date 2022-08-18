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
            private Image progressBar;

            private Transform canvas;

            private void Awake()
            {
                canvas = transform.Find("Canvas");
                var finalRewards = canvas.Find("FinalRewards");
                
                freeFinalReward = finalRewards.Find("FreeReward").GetComponent<Image>();
                premiumFinalReward = finalRewards.Find("PremiumReward").GetComponent<Image>();
                titleFinalReward = finalRewards.Find("Title").GetComponent<TextMeshProUGUI>();

                content = canvas.Find("ScrollView").Find("Viewport").Find("Content");
                progressBar = content.Find("ProgressBar").GetComponent<Image>();
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
            }

            public void SetCanvasActive(bool value)
            {
                if (value != canvas.gameObject.activeSelf)
                {
                    canvas.gameObject.SetActive(value);
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
