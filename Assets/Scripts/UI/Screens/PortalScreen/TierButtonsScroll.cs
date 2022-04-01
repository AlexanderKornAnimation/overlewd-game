using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSPortalScreen
    {
        public class TierButtonsScroll : MonoBehaviour
        {
            public Transform content { get; protected set; }

            protected virtual void Awake()
            {
                content = transform.Find("Viewport").Find("Content");
                Hide();
            }

            public void Show()
            {
                gameObject.SetActive(true);
            }

            public void Hide()
            {
                gameObject.SetActive(false);
            }

            public static TierButtonsScroll GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<TierButtonsScroll>
                    ("Prefabs/UI/Screens/PortalScreen/TierButtonsScroll", parent);
            }
        }
    }
}
