using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class SidebarButtonWidget : BaseWidget
    {
        protected Button sidebarMenuButton;

        private void Awake()
        {
            var canvas = transform.Find("Canvas");

            sidebarMenuButton = canvas.Find("SidebarMenu").GetComponent<Button>();
            sidebarMenuButton.onClick.AddListener(SidebarMenuButtonClick);
        }

        protected virtual void SidebarMenuButtonClick()
        {
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
            var prefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Widgets/SidebarButtonWidget/SidebarButtonWidget"), parent);
            prefab.name = nameof(SidebarButtonWidget);
            var rectTransform = prefab.GetComponent<RectTransform>();
            UIManager.SetStretch(rectTransform);
            return prefab.AddComponent<SidebarButtonWidget>();
        }
    }

}
