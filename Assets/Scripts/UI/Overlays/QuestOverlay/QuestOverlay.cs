using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class QuestOverlay : BaseOverlay
    {
        void Start()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Overlays/QuestOverlay/QuestOverlay"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            screenRectTransform.Find("Canvas").Find("Back").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.HideOverlay();
            });
        }

        void Update()
        {

        }
    }
}
