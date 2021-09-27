using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class CastleScreen : BaseScreen
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
                GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "Castle Screen");

                var rect = new Rect(60, 100, 110, 30);
                if (GUI.Button(rect, "Market"))
                {
                    UIManager.ShowScreen<MarketScreen>();
                }

                rect.y += 35;
                if (GUI.Button(rect, "Global Map"))
                {
                    UIManager.ShowScreen<MapScreen>();
                }

                rect.y += 35;
                if (GUI.Button(rect, "Harem"))
                {
                    UIManager.ShowScreen<HaremScreen>();
                }

                rect.y += 35;
                if (GUI.Button(rect, "User Settings"))
                {
                    UIManager.ShowScreen<UserSettingsScreen>();
                }

                rect.y += 35;
                if (GUI.Button(rect, "Inventory"))
                {
                    UIManager.ShowScreen<InventoryAndUserScreen>();
                }

                rect.y += 35;
                if (GUI.Button(rect, "Forge"))
                {
                    UIManager.ShowScreen<ForgeScreen>();
                }

                rect.y += 35;
                if (GUI.Button(rect, "Portal"))
                {
                    UIManager.ShowScreen<PortalScreen>();
                }

                rect.y += 35;
                if (GUI.Button(rect, "Magic Guild"))
                {
                    UIManager.ShowScreen<MagicGuildScreen>();
                }
            }
            GUI.EndGroup();
        }
    }
}
