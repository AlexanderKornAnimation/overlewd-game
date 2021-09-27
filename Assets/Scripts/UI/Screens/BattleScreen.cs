using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class BattleScreen : BaseScreen
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
                GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "Battle Screen");

                var rect = new Rect(60, 100, 110, 30);
                if (GUI.Button(rect, "Global Map"))
                {
                    UIManager.ShowScreen<MapScreen>();
                }
            }
            GUI.EndGroup();
        }
    }
}
