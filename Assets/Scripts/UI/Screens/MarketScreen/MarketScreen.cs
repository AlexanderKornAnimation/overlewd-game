using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class MarketScreen : BaseFullScreenParent<MarketScreenInData>
    {
        private Transform bottomGrid;
        private Button backButton;

        private void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/MarketScreen/Market", transform);

            var canvas = screenInst.transform.Find("Canvas");

            backButton = canvas.Find("MainMenuButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);

            bottomGrid = canvas.Find("BottomGrid");
        }

        private void Start()
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
        
        private void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            
            if (inputData?.prevScreenInData != null)
            {
                if (inputData.prevScreenInData.IsType<MapScreenInData>())
                {
                    UIManager.ShowScreen<MapScreen>();
                }
                else if (inputData.prevScreenInData.IsType<EventMapScreenInData>())
                {
                    UIManager.ShowScreen<EventMapScreen>();
                }
                else
                {
                    UIManager.ShowScreen<CastleScreen>();
                }
            }
            else
            {
                UIManager.ShowScreen<CastleScreen>();
            }
        }
    }

    public class MarketScreenInData : BaseFullScreenInData
    {

    }
}
