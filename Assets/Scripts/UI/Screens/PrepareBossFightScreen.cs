using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class PrepareBossFightScreen : BaseScreen
    {
        void Start()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/PrepareBossFightScreen/PrepareBossFight"));
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

        void OnGUI()
        {
            return;
            GUI.depth = 2;
            GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
            {
                GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "Boss Fight Screen");

                var rect = new Rect(60, 100, 110, 30);
                if (GUI.Button(rect, "Global Map"))
                {
                    UIManager.ShowScreen<MapScreen>();
                }

                rect.y += 35;
                if (GUI.Button(rect, "Boss Fight"))
                {
                    UIManager.ShowScreen<BossFightScreen>();
                }
            }
            GUI.EndGroup();
        }
    }

}
