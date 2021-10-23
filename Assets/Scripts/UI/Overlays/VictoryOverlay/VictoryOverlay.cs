using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class VictoryOverlay : BaseOverlay
    {
        void Start()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Overlays/VictoryOverlay/VictoryOverlay"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            screenRectTransform.Find("Canvas").Find("NextButton").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.HideOverlay();
            });
        }

        void Update()
        {

        }

        public override void Show()
        {
            gameObject.AddComponent<ImmediateShow>();
        }

        public override void Hide()
        {
            gameObject.AddComponent<ImmediateHide>();
        }
    }
}
