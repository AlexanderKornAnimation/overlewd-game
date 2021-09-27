using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class QuestsWidget : BaseWidget
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
                var rectBox = new Rect(Screen.width - 145, Screen.height - 150, 140, 110);
                GUI.BeginGroup(rectBox);
                {
                    GUI.Box(new Rect(0, 0, rectBox.width, rectBox.height), "Quests");

                    var rectBtn = new Rect(rectBox.width - 125, 30, 110, 30);
                    if (GUI.Button(rectBtn, "Quest 1"))
                    {
                        UIManager.ShowOverlay<QuestOverlay>();
                    }

                    rectBtn.y += 35;
                    if (GUI.Button(rectBtn, "Quest 2"))
                    {
                        UIManager.ShowOverlay<QuestOverlay>();
                    }
                }
                GUI.EndGroup();
            }
            GUI.EndGroup();
        }
    }
}
