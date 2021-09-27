using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class HaremScreen : BaseScreen
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
                GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "Harem Screen");

                var rect = new Rect(60, 100, 110, 30);
                if (GUI.Button(rect, "Castle"))
                {
                    UIManager.ShowScreen<CastleScreen>();
                }

                rect.y += 35;
                if (GUI.Button(rect, "Girl 1"))
                {
                    UIManager.ShowScreen<GirlScreen>();
                }

                rect.y += 35;
                if (GUI.Button(rect, "Girl 2"))
                {
                    UIManager.ShowScreen<GirlScreen>();
                }
            }
            GUI.EndGroup();
        }
    }
}
