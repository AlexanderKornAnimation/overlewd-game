using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class GirlScreen : BaseScreen
    {
        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/GirlScreen/Girl", transform);

            var canvas = screenInst.transform.Find("Canvas");

            canvas.Find("Harem").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<HaremScreen>();
            });

            canvas.Find("Memory").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<MemoryScreen>();
            });

            canvas.Find("Dialog").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<DialogScreen>();
            });

            canvas.Find("Portal").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<PortalScreen>();
            });
        }
    }
}
