using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class MapScreen : BaseScreen
    {
        void Start()
        {
            gameObject.AddComponent<EventsWidget>();
            gameObject.AddComponent<QuestsWidget>();
            gameObject.AddComponent<SidebarButtonWidget>();
        }

        void Update()
        {

        }

        void OnGUI()
        {
            GUI.depth = 3;
            GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
            {
                GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "Map Screen");

                var rect = new Rect(60, 100, 110, 30);
                if (GUI.Button(rect, "Castle"))
                {
                    UIManager.ShowScreen<CastleScreen>();
                }

                rect.y += 35;
                if (GUI.Button(rect, "Dialog"))
                {
                    UIManager.ShowScreen<DialogScreen>();
                }

                rect.y += 35;
                if (GUI.Button(rect, "Sex"))
                {
                    UIManager.ShowScreen<SexScreen>();
                }

                rect.y += 35;
                if (GUI.Button(rect, "Prepare Battle"))
                {
                    UIManager.ShowScreen<PrepareBattleScreen>();
                }

                //Events list
                rect.y += 50;
                if (GUI.Button(rect, "Event 1"))
                {
                    UIManager.ShowScreen<EventMapScreen>();
                }

                rect.y += 35;
                if (GUI.Button(rect, "Event 2"))
                {
                    UIManager.ShowScreen<EventMapScreen>();
                }
            }
            GUI.EndGroup();
        }
    }
}
