using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class SidebarButtonWidget : BaseWidget
    {
        private void Awake()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Widgets/SidebarButtonWidget/SidebarButtonWidget"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            screenRectTransform.Find("Canvas").Find("SidebarMenu").GetComponent<Button>().onClick.AddListener(() => 
            {
                if (!UIManager.ShowingOverlay<SidebarMenuOverlay>())
                {
                    UIManager.ShowOverlay<SidebarMenuOverlay>();
                }
                else
                {
                    UIManager.HideOverlay();
                }
            });
        }

        void Update()
        {

        }
    }

}
