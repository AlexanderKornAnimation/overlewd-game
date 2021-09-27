using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class EventMapScreen : BaseScreen
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
                GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "Event Map Screen");

                var rect = new Rect(60, 100, 110, 30);
                if (GUI.Button(rect, "Global Map"))
                {
                    UIManager.ShowScreen<MapScreen>();
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

                rect.y += 35;
                if (GUI.Button(rect, "Prepare Boss Fight"))
                {
                    UIManager.ShowScreen<PrepareBossFightScreen>();
                }
            }
            GUI.EndGroup();
        }
    }
}
