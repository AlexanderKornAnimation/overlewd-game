using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class VictoryPopup : BasePopup
    {
        void Start()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Popups/VictoryPopup/VictoryPopup"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            screenRectTransform.Find("Canvas").Find("NextButton").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.HideOverlay();
            });
        }

        void Update()
        {

        }
    }
}
