using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Overlewd
{
    public static class UIManager
    {
        private static GameObject currentScreenGO;
        private static GameObject currentOverlayGO;
        private static GameObject currentDialogBoxGO;

        private static GameObject uiRootCanvasGO;
        private static GameObject uiScreenLayerGO;
        private static GameObject uiOverlayLayerGO;
        private static GameObject uiDialogLayerGO;
        private static GameObject uiEventSystem;

        private static void ConfigureLayer(out GameObject layerGO, string name, int siblingIndex)
        {
            layerGO = new GameObject(name);
            layerGO.layer = 5;
            var layerGO_rectTransform = layerGO.AddComponent<RectTransform>();
            layerGO_rectTransform.SetParent(uiRootCanvasGO.transform, false);
            SetStretch(layerGO_rectTransform);

            //FullHD fixed aspect
            var layerGO_aspectRatioFitter = layerGO.AddComponent<AspectRatioFitter>();
            layerGO_aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
            layerGO_aspectRatioFitter.aspectRatio = 1.777778f;

            //cut screen content outside FullHD 
            var layerGO_rectMask2D = layerGO.AddComponent<RectMask2D>();

            layerGO.transform.SetSiblingIndex(siblingIndex);
        }

        public static void SetStretch(RectTransform rectTransform)
        {
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
        }

        public static void Initialize()
        {
            uiRootCanvasGO = new GameObject("UIManagerRootCanvas");
            uiRootCanvasGO.layer = 5;
            var uiRootCanvasGO_rectTransform = uiRootCanvasGO.AddComponent<RectTransform>();
            var uiRootCanvasGO_canvas = uiRootCanvasGO.AddComponent<Canvas>();
            uiRootCanvasGO_canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var uiRootCanvasGO_canvasScaler = uiRootCanvasGO.AddComponent<CanvasScaler>();
            uiRootCanvasGO_canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            uiRootCanvasGO_canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
            uiRootCanvasGO_canvasScaler.referenceResolution = new Vector2(1920, 1080);
            var uiRootCanvasGO_graphicRaycaster = uiRootCanvasGO.AddComponent<GraphicRaycaster>();

            ConfigureLayer(out uiScreenLayerGO, "UIScreenLayer", 0);
            ConfigureLayer(out uiOverlayLayerGO, "UIOverlayLayer", 1);
            ConfigureLayer(out uiDialogLayerGO, "UIDialogLayer", 2);

            uiEventSystem = new GameObject("UIManagerEventSystem");
            var uiEventSystem_eventSystem = uiEventSystem.AddComponent<EventSystem>();
            var uiEventSystem_standaloneInputModule = uiEventSystem.AddComponent<StandaloneInputModule>();
            var uiEventSystem_baseInput = uiEventSystem.AddComponent<BaseInput>();
        }

        public static void ShowScreen<T>() where T : BaseScreen
        {
            if (currentScreenGO?.GetComponent<T>() == null)
            {
                currentScreenGO?.GetComponent<BaseScreen>().Hide();
                
                currentScreenGO = new GameObject(typeof(T).Name);
                currentScreenGO.layer = 5;
                var currentScreenGO_rectTransform = currentScreenGO.AddComponent<RectTransform>();
                currentScreenGO_rectTransform.SetParent(uiScreenLayerGO.transform, false);
                SetStretch(currentScreenGO_rectTransform);
                currentScreenGO.AddComponent<T>().Show();

                HideOverlay();
            }
            else
            {
                HideOverlay();
            }
        }

        public static void HideScreen()
        {
            currentScreenGO?.GetComponent<BaseScreen>().Hide();
            currentScreenGO = null;
        }

        public static void ShowOverlay<T>() where T : BaseOverlay
        {
            if (currentOverlayGO?.GetComponent<T>() == null)
            {
                currentOverlayGO?.GetComponent<BaseOverlay>().Hide();

                currentOverlayGO = new GameObject(typeof(T).Name);
                currentOverlayGO.layer = 5;
                var currentOverlayGO_rectTransform = currentOverlayGO.AddComponent<RectTransform>();
                currentOverlayGO_rectTransform.SetParent(uiOverlayLayerGO.transform, false);
                SetStretch(currentOverlayGO_rectTransform);
                currentOverlayGO.AddComponent<T>().Show();
            }
        }

        public static void HideOverlay()
        {
            currentOverlayGO?.GetComponent<BaseOverlay>().Hide();
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
            currentDialogBoxGO.layer = 5;
            var currentDialogBoxGO_rectTransform = currentDialogBoxGO.AddComponent<RectTransform>();
            currentDialogBoxGO_rectTransform.SetParent(uiDialogLayerGO.transform, false);
            SetStretch(currentDialogBoxGO_rectTransform);
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
