using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSBannerNotification
    {
        public abstract class BaseResource : MonoBehaviour
        {
            private Image icon;
            private Text count;

            private void Awake()
            {
                icon = transform.Find("Resource").Find("Icon").GetComponent<Image>();
                count = transform.Find("Resource").Find("Count").GetComponent<Text>();
            }

            public void SetCount(int value)
            {
                count.text = value.ToString();
            }

            public void SetIcon(string path)
            {
                icon.sprite = Resources.Load<Sprite>(path);
            }
        }
    }
}