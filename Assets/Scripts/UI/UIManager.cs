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
        private static GameObject uiRootCanvasGO;
        private static GameObject uiRootScreenLayerGO;

        private static GameObject uiEventSystem;

        private static GameObject uiScreenLayerGO;
        private static GameObject uiPopupLayerGO;
        private static GameObject uiSubPopupLayerGO;
        private static GameObject uiOverlayLayerGO;
        private static GameObject uiNotificationLayerGO;
        private static GameObject uiDialogLayerGO;

        private static GameObject currentScreenGO;
        private static GameObject currentPopupGO;
        private static GameObject currentSubPopupGO;
        private static GameObject currentOverlayGO;
        private static GameObject currentNotificationGO;
        private static GameObject currentDialogBoxGO;

        private static void ConfigureRootScreenLayer()
        {
            uiRootScreenLayerGO = new GameObject("UIRootScreenLayer");
            uiRootScreenLayerGO.layer = 5;
            var uiRootScreenLayerGO_rectTransform = uiRootScreenLayerGO.AddComponent<RectTransform>();
            uiRootScreenLayerGO_rectTransform.SetParent(uiRootCanvasGO.transform, false);
            SetStretch(uiRootScreenLayerGO_rectTransform);

            //FullHD fixed aspect
            var uiRootScreenLayerGO_aspectRatioFitter = uiRootScreenLayerGO.AddComponent<AspectRatioFitter>();
            uiRootScreenLayerGO_aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
            uiRootScreenLayerGO_aspectRatioFitter.aspectRatio = 1.777778f;

            //cut screen content outside FullHD 
            var uiRootScreenLayerGO_rectMask2D = uiRootScreenLayerGO.AddComponent<RectMask2D>();
        }

        private static void ConfigureLayer(out GameObject layerGO, string name, int siblingIndex)
        {
            layerGO = new GameObject(name);
            layerGO.layer = 5;
            var layerGO_rectTransform = layerGO.AddComponent<RectTransform>();
            layerGO_rectTransform.SetParent(uiRootScreenLayerGO.transform, false);
            SetStretch(layerGO_rectTransform);

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

            ConfigureRootScreenLayer();

            ConfigureLayer(out uiScreenLayerGO, "UIScreenLayer", 0);
            ConfigureLayer(out uiPopupLayerGO, "UIPopupLayer", 1);
            ConfigureLayer(out uiSubPopupLayerGO, "UISubPopupLayer", 2);
            ConfigureLayer(out uiOverlayLayerGO, "UIOverlayLayer", 3);
            ConfigureLayer(out uiNotificationLayerGO, "UINotificationLayer", 4);
            ConfigureLayer(out uiDialogLayerGO, "UIDialogLayer", 5);

            uiEventSystem = new GameObject("UIManagerEventSystem");
            var uiEventSystem_eventSystem = uiEventSystem.AddComponent<EventSystem>();
            var uiEventSystem_standaloneInputModule = uiEventSystem.AddComponent<StandaloneInputModule>();
            var uiEventSystem_baseInput = uiEventSystem.AddComponent<BaseInput>();
        }

        public static T ShowScreen<T>() where T : BaseScreen
        {
            if (currentScreenGO?.GetComponent<T>() == null)
            {
                HideScreen();

                currentScreenGO = new GameObject(typeof(T).Name);
                currentScreenGO.layer = 5;
                var currentScreenGO_rectTransform = currentScreenGO.AddComponent<RectTransform>();
                currentScreenGO_rectTransform.SetParent(uiScreenLayerGO.transform, false);
                SetStretch(currentScreenGO_rectTransform);
                currentScreenGO.AddComponent<T>().Show();
            }
            else
            {
                HidePopup();
                HideSubPopup();
                HideOverlay();
            }

            return currentScreenGO.GetComponent<T>();
        }

        public static void HideScreen()
        {
            currentScreenGO?.GetComponent<BaseScreen>().Hide();
            currentScreenGO = null;

            HidePopup();
            HideSubPopup();
            HideOverlay();
        }

        public static T ShowPopup<T>() where T : BasePopup
        {
            if (currentPopupGO?.GetComponent<T>() == null)
            {
                HidePopup();

                currentPopupGO = new GameObject(typeof(T).Name);
                currentPopupGO.layer = 5;
                var currentPopupGO_rectTransform = currentPopupGO.AddComponent<RectTransform>();
                currentPopupGO_rectTransform.SetParent(uiPopupLayerGO.transform, false);
                SetStretch(currentPopupGO_rectTransform);
                currentPopupGO.AddComponent<T>().Show();
            }
            else
            {
                HideSubPopup();
            }

            return currentPopupGO.GetComponent<T>();
        }

        public static void HidePopup()
        {
            currentPopupGO?.GetComponent<BasePopup>().Hide();
            currentPopupGO = null;

            HideSubPopup();
        }

        public static T ShowSubPopup<T>() where T : BaseSubPopup
        {
            if (currentSubPopupGO?.GetComponent<T>() == null)
            {
                HideSubPopup();

                currentSubPopupGO = new GameObject(typeof(T).Name);
                currentSubPopupGO.layer = 5;
                var currentSubPopupGO_rectTransform = currentSubPopupGO.AddComponent<RectTransform>();
                currentSubPopupGO_rectTransform.SetParent(uiSubPopupLayerGO.transform, false);
                SetStretch(currentSubPopupGO_rectTransform);
                currentSubPopupGO.AddComponent<T>().Show();
            }

            return currentSubPopupGO.GetComponent<T>();
        }

        public static void HideSubPopup()
        {
            currentSubPopupGO?.GetComponent<BaseSubPopup>().Hide();
            currentSubPopupGO = null;
        }

        public static T ShowOverlay<T>() where T : BaseOverlay
        {
            if (currentOverlayGO?.GetComponent<T>() == null)
            {
                HideOverlay();

                currentOverlayGO = new GameObject(typeof(T).Name);
                currentOverlayGO.layer = 5;
                var currentOverlayGO_rectTransform = currentOverlayGO.AddComponent<RectTransform>();
                currentOverlayGO_rectTransform.SetParent(uiOverlayLayerGO.transform, false);
                SetStretch(currentOverlayGO_rectTransform);
                currentOverlayGO.AddComponent<T>().Show();
            }

            return currentOverlayGO.GetComponent<T>();
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

        public static T ShowNotification<T>() where T : BaseNotification
        {
            if (currentNotificationGO?.GetComponent<T>() == null)
            {
                HideNotification();

                currentNotificationGO = new GameObject(typeof(T).Name);
                currentNotificationGO.layer = 5;
                var currentNotificationGO_rectTransform = currentNotificationGO.AddComponent<RectTransform>();
                currentNotificationGO_rectTransform.SetParent(uiNotificationLayerGO.transform, false);
                SetStretch(currentNotificationGO_rectTransform);
                currentNotificationGO.AddComponent<T>().Show();
            }

            return currentNotificationGO.GetComponent<T>();
        }

        public static void HideNotification()
        {
            currentNotificationGO?.GetComponent<BaseNotification>().Hide();
            currentNotificationGO = null;
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
