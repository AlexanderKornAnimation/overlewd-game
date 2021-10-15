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
            transform.Find("Canvas").Find("SidebarMenu").GetComponent<Button>().onClick.AddListener(() => 
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

        public static SidebarButtonWidget CreateInstance(Transform parent)
        {
            var prefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Widgets/SidebarButtonWidget/SidebarButtonWidget"));
            prefab.name = nameof(SidebarButtonWidget);
            var rectTransform = prefab.GetComponent<RectTransform>();
            rectTransform.SetParent(parent, false);
            UIManager.SetStretch(rectTransform);
            return prefab.AddComponent<SidebarButtonWidget>();
        }
    }

}
