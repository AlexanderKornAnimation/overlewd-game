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
            public int levelId { get; set; }
            public AdminBRO.BattlePass.Level levelData =>
                passData.levels[levelId];

            public bool reached =>
                levelData.pointsThreshold <= passData.currentPointsCount;

            private Transform freeRewards;
            private Transform premiumRewards;
            private TextMeshProUGUI level;
            public RectTransform levelReached { get; private set; }

            void Awake()
            {
                var canvas = transform.Find("Canvas");
                var levelBackground = canvas.Find("LevelBackground");
                freeRewards = canvas.Find("FreeRewards");
                premiumRewards = canvas.Find("PremiumRewards");
                level = levelBackground.Find("Level").GetComponent<TextMeshProUGUI>();
                levelReached = levelBackground.Find("LevelReached") as RectTransform;
            }

            void Start()
            {
                foreach (var rData in levelData.defaultReward)
                {
                    var reward = Reward.GetInstance(freeRewards);
                    reward.icon = ResourceManager.LoadSprite(rData.icon);
                    reward.amount = rData.amount.Value;
                }
                foreach (var rData in levelData.premiumReward)
                {
                    var reward = Reward.GetInstance(premiumRewards);
                    reward.icon = ResourceManager.LoadSprite(rData.icon);
                    reward.amount = rData.amount.Value;
                }

                Customize();
            }

            public void Customize()
            {
                level.text = levelData.pointsThreshold.ToString();
                levelReached.gameObject.SetActive(reached);

                SetRewardMarks();
            }

            private void SetRewardMarks()
            {
                foreach (var r in freeRewards.GetComponentsInChildren<Reward>())
                {
                    r.isDone = levelData.isDefaultRewardClaimed;
                    r.isLocked = false;
                }
                foreach (var r in premiumRewards.GetComponentsInChildren<Reward>())
                {
                    r.isDone = levelData.isPremiumRewardClaimed;
                    r.isLocked = !passData.isPremium;
                }
            }

            public void Refresh()
            {
                SetRewardMarks();
            }
            
            public static BattlePassLevel GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<BattlePassLevel>(
                    "Prefabs/UI/Overlays/EventOverlay/BattlePassLevel", parent);
            }

            private class Reward : MonoBehaviour
            {
                public Sprite icon
                {
                    get => transform.GetComponent<Image>().sprite;
                    set => transform.GetComponent<Image>().sprite = value;
                }
                public int amount
                {
                    get => int.Parse(transform.Find("Count").GetComponent<TextMeshProUGUI>().text);
                    set => transform.Find("Count").GetComponent<TextMeshProUGUI>().text = value.ToString();
                }
                public bool isDone
                {
                    get => transform.Find("MarkDone").gameObject.activeSelf;
                    set => transform.Find("MarkDone").gameObject.SetActive(value);
                }
                public bool isLocked
                {
                    get => transform.Find("MarkLock").gameObject.activeSelf;
                    set => transform.Find("MarkLock").gameObject.SetActive(value);
                }

                public static Reward GetInstance(Transform parent)
                {
                    return ResourceManager.InstantiateWidgetPrefab<Reward>(
                        "Prefabs/UI/Overlays/EventOverlay/BattlePassReward", parent);
                }
            }
        }
    }
}