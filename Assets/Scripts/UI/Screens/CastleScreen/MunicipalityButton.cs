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

            protected override async void Customize()
            {
                secondsLeft = await GameData.buildings.MunicipalityTimeLeft();
                
                if (secondsLeft > 0)
                {
                    StartCoroutine(StartCollectTimer());
                }
                else
                {
                    collectNotification.text = $"Collect {GameData.buildings.municipalitySettings.moneyPerPeriod} gold";
                }
            }

            private IEnumerator StartCollectTimer()
            {
                var time = TimeTools.TimeToString(new TimeSpan(0, 0, secondsLeft));
                while (!String.IsNullOrEmpty(time))
                {
                    collectNotification.text = $"Collect gold in\n {time}";
                    yield return new WaitForSeconds(1.0f);
                    secondsLeft--;
                    time = TimeTools.TimeToString(new TimeSpan(0, 0, secondsLeft));
                }
                
                Customize();
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