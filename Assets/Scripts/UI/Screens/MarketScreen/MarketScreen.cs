using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class MarketScreen : BaseScreen
    {

        void Start()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/MarketScreen/Market"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);


            screenRectTransform.Find("Canvas").Find("MainMenuButton").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<CastleScreen>();
            });

            var bundlesGrid = screenRectTransform.Find("Canvas").Find("BottomGrid");
            NSMarketScreen.BundleTypeA.GetInstance(bundlesGrid);
            NSMarketScreen.BundleTypeB.GetInstance(bundlesGrid);
            NSMarketScreen.BundleTypeC.GetInstance(bundlesGrid);
            NSMarketScreen.BundleTypeD.GetInstance(bundlesGrid);
        }

        void Update()
        {

        }
    }
}
