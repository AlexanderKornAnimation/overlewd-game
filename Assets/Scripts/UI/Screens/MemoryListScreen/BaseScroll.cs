using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSMemoryListScreen
    {
        public class BaseScroll : MonoBehaviour
        {
            protected Transform content;
            
            protected virtual void Awake()
            {
                content = transform.Find("Viewport").Find("Content");
            }

            public  void Show()
            {
                transform.gameObject.SetActive(true);
            }

            public  void Hide()
            {
                transform.gameObject.SetActive(false);
            }
            
            protected virtual void Start()
            {
                Customize();
            }

            protected virtual void Customize()
            {
                
            }
        }
    }
}