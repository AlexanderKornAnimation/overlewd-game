using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSCastleScreen
    {
        public abstract class BaseButton : MonoBehaviour
        {
            protected Button button;
            protected TextMeshProUGUI title;

            protected TextMeshProUGUI markers;

            protected virtual void Awake()
            {
                button = transform.Find("Button").GetComponent<Button>();
                title = button.transform.Find("Title").GetComponent<TextMeshProUGUI>();
                markers = transform.Find("Markers").GetComponent<TextMeshProUGUI>();
                button.onClick.AddListener(ButtonClick);
            }

            protected virtual void Start()
            {
                Customize();
            }

            public async Task ShowAsync()
            {
                await UITools.FadeShowAsync(gameObject, 0.7f);
            }

            public void Hide()
            {
                UITools.FadeHide(gameObject);
            }
            
            protected virtual void Customize()
            {
   
            }

            protected virtual void ButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_CastleScreenButtons);
            }
        }
    }
}