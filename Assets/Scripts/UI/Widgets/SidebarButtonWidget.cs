using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class SidebarButtonWidget : BaseWidget
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
            var rect = new Rect(10, 10, 110, 30);
            if (GUI.Button(rect, "Sidebar Menu"))
            {
                if (!UIManager.ShowingOverlay<SidebarMenuOverlay>())
                {
                    UIManager.ShowOverlay<SidebarMenuOverlay>();
                }
                else
                {
                    UIManager.HideOverlay();
                }
            }
        }
    }

}
