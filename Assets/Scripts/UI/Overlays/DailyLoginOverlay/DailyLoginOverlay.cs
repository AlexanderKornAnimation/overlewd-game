using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class DailyLoginOverlay : BaseOverlayParent<DailyLoginOverlayInData>
    {
        private Image background;
        private TextMeshProUGUI descr;
        private Transform dayTabs;
        private Button closeButton;

        private void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Overlays/DailyLoginOverlay/DailyLoginOverlay", transform);
            
            var canvas = screenInst.transform.Find("Canvas");
            
            background = canvas.Find("Background").GetComponent<Image>();
            descr = canvas.Find("Headline/Description").GetComponent<TextMeshProUGUI>();
            dayTabs = canvas.Find("DayTabs");
            closeButton = canvas.Find("CloseButton").GetComponent<Button>();
            closeButton.onClick.AddListener(CloseButtonClick);

            dayTabs.Find("DayTab1").GetComponent<NSDailyLoginOverlay.DayTab>().dayName = AdminBRO.DailyLogin.Day.DayName_Day1;
            dayTabs.Find("DayTab2").GetComponent<NSDailyLoginOverlay.DayTab>().dayName = AdminBRO.DailyLogin.Day.DayName_Day2;
            dayTabs.Find("DayTab3").GetComponent<NSDailyLoginOverlay.DayTab>().dayName = AdminBRO.DailyLogin.Day.DayName_Day3;
            dayTabs.Find("DayTab4").GetComponent<NSDailyLoginOverlay.DayTab>().dayName = AdminBRO.DailyLogin.Day.DayName_Day4;
            dayTabs.Find("DayTab5").GetComponent<NSDailyLoginOverlay.DayTab>().dayName = AdminBRO.DailyLogin.Day.DayName_Day5;
            dayTabs.Find("DayTab6").GetComponent<NSDailyLoginOverlay.DayTab>().dayName = AdminBRO.DailyLogin.Day.DayName_Day6;
            dayTabs.Find("DayTab7").GetComponent<NSDailyLoginOverlay.DayTab>().dayName = AdminBRO.DailyLogin.Day.DayName_Day7;
            foreach (var dayTab in dayTabs.GetComponentsInChildren<NSDailyLoginOverlay.DayTab>())
            {
                dayTab.dailyLoginOverlay = this;
            }
        }

        public override async Task BeforeShowMakeAsync()
        {
            Customize();
            await Task.CompletedTask;
        }

        private void Customize()
        {
            background.sprite = ResourceManager.LoadSprite(GameData.dailyLogin.info.image);
            descr.text = GameData.dailyLogin.info.bannerDescription;
        }

        private void CloseButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.HideOverlay();
        }

        public void Refresh()
        {
            foreach (var dayTab in dayTabs.GetComponentsInChildren<NSDailyLoginOverlay.DayTab>())
            {
                dayTab.Refresh();
            }
        }
    }

    public class DailyLoginOverlayInData : BaseOverlayInData
    {

    }
}
