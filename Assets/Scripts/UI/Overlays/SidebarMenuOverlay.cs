using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class SidebarMenuOverlay : BaseOverlay
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
                GUI.Box(new Rect(Screen.width - 400, 0, 400, Screen.height), "Sidebar Menu Overlay");

                var rect = new Rect(Screen.width - 130, 10, 110, 30);
                if (GUI.Button(rect, "Global Map"))
                {
                    UIManager.ShowScreen<MapScreen>();
                }

                rect.y += 35;
                if (GUI.Button(rect, "Castle"))
                {
                    UIManager.ShowScreen<CastleScreen>();
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
