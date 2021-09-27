using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class DialogScreen : BaseScreen
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
                GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "Dialog Screen");

                var rect = new Rect(60, 100, 110, 30);
                if (GUI.Button(rect, "Girl"))
                {
                    UIManager.ShowScreen<GirlScreen>();
                }
            }
            GUI.EndGroup();
        }
    }
}
