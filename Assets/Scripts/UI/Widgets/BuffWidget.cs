using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class BuffWidget : MonoBehaviour
    {
        private Image icon;
        private Button button;
        private Text description;
        private Text timer;
        
        private void Awake()
        {
            var canvas = transform.Find("Canvas");
            
            button = canvas.Find("Button").GetComponent<Button>();
            icon = button.transform.Find("Icon").GetComponent<Image>();
            description = button.transform.Find("Description").GetComponent<Text>();
            timer = button.transform.Find("Timer").GetComponent<Text>();
            
            button.onClick.AddListener(ButtonClick);
        }

        private void ButtonClick()
        {
            UIManager.ShowScreen<HaremScreen>();
        }
        
        public static BuffWidget CreateInstance(Transform parent)
        {
            var prefab = (GameObject) Instantiate(Resources.Load("Prefabs/UI/Widgets/BuffWidget/BuffWidget"));
            prefab.name = nameof(BuffWidget);
            var rectTransform = prefab.GetComponent<RectTransform>();
            rectTransform.SetParent(parent, false);
            UIManager.SetStretch(rectTransform);
            return prefab.AddComponent<BuffWidget>();
        }
    }
}

