using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSMatriarchMemoryListScreen
    {
        public class MemoryBannerWithShards : BaseMemoryBanner
        {
            private Button activateButton;
            private TextMeshProUGUI timer;
            private TextMeshProUGUI eventMarker;
            private TextMeshProUGUI eventName;

            private TextMeshProUGUI heroicShardsAmount;
            private TextMeshProUGUI epicShardsAmount;
            private TextMeshProUGUI advancedShardsAmount;
            private TextMeshProUGUI basicShardsAmount;

            protected override void Awake()
            {
                base.Awake();
                
                activateButton = closed.transform.Find("ActivateButton").GetComponent<Button>();
                activateButton.onClick.AddListener(ActivateButtonClick);

                var shardsBack = closed.transform.Find("ShardsBack");
                heroicShardsAmount = shardsBack.Find("ShardHeroic/Count").GetComponent<TextMeshProUGUI>();
                epicShardsAmount = shardsBack.Find("ShardEpic/Count").GetComponent<TextMeshProUGUI>();
                advancedShardsAmount = shardsBack.Find("ShardAdvanced/Count").GetComponent<TextMeshProUGUI>();
                basicShardsAmount = shardsBack.Find("ShardBasic/Count").GetComponent<TextMeshProUGUI>();

                timer = canvas.Find("TimerBack/Timer").GetComponent<TextMeshProUGUI>();
                eventName = canvas.Find("EventName").GetComponent<TextMeshProUGUI>();
                eventMarker = eventName.transform.Find("EventMarker").GetComponent<TextMeshProUGUI>();
            }

            protected override void Customize()
            {
                base.Customize();

                var purchasedHeroicShards = memoryData?.heroicPieces.Count(p => p.isPurchased);
                var purchasedEpicShards = memoryData?.heroicPieces.Count(p => p.isPurchased);
                var purchasedAdvancedShards = memoryData?.heroicPieces.Count(p => p.isPurchased);
                var purchasedBasicShards = memoryData?.heroicPieces.Count(p => p.isPurchased);
                
                heroicShardsAmount.text = $"{purchasedHeroicShards}/{memoryData?.heroicPieces?.Count}";
                epicShardsAmount.text = $"{purchasedEpicShards}/{memoryData?.epicPieces?.Count}";
                advancedShardsAmount.text = $"{purchasedAdvancedShards}/{memoryData?.advancedPieces?.Count}";
                basicShardsAmount.text = $"{purchasedBasicShards}/{memoryData?.basicPieces?.Count}";

                var eventData = GameData.events.GetEventById(memoryData?.eventId);
                eventName.text = eventData?.name;
                var periodLeft = TimeTools.AvailableTimeToString(eventData?.timePeriodLeft);
                timer.text = UITools.IncNumberSizeTo(periodLeft, 50);
            }

            private void ActivateButtonClick()
            {
                UIManager.MakeScreen<MemoryScreen>().
                    SetData(new MemoryScreenInData
                    {
                        memoryId = memoryId,
                        girlKey = memoryData?.matriarchData.key,
                    }).DoShow();
            }

            public static MemoryBannerWithShards GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<MemoryBannerWithShards>(
                    "Prefabs/UI/Screens/MatriarchMemoryListScreen/MemoryBannerWithShards", parent);
            }
        }
    }
}
