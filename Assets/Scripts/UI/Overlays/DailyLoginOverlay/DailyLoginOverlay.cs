using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class DailyLoginOverlay : BaseOverlayParent<DailyLoginOverlayInData>
    {
        private Image background;
        private Transform rewardsGrid;
        private Button closeButton;

        private void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Overlays/DailyLoginOverlay/DailyLoginOverlay", transform);
            
            var canvas = screenInst.transform.Find("Canvas");
            
            background = canvas.Find("Background").GetComponent<Image>();
            rewardsGrid = canvas.Find("RewardsGrid");
            closeButton = canvas.Find("CloseButton").GetComponent<Button>();
            closeButton.onClick.AddListener(CloseButtonClick);
        }

        private void CloseButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.HideOverlay();
        }

        public override async Task BeforeShowMakeAsync()
        {
            Customize();
            await Task.CompletedTask;
        }

        private void Customize()
        {
            var rewards = rewardsGrid.GetComponentsInChildren<NSDailyLoginOverlay.RewardItem>();

            foreach (var reward in rewards)
            {
                reward.Customize();
            }
        }
    }

    public class DailyLoginOverlayInData : BaseOverlayInData
    {

    }
}
