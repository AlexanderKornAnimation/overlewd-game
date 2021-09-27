using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class EventOverlay : BaseOverlay
    {
        void Start()
        {

        }

        void Update()
        {

        }

        void OnGUI()
        {
            GUI.depth = 1;
            GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
            {
                GUI.Box(new Rect(Screen.width - 400, 0, 400, Screen.height), "Events Overlay");

                var rect = new Rect(Screen.width - 130, 10, 110, 30);
                if (GUI.Button(rect, "Back"))
                {
                    UIManager.HideOverlay();
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
