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
        public abstract class BaseContent : BaseWidget
        {
            protected Image contentBackground;
            protected TextMeshProUGUI title;
            protected TextMeshProUGUI markers;
            protected TextMeshProUGUI discount;
            
            protected Transform canvas;
            protected RectTransform rect;
            
            public int gachaId { get; set; }

            public AdminBRO.GachaItem gachaData => GameData.gacha.GetGachaById(gachaId);

            protected virtual void Awake()
            {
                canvas = transform.Find("Canvas");
                title = canvas.Find("Title").GetComponent<TextMeshProUGUI>();

                UIManager.widgetsGameDataListeners += OnGameDataEvent;
            }

            protected virtual void Start()
            {
                Customize();
            }

            public virtual void Customize()
            {

            }
        }
    }
}