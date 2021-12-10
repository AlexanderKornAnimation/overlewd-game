using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class DebugScreenViewer : BaseScreen
    {
        
        void Start()
        {
            string prefabPath = "Prefabs/UI/Popups/BuildingPopups/BuildingPopup";
            //prefabPath = "Prefabs/UI/DebugScreens/DebugContentViewer/ContentViewer";
            var screenPrefab = (GameObject)Instantiate(Resources.Load(prefabPath));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            screenPrefab.AddComponent<FTUE.CavePopup>();
        }

        void Update()
        {

        }

        void OnGUI()
        {
            float offset = Screen.width * 0.05f;
            float size = Screen.width * 0.1f;
            var rect = new Rect(offset, offset, size, size);
            if (GUI.Button(rect, "Castle")) {
                UIManager.ShowScreen<CastleScreen>();
            }
        }
    }
}
