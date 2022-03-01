using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class SidebarButtonWidget : BaseWidget
    {
        protected Button sidebarMenuButton;

        void Awake()
        {
            var canvas = transform.Find("Canvas");

            sidebarMenuButton = canvas.Find("SidebarMenu").GetComponent<Button>();
            sidebarMenuButton.onClick.AddListener(SidebarMenuButtonClick);
        }

        protected virtual void SidebarMenuButtonClick()
        {
            SoundManager.PlayOneShoot(SoundPath.UI_GenericButtonClick);
            if (!UIManager.HasOverlay<SidebarMenuOverlay>())
            {
                UIManager.ShowOverlay<SidebarMenuOverlay>();
            }
            else
            {
                UIManager.HideOverlay();
            }
        }

        public static SidebarButtonWidget GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateScreenPrefab<SidebarButtonWidget>
                ("Prefabs/UI/Widgets/SidebarButtonWidget/SidebarButtonWidget", parent);
        }
    }

}
