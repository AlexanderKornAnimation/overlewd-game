using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSDailyLoginOverlay
    {
        public class DayTab : MonoBehaviour
        {
            public DailyLoginOverlay dailyLoginOverlay { get; set; }
            public string dayName { get; set; }
            public AdminBRO.DailyLogin.Day dayData =>
                GameData.dailyLogin.info.GetDayByName(dayName);

            private GameObject active;
            private GameObject claimed;
            private GameObject upcoming;
            private Image icon;
            private TextMeshProUGUI count;
            private GameObject markClaimed;
            private Button claimButton;

            private void Awake()
            {
                active = transform.Find("Active").gameObject;
                claimed = transform.Find("Claimed").gameObject;
                upcoming = transform.Find("Upcoming").gameObject;
                icon = transform.Find("Icon").GetComponent<Image>();
                count = transform.Find("Icon/Count").GetComponent<TextMeshProUGUI>();
                markClaimed = icon.transform.Find("MarkClaimed").gameObject;
                claimButton = transform.Find("ClaimButton").GetComponent<Button>();
                claimButton.onClick.AddListener(ClaimButtonClick);
            }

            private void Start()
            {
                Customize();
            }

            private async void ClaimButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                await GameData.dailyLogin.Collect(dayName);
                if (GameData.dailyLogin.isValid)
                {
                    dailyLoginOverlay.Refresh();
                }
                else
                {
                    UIManager.HideOverlay();
                }
            }

            public void Customize()
            {
                var reward = dayData.rewards.FirstOrDefault();
                icon.sprite = ResourceManager.LoadSprite(reward?.icon);
                count.text = reward?.amount?.ToString();
                Refresh();
            }

            public void Refresh()
            {
                var _dayData = dayData;
                active.SetActive(!_dayData.isCollected && _dayData.isLoggedIn && _dayData.isCanCollect);
                claimButton.interactable = active.activeSelf;
                claimed.SetActive(_dayData.isCollected);
                markClaimed.SetActive(_dayData.isCollected);
                upcoming.SetActive(!_dayData.isLoggedIn || !_dayData.isCanCollect);
            }
        }
    }
}
