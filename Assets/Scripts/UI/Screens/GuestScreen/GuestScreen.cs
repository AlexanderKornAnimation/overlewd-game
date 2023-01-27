using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class GuestScreen : BaseFullScreenParent<GuestScreenInData>
    {
        private Button backButton;
        private Transform guestsGrid;

        private void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/GuestScreen/GuestScreen", transform);
            var canvas = screenInst.transform.Find("Canvas");

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);
            guestsGrid = canvas.Find("GuestsGrid");
        }

        public override async Task BeforeShowMakeAsync()
        {
            Customize();
            await Task.CompletedTask;
        }

        private void Customize()
        {
            var guests = guestsGrid.GetComponentsInChildren<NSGuestScreen.Guest>();

            var today = DateTime.Today;
            var activeGuest = guests[today.Month - 1];
            activeGuest.girlId = GameData.events.activeMonthly.narratorMatriarchId;
            activeGuest.Customize();
        }

        private void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ToPrevScreen();
        }
    }

    public class GuestScreenInData : BaseFullScreenInData
    {
        
    }
}
