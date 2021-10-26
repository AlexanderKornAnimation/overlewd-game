using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class PrepareBossFightPopup : BasePopup
    {
        void Start()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Popups/PrepareBossFightPopup/PrepareBossFightPopup"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            screenRectTransform.Find("Canvas").Find("GlobalMap").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<MapScreen>();
            });

            screenRectTransform.Find("Canvas").Find("BossFight").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<BossFightScreen>();
            });
        }

        void Update()
        {

        }
    }

}
