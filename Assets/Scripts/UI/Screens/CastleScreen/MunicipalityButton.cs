using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Overlewd
{
    namespace NSCastleScreen
    {
        public class MunicipalityButton : BaseButton
        {
            private TextMeshProUGUI collectNotification;
            private int secondsLeft;

            protected override void Awake()
            {
                base.Awake();

                collectNotification = transform.Find("CollectNotification").Find("Text").GetComponent<TextMeshProUGUI>();
            }

            protected override void Start()
            {
                base.Start();
                StartCoroutine(GameData.buildings.municipality.TimeLeftLocalUpd(NotifTitleCustomize));
            }

            protected override void Customize()
            {
                NotifTitleCustomize();
            }

            private void NotifTitleCustomize()
            {
                var timeLeftMs = GameData.buildings.municipality.goldAccTimeLeftMs;
                if (timeLeftMs > 0.0f)
                {
                    var timeLeftStr = TimeTools.TimeToString(TimeSpan.FromMilliseconds(timeLeftMs));
                    collectNotification.text = $"Collect gold in\n {timeLeftStr}";
                }
                else
                {
                    collectNotification.text = $"Collect {GameData.buildings.municipality.settings.moneyPerPeriod} gold";
                }
            }
            
            protected override void ButtonClick()
            {
                base.ButtonClick();
                UIManager.ShowScreen<MunicipalityScreen>();
            }
            
            public static MunicipalityButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<MunicipalityButton>
                    ("Prefabs/UI/Screens/CastleScreen/MunicipalityButton", parent);
            }
        }
    }
}