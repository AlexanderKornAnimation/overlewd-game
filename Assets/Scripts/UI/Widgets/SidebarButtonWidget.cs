using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class SidebarButtonWidget : BaseWidget
    {
        public SidebarMenuOverayInData sidebarOverlayInData { get; set; }

        protected Button sidebarMenuButton;

        void Awake()
        {
            var canvas = transform.Find("Canvas");

            sidebarMenuButton = canvas.Find("SidebarMenu").GetComponent<Button>();
            sidebarMenuButton.onClick.AddListener(SidebarMenuButtonClick);
        }

        public void DisableButton()
        {
            UITools.DisableButton(sidebarMenuButton);
        }

        protected virtual void SidebarMenuButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            if (!UIManager.HasOverlay<SidebarMenuOverlay>())
            {
                if (sidebarOverlayInData != null)
                {
                    UIManager.MakeOverlay<SidebarMenuOverlay>().
                        SetData(sidebarOverlayInData).RunShowOverlayProcess();
                }
                else
                {
                    UIManager.ShowOverlay<SidebarMenuOverlay>();
                }
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
