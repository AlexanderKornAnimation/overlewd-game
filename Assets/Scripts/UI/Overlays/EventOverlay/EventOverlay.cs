using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class EventOverlay : BaseOverlay
    {
        private Transform[] scrollView = new Transform[5];
        private Transform[] scrollViewContent = new Transform[5];
        private Transform[] eventButton = new Transform[5];

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

            for (int i = 0; i < 5; i++)
            {
                scrollView[i] = screenRectTransform.Find("Canvas").Find("ScrollView" + (i + 1).ToString());
                scrollViewContent[i] = scrollView[i].Find("Viewport").Find("Content");
                eventButton[i] = screenRectTransform.Find("Canvas").Find("EventButton" + (i + 1).ToString());

                var tabId = i;
                eventButton[i].GetComponent<Button>().onClick.AddListener(() =>
                {
                    TabClick(tabId);
                });
            }

            NSEventOverlay.Banner.GetInstance(scrollViewContent[0]);
            NSEventOverlay.EventItem.GetInstance(scrollViewContent[0]);
            NSEventOverlay.EventItem.GetInstance(scrollViewContent[0]);
            NSEventOverlay.EventItem.GetInstance(scrollViewContent[0]);
            NSEventOverlay.EventItem.GetInstance(scrollViewContent[0]);
            NSEventOverlay.EventDescription.GetInstance(scrollViewContent[0]);

            NSEventOverlay.ComingEvent.GetInstance(scrollViewContent[4]);

            TabClick(0);
        }

        private void TabClick(int tabId)
        {
            for (int i = 0; i < 5; i++)
            {
                scrollView[i].gameObject.SetActive(i == tabId);
            }
        }

        void Update()
        {

        }
    }
}
