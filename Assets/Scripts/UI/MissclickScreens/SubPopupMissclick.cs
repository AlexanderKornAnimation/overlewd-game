using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Overlewd
{
    public class SubPopupMissclick : BaseMissclick
    {
        protected override void OnClick(BaseEventData data)
        {
            if (missClickEnabled)
            {
                UIManager.HideSubPopup();
            }
        }

        public static SubPopupMissclick GetInstance(Transform parent)
        {
            var newItem = new GameObject(nameof(SubPopupMissclick));
            var screenRectTransform = newItem.AddComponent<RectTransform>();
            screenRectTransform.SetParent(parent, false);
            screenRectTransform.SetAsFirstSibling();
            UIManager.SetStretch(screenRectTransform);
            
            return newItem.AddComponent<SubPopupMissclick>();
        }
    }
}
