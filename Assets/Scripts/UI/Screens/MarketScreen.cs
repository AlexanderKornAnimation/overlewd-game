using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class MarketScreen : BaseScreen
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
                GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "Market Screen");

                var rect = new Rect(Screen.width - 130, 10, 110, 30);
                if (GUI.Button(rect, "Castle"))
                {
                    UIManager.ShowScreen<CastleScreen>();
                }

                rect.y += 35;
                if (GUI.Button(rect, "Event Market"))
                {
                    UIManager.ShowScreen<EventMarketScreen>();
                }
            }
            GUI.EndGroup();
        }
    }
}
