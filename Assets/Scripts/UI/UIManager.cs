using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public static class UIManager
    {
        private static GameObject currentScreenGO;
        private static GameObject currentOverlayGO;
        private static GameObject currentDialogBoxGO;

        public static void ShowScreen<T>() where T : BaseScreen
        {
            if (currentScreenGO?.GetComponent<T>() == null)
            {
                GameObject.Destroy(currentScreenGO);
                currentScreenGO = new GameObject(typeof(T).Name);
                currentScreenGO.AddComponent<T>();

                HideOverlay();
            }
            else
            {
                HideOverlay();
            }
        }

        public static void HideScreen()
        {
            GameObject.Destroy(currentScreenGO);
            currentScreenGO = null;
        }

        public static void ShowOverlay<T>() where T : BaseOverlay
        {
            if (currentOverlayGO?.GetComponent<T>() == null)
            {
                GameObject.Destroy(currentOverlayGO);
                currentOverlayGO = new GameObject(typeof(T).Name);
                currentOverlayGO.AddComponent<T>();
            }
        }

        public static void HideOverlay()
        {
            GameObject.Destroy(currentOverlayGO);
            currentOverlayGO = null;
        }

        public static bool ShowingOverlay<T>() where T : BaseOverlay
        {
            return (currentOverlayGO?.GetComponent<T>() != null);
        }

        public static void ShowDialogBox(string title, string message, Action yes, Action no = null)
        {
            GameObject.Destroy(currentDialogBoxGO);
            currentDialogBoxGO = new GameObject(typeof(DebugDialogBox).Name);
            var dialogBox = currentDialogBoxGO.AddComponent<DebugDialogBox>();

            dialogBox.title = title;
            dialogBox.message = message;

            dialogBox.yesAction = () =>
            {
                GameObject.Destroy(currentDialogBoxGO);
                currentDialogBoxGO = null;
                yes?.Invoke();
            };

            if (no != null)
            {
                dialogBox.noAction = () =>
                {
                    GameObject.Destroy(currentDialogBoxGO);
                    currentDialogBoxGO = null;
                    no.Invoke();
                };
            }
        }

        public static void HideDialogBox()
        {
            GameObject.Destroy(currentDialogBoxGO);
            currentDialogBoxGO = null;
        }
    }

}
