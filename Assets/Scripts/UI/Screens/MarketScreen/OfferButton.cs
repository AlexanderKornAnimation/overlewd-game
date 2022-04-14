using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class OfferButton : MonoBehaviour
    {
        private TextMeshProUGUI title;
        private TextMeshProUGUI notification;
        private Button button;
        
        private void Awake()
        {
            var canvas = transform.Find("Canvas");

            button = canvas.Find("Button").GetComponent<Button>();
            button.onClick.AddListener(ButtonClick);
            
            title = button.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            notification = button.transform.Find("Notification").GetComponent<TextMeshProUGUI>();
        }

        private void ButtonClick()
        {
            
        }        
        
        public static OfferButton GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateWidgetPrefab<OfferButton>("Prefabs/UI/Screens/MarketScreen/OfferButton", parent);
        }
    }
}
