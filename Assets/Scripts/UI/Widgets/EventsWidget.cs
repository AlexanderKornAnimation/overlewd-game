using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class EventsWidget : BaseWidget
    {
        void Awake()
        {
            transform.Find("Canvas").Find("Event1").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowOverlay<EventOverlay>();
            });

            transform.Find("Canvas").Find("Event2").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowOverlay<EventOverlay>();
            });
        }

        void Start()
        {

        }

        void Update()
        {

        }

        public static EventsWidget CreateInstance(Transform parent)
        {
            var prefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Widgets/EventsWidget/EventsWidget"));
            prefab.name = nameof(EventsWidget);
            var rectTransform = prefab.GetComponent<RectTransform>();
            rectTransform.SetParent(parent, false);
            UIManager.SetStretch(rectTransform);
            return prefab.AddComponent<EventsWidget>();
        }
    }
}
