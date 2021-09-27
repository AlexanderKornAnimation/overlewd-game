using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class EventMarketScreen : BaseScreen
    {
        void Start()
        {

        }

        void Update()
        {

        }

        void OnGUI()
        {
            GUI.depth = 2;
            GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
            {
                GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "Event Market Screen");

                var rect = new Rect(Screen.width - 130, 10, 110, 30);
                if (GUI.Button(rect, "Market"))
                {
                    UIManager.ShowScreen<MarketScreen>();
                }
            }
            GUI.EndGroup();
        }
    }
}
