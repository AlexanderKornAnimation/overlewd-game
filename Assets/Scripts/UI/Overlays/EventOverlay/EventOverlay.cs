using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class EventOverlay : BaseOverlay
    {
        private Transform scrollViewContent;

        void Start()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Overlays/EventOverlay/EventOverlay"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            screenRectTransform.Find("Canvas").Find("BackButton").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.HideOverlay();
            });

            scrollViewContent = screenRectTransform.Find("Canvas").Find("Scroll View").Find("Viewport").Find("Content");

            NSEventOverlay.Banner.GetInstance(scrollViewContent);
            NSEventOverlay.EventItem.GetInstance(scrollViewContent);
            NSEventOverlay.EventItem.GetInstance(scrollViewContent);
            NSEventOverlay.EventItem.GetInstance(scrollViewContent);
            NSEventOverlay.EventItem.GetInstance(scrollViewContent);
            NSEventOverlay.EventDescription.GetInstance(scrollViewContent);
        }

        void Update()
        {

        }
    }
}
