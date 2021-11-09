using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Overlewd
{
    public class OverlayMissclick : BaseMissclick
    {
     
        protected override void OnClick(BaseEventData data)
        {
            if (missClickEnabled) 
            {
                UIManager.HideOverlay();
            }
        }

        public static OverlayMissclick GetInstance(Transform parent)
        {
            var newItem = new GameObject(nameof(OverlayMissclick));
            var screenRectTransform = newItem.AddComponent<RectTransform>();
            screenRectTransform.SetParent(parent, false);
            screenRectTransform.SetAsFirstSibling();
            UIManager.SetStretch(screenRectTransform);
            
            return newItem.AddComponent<OverlayMissclick>();
        }
    }
}
