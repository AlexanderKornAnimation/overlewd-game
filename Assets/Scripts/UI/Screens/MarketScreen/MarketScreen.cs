using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class MarketScreen : BaseScreen
    {
        private Transform bottomGrid;
        private Button mainMenuButton;

        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/MarketScreen/Market", transform);

            var canvas = screenInst.transform.Find("Canvas");

            mainMenuButton = canvas.Find("MainMenuButton").GetComponent<Button>();
            mainMenuButton.onClick.AddListener(MainMenuButtonClick);

            bottomGrid = canvas.Find("BottomGrid");
        }

        void Start()
        {
            Customize();
        }

        private void Customize()
        {
            NSMarketScreen.BundleTypeA.GetInstance(bottomGrid);
            NSMarketScreen.BundleTypeB.GetInstance(bottomGrid);
            NSMarketScreen.BundleTypeC.GetInstance(bottomGrid);
            NSMarketScreen.BundleTypeD.GetInstance(bottomGrid);
        }

        private void MainMenuButtonClick()
        {
            SoundManager.PlayOneShoot(SoundManager.SoundPath.UI.ButtonClick);
            UIManager.ShowScreen<CastleScreen>();
        }
    }
}
