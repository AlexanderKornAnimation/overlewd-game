using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSPortalScreen
    {
        public abstract class BaseContent : MonoBehaviour
        {
            protected Button button;
            protected Image background;
            protected Image contentBackground;
            protected TextMeshProUGUI title;
            protected TextMeshProUGUI markers;
            protected TextMeshProUGUI discount;
            
            protected Transform content;
            protected RectTransform rect;
            
            public int gachaId { get; set; }

            public AdminBRO.GachItem gachaData => GameData.gacha.GetGachaById(gachaId);

            protected virtual void Awake()
            {
                var canvas = transform.Find("Canvas");
                
                content = canvas.Find("Content");
                background = button.GetComponent<Image>();
                title = canvas.Find("Title").GetComponent<TextMeshProUGUI>();
            }
        }
    }
}