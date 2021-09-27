using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class DebugDialogBox : MonoBehaviour
    {
        private int dialogWidth = 300;
        private int dialogHeight = 100;

        public Action yesAction { get; set; }
        public Action noAction { get; set; }
        public string title { get; set; }
        public string message { get; set; }

        void DoMyWindow(int windowID)
        {
            var btnWidth = (int)(dialogWidth * 0.4);
            var btnHeight = (int)(btnWidth / 5.0);
            var btnOffset = (int)(dialogWidth * 0.05);

            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.fontSize = (int)(btnHeight * 0.8);

            GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.fontSize = (int)(buttonStyle.fontSize * 0.6);

            GUI.Label(new Rect(10, 20, dialogWidth - 20, (int)(dialogHeight * 0.4)), message, labelStyle);
            if (GUI.Button(new Rect(btnOffset, dialogHeight - btnHeight - btnOffset, btnWidth, btnHeight), "No", buttonStyle))
            {
                noAction?.Invoke();
            }
            if (GUI.Button(new Rect(dialogWidth - btnWidth - btnOffset, dialogHeight - btnHeight - btnOffset, btnWidth, btnHeight), "Yes", buttonStyle))
            {
                yesAction?.Invoke();
            }
        }

        void OnGUI()
        {
            dialogWidth = (int)(Screen.width * 0.7);
            dialogHeight = (int)(dialogWidth / 3.0);

            GUIStyle windowStyle = new GUIStyle(GUI.skin.window);
            windowStyle.fontSize = (int)(dialogHeight * 0.2);

            Rect rect = new Rect((Screen.width / 2) - (dialogWidth / 2), (Screen.height / 2) - (dialogHeight / 2), dialogWidth, dialogHeight);
            GUI.ModalWindow(0, rect, DoMyWindow, title, windowStyle);
        }
    }
}
