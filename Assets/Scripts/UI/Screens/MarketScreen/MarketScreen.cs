using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class MarketScreen : BaseFullScreenParent<MarketScreenInData>
    {
        private Button backButton;

        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/MarketScreen/Market", transform);

            var canvas = screenInst.transform.Find("Canvas");

            backButton = canvas.Find("MainMenuButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);
        }

        
        public override async Task BeforeShowMakeAsync()
        {

            await Task.CompletedTask;
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
