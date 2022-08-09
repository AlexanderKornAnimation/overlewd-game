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
            public int eventId { get; set; }
            public AdminBRO.BattlePass passData =>
                GameData.battlePass.GetByEventId(eventId);

            private Image freeFinalReward;
            private Image premiumFinalReward;
            private TextMeshProUGUI points;

            private Transform content;
            private Image progressBar;

            private Transform canvas;

            private void Awake()
            {
                canvas = transform.Find("Canvas");
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
            }

            private void Customize()
            {
                var data = passData;
                foreach (var level in data.levels)
                {
                    var newLevel = BattlePassLevel.GetInstance(content);
                    newLevel.battlePassId = data.id;
                    newLevel.levelData = level;
                }
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
