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
                //UITools.DisableButton(sidebarMenuButton);
            }

            protected override void SidebarMenuButtonClick()
            {
                if (!UIManager.HasOverlay<SidebarMenuOverlay>())
                {
                    SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                    UIManager.ShowOverlay<SidebarMenuOverlay>();
                }
                else
                {
                    UIManager.HideOverlay();
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
