using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class LoadingScreen : BaseScreen
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnGUI()
        {
            GUI.depth = 2;
            GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
            {
                GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "Loading Screen");

                var rect = new Rect(Screen.width - 130, 10, 110, 30);
                if (GUI.Button(rect, "Content Viewer"))
                {
                    UIManager.ShowScreen<DebugContentViewer>();
                }
            }
            GUI.EndGroup();
        }
    }
}
