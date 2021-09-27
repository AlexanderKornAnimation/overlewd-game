using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class QuestOverlay : BaseOverlay
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
                GUI.Box(new Rect(Screen.width - 400, 0, 400, Screen.height), "Quest Overlay");

                var rect = new Rect(Screen.width - 130, 10, 110, 30);
                if (GUI.Button(rect, "Back"))
                {
                    UIManager.HideOverlay();
                }
            }
            GUI.EndGroup();
        }
    }
}
