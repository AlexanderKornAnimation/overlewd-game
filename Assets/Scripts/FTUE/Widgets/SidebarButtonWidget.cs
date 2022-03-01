using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace FTUE
    {
        public class SidebarButtonWidget : Overlewd.SidebarButtonWidget
        {
            void Start()
            {
                Customize();
            }

            private void Customize()
            {
                if (GameGlobalStates.castle_SideMenuLock)
                {
                    Lock();
                }
            }

            protected override void SidebarMenuButtonClick()
            {
                if (!UIManager.HasOverlay<SidebarMenuOverlay>())
                {
                    SoundManager.PlayOneShoot(SoundPath.UI_GenericButtonClick);
                    UIManager.ShowOverlay<SidebarMenuOverlay>();
                }
                else
                {
                    UIManager.HideOverlay();
                }
            }

            public void Lock()
            {
                sidebarMenuButton.interactable = false;
                foreach (var cr in GetComponentsInChildren<CanvasRenderer>())
                {
                    cr.SetColor(Color.gray);
                }
            }

            public new static SidebarButtonWidget GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateScreenPrefab<SidebarButtonWidget>
                    ("Prefabs/UI/Widgets/SidebarButtonWidget/SidebarButtonWidget", parent);
            }
        }
    }
}
