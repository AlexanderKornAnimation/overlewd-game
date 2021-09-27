using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class GirlScreen : BaseScreen
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
                GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "Girl Screen");

                var rect = new Rect(60, 100, 110, 30);
                if (GUI.Button(rect, "Harem"))
                {
                    UIManager.ShowScreen<HaremScreen>();
                }

                rect.y += 35;
                if (GUI.Button(rect, "Memory"))
                {
                    UIManager.ShowScreen<MemoryScreen>();
                }

                rect.y += 35;
                if (GUI.Button(rect, "Dialog"))
                {
                    UIManager.ShowScreen<DialogScreen>();
                }

                rect.y += 35;
                if (GUI.Button(rect, "Portal"))
                {
                    UIManager.ShowScreen<PortalScreen>();
                }
            }
            GUI.EndGroup();
        }
    }
}
