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
        public abstract class BaseTab : MonoBehaviour
        {
            protected Button button;
            protected Image bannerBackground;
            protected Image contentBackground;
            protected TextMeshProUGUI title;
            protected TextMeshProUGUI markers;
            protected TextMeshProUGUI discount;
            
            
            protected Transform content;
            protected RectTransform rect;

            public int gachaId { get; set; }
            public AdminBRO.GachItem gachaData =>
                GameData.gacha.GetGachaById(gachaId);
            
            public event Action<BaseTab> selectTab;

            protected virtual void Awake()
            {
                var canvas = transform.Find("Canvas");
                
                content = canvas.Find("Content");
                button = canvas.Find("Button").GetComponent<Button>();
                button.onClick.AddListener(TabClick);
                bannerBackground = button.GetComponent<Image>();
                contentBackground = content.Find("Background").GetComponent<Image>();
                title = canvas.Find("Title").GetComponent<TextMeshProUGUI>();
                markers = canvas.transform.Find("Markers").GetComponent<TextMeshProUGUI>();
                rect = GetComponent<RectTransform>();
                Deselect();
            }

            protected virtual void TabClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                selectTab?.Invoke(this);
            }

            public void MoveLeft()
            {
                const float offsetX = 586f;
                var pos = rect.anchoredPosition;
                rect.anchoredPosition = new Vector2(pos.x - offsetX, pos.y);
            }

            public void MoveRight()
            {
                const float offsetX = 586f;
                var pos = rect.anchoredPosition;
                rect.anchoredPosition = new Vector2(pos.x + offsetX, pos.y);
            }
            
            public virtual void Select()
            {
                content.gameObject.SetActive(true);
            }

            public virtual void Deselect()
            {
                content.gameObject.SetActive(false);
            }
        }
    }
}