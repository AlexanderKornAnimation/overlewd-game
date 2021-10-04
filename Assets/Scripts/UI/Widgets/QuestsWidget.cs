using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class QuestsWidget : BaseWidget
    {
        void Start()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Widgets/QuestsWidget/QuestWidget"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            screenRectTransform.Find("Canvas").Find("Quest1").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowOverlay<QuestOverlay>();
            });

            screenRectTransform.Find("Canvas").Find("Quest2").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowOverlay<QuestOverlay>();
            });
        }

        void Update()
        {

        }
    }
}
