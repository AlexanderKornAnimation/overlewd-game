using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Overlewd
{
    public class PopupMissclick : BaseMissclick
    {
        protected override void OnClick(BaseEventData data)
        {
            if (missClickEnabled)
            {
                UIManager.HidePopup();
            }
        }

        public static PopupMissclick GetInstance(Transform parent)
        {
            var newItem = new GameObject(nameof(PopupMissclick));
            var screenRectTransform = newItem.AddComponent<RectTransform>();
            screenRectTransform.SetParent(parent, false);
            screenRectTransform.SetAsFirstSibling();
            UIManager.SetStretch(screenRectTransform);
            
            return newItem.AddComponent<PopupMissclick>();
        }
    }
}
