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
            public Transform content { get; private set; }

            void Awake()
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
                var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/PortalScreen/TierButtonsScroll"), parent);
                newItem.name = nameof(TierButtonsScroll);
                return newItem.AddComponent<TierButtonsScroll>();
            }
        }
    }
}
