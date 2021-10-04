using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class EventsWidget : BaseWidget
    {
        void Start()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Widgets/EventsWidget/EventsWidget"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            screenRectTransform.Find("Canvas").Find("Event1").GetComponent<Button>().onClick.AddListener(() => 
            {
                UIManager.ShowOverlay<EventOverlay>();
            });

            screenRectTransform.Find("Canvas").Find("Event2").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowOverlay<EventOverlay>();
            });
        }

        void Update()
        {

        }
    }
}
