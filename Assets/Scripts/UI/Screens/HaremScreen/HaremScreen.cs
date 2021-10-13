using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class HaremScreen : BaseScreen
    {
        void Start()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/HaremScreen/Harem"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            screenRectTransform.Find("Canvas").Find("Castle").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<CastleScreen>();
            });

            screenRectTransform.Find("Canvas").Find("Girl1").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<GirlScreen>();
            });

            screenRectTransform.Find("Canvas").Find("Girl2").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<GirlScreen>();
            });
        }

        void Update()
        {

        }
    }
}
