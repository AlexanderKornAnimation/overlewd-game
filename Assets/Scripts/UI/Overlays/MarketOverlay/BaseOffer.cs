using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSMarketOverlay
    {
         public abstract class BaseOffer : MonoBehaviour
        {
            public OfferButton offerButton { get; set; }

            protected Transform canvas;

            protected virtual void Awake()
            {
                canvas = transform.Find("Canvas");
            }

            protected virtual void Start()
            {
                Customize();
            }

            protected virtual void Customize()
            {
                
            }

            public virtual void Refresh()
            {

            }
            
            public void Show()
            {
                gameObject.SetActive(true);
            }

            public void Hide()
            {
                gameObject.SetActive(false);
            }
        }
    }
}