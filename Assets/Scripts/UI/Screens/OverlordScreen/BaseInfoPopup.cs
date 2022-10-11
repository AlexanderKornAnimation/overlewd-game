using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSOverlordScreen
    {
        public abstract class BaseInfoPopup : MonoBehaviour
        {
            private Button missClickButton;

            protected virtual void Awake()
            {
                missClickButton = transform.Find("MissClickButton").GetComponent<Button>();
                missClickButton.onClick.AddListener(MissClickButtonClick);
            }

            protected virtual void MissClickButtonClick()
            {
                Destroy(gameObject);
            }
        }
    }
}
