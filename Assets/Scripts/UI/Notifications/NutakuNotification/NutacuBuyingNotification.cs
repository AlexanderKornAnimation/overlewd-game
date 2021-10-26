using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class NutacuBuyingNotification : BaseNotification
    {
        void Start()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Notifications/NutakuNotification/BuyingNotification"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            screenRectTransform.Find("Canvas").Find("BuyButton").GetComponent<Button>().onClick.AddListener(() => 
            {
                UIManager.HideNotification();
            });
        }

        void Update()
        {

        }
    }
}
