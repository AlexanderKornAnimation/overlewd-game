using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class EventsWidget : BaseWidget
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
                var rectBox = new Rect(Screen.width - 145, 40, 140, 110);
                GUI.BeginGroup(rectBox);
                {
                    GUI.Box(new Rect(0, 0, rectBox.width, rectBox.height), "Events");

                    var rectBtn = new Rect(rectBox.width - 125, 30, 110, 30);
                    if (GUI.Button(rectBtn, "Event 1"))
                    {
                        UIManager.ShowOverlay<EventOverlay>();
                    }

                    rectBtn.y += 35;
                    if (GUI.Button(rectBtn, "Event 2"))
                    {
                        UIManager.ShowOverlay<EventOverlay>();
                    }
                }
                GUI.EndGroup();
            }
            GUI.EndGroup();
        }
    }
}
