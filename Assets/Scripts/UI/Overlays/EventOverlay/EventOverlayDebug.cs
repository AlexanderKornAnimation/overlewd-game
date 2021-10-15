using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class EventOverlayDebug : BaseOverlay
    {
        void Start()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Overlays/EventOverlay/EventOverlayDebug"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            screenRectTransform.Find("Canvas").Find("Back").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.HideOverlay();
            });

            screenRectTransform.Find("Canvas").Find("EventMarket").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<EventMarketScreen>();
            });
        }

        void Update()
        {

        }
    }
}
