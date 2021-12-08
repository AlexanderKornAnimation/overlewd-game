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
            
            public new static SidebarButtonWidget GetInstance(Transform parent)
            {
                var prefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Widgets/SidebarButtonWidget/SidebarButtonWidget"), parent);
                prefab.name = nameof(SidebarButtonWidget);
                var rectTransform = prefab.GetComponent<RectTransform>();
                UIManager.SetStretch(rectTransform);
                return prefab.AddComponent<SidebarButtonWidget>();
            }
        }
    }
}
