using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class GirlScreen : BaseScreen
    {
        void Start()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/GirlScreen/Girl"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            screenRectTransform.Find("Canvas").Find("Harem").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<HaremScreen>();
            });

            screenRectTransform.Find("Canvas").Find("Memory").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<MemoryScreen>();
            });

            screenRectTransform.Find("Canvas").Find("Dialog").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<DialogScreen>();
            });

            screenRectTransform.Find("Canvas").Find("Portal").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<PortalScreen>();
            });
        }

        void Update()
        {

        }
    }
}
