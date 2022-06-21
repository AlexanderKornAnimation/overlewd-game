using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class DevWidget : BaseWidget
    {
        private Button button;

        private void Awake()
        {
            var canvas = transform.Find("Canvas");
            button = canvas.Find("Button").GetComponent<Button>();
            button.onClick.AddListener(ButtonClick);
        }

        private void ButtonClick()
        {
            UIManager.ShowOverlay<DevOverlay>();
        }

        public static DevWidget GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateWidgetPrefab<DevWidget>("Prefabs/UI/Widgets/DevWidget/DevWidget", parent);
        }
    }
}