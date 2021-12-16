using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class BuffWidget : MonoBehaviour
    {
        protected Image icon;
        protected Button button;
        protected TextMeshProUGUI description;
        protected TextMeshProUGUI timer;
        
        private void Awake()
        {
            var canvas = transform.Find("Canvas");
            
            button = canvas.Find("Button").GetComponent<Button>();
            icon = button.transform.Find("Icon").GetComponent<Image>();
            description = button.transform.Find("Description").GetComponent<TextMeshProUGUI>();
            timer = button.transform.Find("Timer").GetComponent<TextMeshProUGUI>();
            
            button.onClick.AddListener(ButtonClick);
        }

        protected virtual void ButtonClick()
        {
            UIManager.ShowScreen<HaremScreen>();
        }
        
        public static BuffWidget GetInstance(Transform parent)
        {
            var prefab = (GameObject) Instantiate(Resources.Load("Prefabs/UI/Widgets/BuffWidget/BuffWidget"), parent);
            prefab.name = nameof(BuffWidget);
            var rectTransform = prefab.GetComponent<RectTransform>();
            UIManager.SetStretch(rectTransform);
            return prefab.AddComponent<BuffWidget>();
        }
    }
}

