using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSMemoryListScreen
    {
        public class BaseMemory : MonoBehaviour
        {
            protected Transform canvas;
            protected Button button;
            protected Image art;

            protected virtual void Awake()
            {
                canvas = transform.Find("Canvas");
                
                button = canvas.Find("Button").GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);
                art = button.transform.Find("Art").GetComponent<Image>();
            }

            protected virtual void ButtonClick()
            {
                
            }
        }
    }
}