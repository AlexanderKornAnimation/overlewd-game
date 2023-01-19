using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class WeeklyLoginPopup : BasePopupParent<WeeklyLoginPopupInData>
    {
        private Image background;
        private Transform rewardsGrid;
        private Button closeButton;

        private void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Popups/WeeklyLoginPopup/WeeklyLoginPopup", transform);
            
            var canvas = screenInst.transform.Find("Canvas");
            
            background = canvas.Find("Background").GetComponent<Image>();
            rewardsGrid = canvas.Find("RewardsGrid");
            closeButton = canvas.Find("CloseButton").GetComponent<Button>();
            closeButton.onClick.AddListener(CloseButtonClick);
        }

        private void CloseButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.HidePopup();
        }

        public override async Task BeforeShowMakeAsync()
        {
            Customize();
            await Task.CompletedTask;
        }

        private void Customize()
        {
            var rewards = rewardsGrid.GetComponentsInChildren<NSWeeklyLoginPopup.RewardItem>();

            foreach (var reward in rewards)
            {
                reward.Customize();
            }
        }
    }

    public class WeeklyLoginPopupInData : BasePopupInData
    {
    }
}
