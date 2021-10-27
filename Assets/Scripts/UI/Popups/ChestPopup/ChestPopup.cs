using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class ChestPopup : BasePopup
    {
        void Start()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Popups/ChestPopup/ChestPopup"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

        }

        void Update()
        {

        }
    }

}
