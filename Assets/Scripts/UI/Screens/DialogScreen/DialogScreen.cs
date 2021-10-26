using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class DialogScreen : BaseScreen
    {
        void Start()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/DialogScreen/DialogScreen"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            screenRectTransform.Find("Canvas").Find("SkipButton").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<GirlScreen>();
            });
        }

        void Update()
        {

        }
    }
}
