using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Threading.Tasks;

namespace Overlewd
{
    public static class UIManager
    {
        private static Vector2 currentResolution;
        private static float currentAspectRatio;

        private static GameObject uiRootCanvasGO;
        private static CanvasScaler uiRootCanvasGO_canvasScaler;
        private static GameObject uiRootScreenLayerGO;
        private static AspectRatioFitter uiRootScreenLayerGO_aspectRatioFitter;

        private static GameObject uiEventSystem;

        private static GameObject uiScreenLayerGO;
        private static GameObject uiPopupLayerGO;
        private static GameObject uiSubPopupLayerGO;
        private static GameObject uiOverlayLayerGO;
        private static GameObject uiNotificationLayerGO;
        private static GameObject uiDialogLayerGO;

        private static BaseScreen prevScreen;
        private static BaseScreen currentScreen;
        private static BasePopup prevPopup;
        private static BasePopup currentPopup;
        private static BaseSubPopup prevSubPopup;
        private static BaseSubPopup currentSubPopup;
        private static BaseOverlay prevOverlay;
        private static BaseOverlay currentOverlay;
        private static BaseNotification prevNotification;
        private static BaseNotification currentNotification;
        private static GameObject currentDialogBoxGO;

        private static BaseMissclick overlayMissclick;
        private static BaseMissclick popupMissclick;
        private static BaseMissclick subPopupMissclick;
        private static BaseMissclick notificationMissclick;

        private static Vector2 SelectResolution()
        {
            var aspectRatio = (float)Screen.width / (float)Screen.height;

            //1920:1080
            var aspectDeltaFullHD = Mathf.Abs(1920.0f / 1080.0f - aspectRatio);
            //2160:1080
            var aspectDeltaWideHD = Mathf.Abs(2160.0f / 1080.0f - aspectRatio);

            if (aspectDeltaFullHD < aspectDeltaWideHD)
            {
                return new Vector2(1920, 1080);
            }
            return new Vector2(2160, 1080);
        }

        public static void ChangeResolution()
        {
            currentResolution = SelectResolution();
            currentAspectRatio = currentResolution.x / currentResolution.y;

            uiRootCanvasGO_canvasScaler.referenceResolution = currentResolution;
            uiRootScreenLayerGO_aspectRatioFitter.aspectRatio = currentAspectRatio;
        }

        private static void ConfigureRootScreenLayer()
        {
            uiRootScreenLayerGO = new GameObject("UIRootScreenLayer");
            uiRootScreenLayerGO.layer = 5;
            var uiRootScreenLayerGO_rectTransform = uiRootScreenLayerGO.AddComponent<RectTransform>();
            uiRootScreenLayerGO_rectTransform.SetParent(uiRootCanvasGO.transform, false);
            SetStretch(uiRootScreenLayerGO_rectTransform);

            //resolution fixed aspect
            uiRootScreenLayerGO_aspectRatioFitter = uiRootScreenLayerGO.AddComponent<AspectRatioFitter>();
            uiRootScreenLayerGO_aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
            uiRootScreenLayerGO_aspectRatioFitter.aspectRatio = currentAspectRatio;

            //cut screen content outside
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

        //Missclick Instantiate
        private static T GetMissclickInstance<T>(Transform parent) where T : BaseMissclick
        {
            var missclickGO = new GameObject(typeof(T).Name);
            var missclickGO_screenRectTransform = missclickGO.AddComponent<RectTransform>();
            missclickGO_screenRectTransform.SetParent(parent, false);
            missclickGO_screenRectTransform.SetAsFirstSibling();
            SetStretch(missclickGO_screenRectTransform);
            return missclickGO.AddComponent<T>();
        }

        public static T GetPopupMissclickInstance<T>() where T : PopupMissclick
        {
            return GetMissclickInstance<T>(uiPopupLayerGO.transform);
        }

        public static T GetSubPopupMissclickInstance<T>() where T : SubPopupMissclick
        {
            return GetMissclickInstance<T>(uiSubPopupLayerGO.transform);
        }

        public static T GetOverlayMissclickInstance<T>() where T : OverlayMissclick
        {
            return GetMissclickInstance<T>(uiOverlayLayerGO.transform);
        }

        public static T GetNotificationMissclickInstance<T>() where T : NotificationMissclick
        {
            return GetMissclickInstance<T>(uiNotificationLayerGO.transform);
        }

        //Screen Instantiate
        private static T GetScreenInstance<T>(Transform parent) where T : BaseScreen
        {
            var screenGO = new GameObject(typeof(T).Name);
            screenGO.layer = 5;
            var screenGO_rectTransform = screenGO.AddComponent<RectTransform>();
            screenGO_rectTransform.SetParent(parent, false);
            SetStretch(screenGO_rectTransform);
            return screenGO.AddComponent<T>();
        }

        public static T GetScreenInstance<T>() where T : BaseScreen
        {
            return GetScreenInstance<T>(uiScreenLayerGO.transform);
        }

        public static T GetPopupInstance<T>() where T : BasePopup
        {
            return GetScreenInstance<T>(uiPopupLayerGO.transform); 
        }

        public static T GetSubPopupInstance<T>() where T : BaseSubPopup
        {
            return GetScreenInstance<T>(uiSubPopupLayerGO.transform);
        }

        public static T GetOverlayInstance<T>() where T : BaseOverlay
        {
            return GetScreenInstance<T>(uiOverlayLayerGO.transform);
        }

        public static T GetNotificationInstance<T>() where T : BaseNotification
        {
            return GetScreenInstance<T>(uiNotificationLayerGO.transform);
        }

        public static void Initialize()
        {
#if UNITY_ANDROID
            Application.targetFrameRate = 60;
#endif

            currentResolution = SelectResolution();
            currentAspectRatio = currentResolution.x / currentResolution.y;

            uiRootCanvasGO = new GameObject("UIManagerRootCanvas");
            uiRootCanvasGO.layer = 5;
            var uiRootCanvasGO_rectTransform = uiRootCanvasGO.AddComponent<RectTransform>();
            var uiRootCanvasGO_canvas = uiRootCanvasGO.AddComponent<Canvas>();
            uiRootCanvasGO_canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            uiRootCanvasGO_canvasScaler = uiRootCanvasGO.AddComponent<CanvasScaler>();
            uiRootCanvasGO_canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            uiRootCanvasGO_canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
            uiRootCanvasGO_canvasScaler.referenceResolution = currentResolution;
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

        public static void UpdateGameData()
        {
            currentScreen?.UpdateGameData();
            currentPopup?.UpdateGameData();
            currentSubPopup?.UpdateGameData();
            currentOverlay?.UpdateGameData();
            currentNotification?.UpdateGameData();
        }

        //Screen Layer
        public static T GetScreen<T>() where T : BaseScreen
        {
            return currentScreen as T;
        }

        public static bool HasScreen<T>() where T : BaseScreen
        {
            return currentScreen?.GetType() == typeof(T);
        }

        public static T ShowScreen<T>() where T : BaseScreen
        {
            HideScreen();

            currentScreen = GetScreenInstance<T>();
            currentScreen.Show();
            currentScreen.ShowMissclick();

            var prevSubPopupTr = prevSubPopup?.GetTransition();
            var prevPopupTr = prevPopup?.GetTransition();
            var prevOverlayTr = prevOverlay?.GetTransition();
            var prevScreenTr = prevScreen?.GetTransition();
            var curScreenTr = currentScreen.GetTransition();

            prevSubPopupTr?.InitializerCall(() => 
            {
                prevPopupTr?.AddLocker(prevSubPopupTr);
                prevOverlayTr?.AddLocker(prevSubPopupTr);
                prevScreenTr?.AddLocker(prevSubPopupTr);
                curScreenTr.AddLocker(prevSubPopupTr);
            });
            prevSubPopupTr?.AddEndListener(() => 
            {
                prevPopupTr?.RemoveLocker(prevSubPopupTr);
                prevOverlayTr?.RemoveLocker(prevSubPopupTr);
                prevScreenTr?.RemoveLocker(prevSubPopupTr);
                curScreenTr.RemoveLocker(prevSubPopupTr);
            });

            prevPopupTr?.InitializerCall(() => 
            {
                prevSubPopupTr?.AddLocker(prevPopupTr);
                prevOverlayTr?.AddLocker(prevPopupTr);
                prevScreenTr?.AddLocker(prevPopupTr);
                curScreenTr.AddLocker(prevPopupTr);
            });
            prevPopupTr?.AddPreparedListener(() => 
            {
                prevSubPopupTr?.RemoveLocker(prevPopupTr);
                prevOverlayTr?.RemoveLocker(prevPopupTr);
                prevScreenTr?.RemoveLocker(prevPopupTr);
                curScreenTr.RemoveLocker(prevPopupTr);
            });

            prevOverlayTr?.InitializerCall(() => 
            {
                prevSubPopupTr?.AddLocker(prevOverlayTr);
                prevPopupTr?.AddLocker(prevOverlayTr);
                prevScreenTr?.AddLocker(prevOverlayTr);
                curScreenTr.AddLocker(prevOverlayTr);
            });
            prevOverlayTr?.AddPreparedListener(() =>
            {
                prevSubPopupTr?.RemoveLocker(prevOverlayTr);
                prevPopupTr?.RemoveLocker(prevOverlayTr);
                prevScreenTr?.RemoveLocker(prevOverlayTr);
                curScreenTr.RemoveLocker(prevOverlayTr);
            });

            prevScreenTr?.InitializerCall(() => 
            {
                prevSubPopupTr?.AddLocker(prevScreenTr);
                prevPopupTr?.AddLocker(prevScreenTr);
                prevOverlayTr?.AddLocker(prevScreenTr);
                curScreenTr.AddLocker(prevScreenTr);
            });
            prevScreenTr?.AddPreparedListener(() => 
            {
                prevSubPopupTr?.RemoveLocker(prevScreenTr);
                prevPopupTr?.RemoveLocker(prevScreenTr);
                prevOverlayTr?.RemoveLocker(prevScreenTr);
            });
            prevScreenTr?.AddEndListener(() => 
            {
                curScreenTr.RemoveLocker(prevScreenTr);
            });

            curScreenTr.InitializerCall(() => 
            {
                prevSubPopupTr?.AddLocker(curScreenTr);
                prevPopupTr?.AddLocker(curScreenTr);
                prevOverlayTr?.AddLocker(curScreenTr);
                prevScreenTr?.AddLocker(curScreenTr);
            });
            curScreenTr.AddPreparedListener(() => 
            {
                prevSubPopupTr?.RemoveLocker(curScreenTr);
                prevPopupTr?.RemoveLocker(curScreenTr);
                prevOverlayTr?.RemoveLocker(curScreenTr);
                prevScreenTr?.RemoveLocker(curScreenTr);
            });

            return currentScreen as T;
        }

        public static void HideScreen()
        {
            prevScreen = currentScreen;
            currentScreen = null;
            prevScreen?.Hide();

            HidePopup();
            HideOverlay();

            var prevSubPopupTr = prevSubPopup?.GetTransition();
            var prevPopupTr = prevPopup?.GetTransition();
            var prevOverlayTr = prevOverlay?.GetTransition();
            var prevScreenTr = prevScreen?.GetTransition();

            prevSubPopupTr?.InitializerCall(() =>
            {
                prevPopupTr?.AddLocker(prevSubPopupTr);
                prevOverlayTr?.AddLocker(prevSubPopupTr);
                prevScreenTr?.AddLocker(prevSubPopupTr);
            });
            prevSubPopupTr?.AddEndListener(() =>
            {
                prevPopupTr?.RemoveLocker(prevSubPopupTr);
                prevOverlayTr?.RemoveLocker(prevSubPopupTr);
                prevScreenTr?.RemoveLocker(prevSubPopupTr);
            });

            prevPopupTr?.InitializerCall(() =>
            {
                prevSubPopupTr?.AddLocker(prevPopupTr);
                prevOverlayTr?.AddLocker(prevPopupTr);
                prevScreenTr?.AddLocker(prevPopupTr);
            });
            prevPopupTr?.AddPreparedListener(() =>
            {
                prevSubPopupTr?.RemoveLocker(prevPopupTr);
                prevOverlayTr?.RemoveLocker(prevPopupTr);
                prevScreenTr?.RemoveLocker(prevPopupTr);
            });

            prevOverlayTr?.InitializerCall(() =>
            {
                prevSubPopupTr?.AddLocker(prevOverlayTr);
                prevPopupTr?.AddLocker(prevOverlayTr);
                prevScreenTr?.AddLocker(prevOverlayTr);
            });
            prevOverlayTr?.AddPreparedListener(() =>
            {
                prevSubPopupTr?.RemoveLocker(prevOverlayTr);
                prevPopupTr?.RemoveLocker(prevOverlayTr);
                prevScreenTr?.RemoveLocker(prevOverlayTr);
            });

            prevScreenTr?.InitializerCall(() =>
            {
                prevSubPopupTr?.AddLocker(prevScreenTr);
                prevPopupTr?.AddLocker(prevScreenTr);
                prevOverlayTr?.AddLocker(prevScreenTr);
            });
            prevScreenTr?.AddPreparedListener(() =>
            {
                prevSubPopupTr?.RemoveLocker(prevScreenTr);
                prevPopupTr?.RemoveLocker(prevScreenTr);
                prevOverlayTr?.RemoveLocker(prevScreenTr);
            });
        }

        //Popup Layer
        public static T GetPopupMissclick<T>() where T : PopupMissclick
        {
            return popupMissclick as T;
        }

        public static bool HasPopupMissclick<T>() where T : PopupMissclick
        {
            return popupMissclick?.GetType() == typeof(T);
        }

        public static T ShowPopupMissclick<T>() where T : PopupMissclick
        {
            if (!HasPopupMissclick<T>())
            {
                HidePopupMissclick();

                popupMissclick = GetPopupMissclickInstance<T>();
                popupMissclick.Show();
            }
            return popupMissclick as T;
        }

        public static void HidePopupMissclick()
        {
            popupMissclick?.Hide();
            popupMissclick = null;
        }

        public static T GetPopup<T>() where T : BasePopup
        {
            return currentPopup as T;
        }

        public static bool HasPopup<T>() where T : BasePopup
        {
            return currentPopup?.GetType() == typeof(T);
        }

        public static T ShowPopup<T>() where T : BasePopup
        {
            prevPopup = currentPopup;
            prevPopup?.Hide();
            HideSubPopup();

            currentPopup = GetPopupInstance<T>();
            currentPopup.Show();
            currentPopup.ShowMissclick();

            var prevSubPopupTr = prevSubPopup?.GetTransition();
            var prevPopupTr = prevPopup?.GetTransition();
            var curPopupTr = currentPopup.GetTransition();

            prevSubPopupTr?.InitializerCall(() =>
            {
                prevPopupTr?.AddLocker(prevSubPopupTr);
                curPopupTr.AddLocker(prevSubPopupTr);
            });
            prevSubPopupTr?.AddEndListener(() =>
            {
                prevPopupTr?.RemoveLocker(prevSubPopupTr);
                curPopupTr.RemoveLocker(prevSubPopupTr);
            });

            prevPopupTr?.InitializerCall(() =>
            {
                prevSubPopupTr?.AddLocker(prevPopupTr);
                curPopupTr.AddLocker(prevPopupTr);
            });
            prevPopupTr?.AddPreparedListener(() =>
            {
                prevSubPopupTr?.RemoveLocker(prevPopupTr);
            });
            prevPopupTr?.AddEndListener(() =>
            {
                curPopupTr?.RemoveLocker(prevPopupTr);
            });

            curPopupTr.InitializerCall(() => 
            {
                prevSubPopupTr?.AddLocker(curPopupTr);
                prevPopupTr?.AddLocker(curPopupTr);
            });
            curPopupTr.AddPreparedListener(() => 
            {
                prevSubPopupTr?.RemoveLocker(curPopupTr);
                prevPopupTr?.RemoveLocker(curPopupTr);
            });

            return currentPopup as T;
        }

        public static void HidePopup()
        {
            HidePopupMissclick();

            prevPopup = currentPopup;
            currentPopup = null;
            prevPopup?.Hide();

            HideSubPopup();

            var prevSubPopupTr = prevSubPopup?.GetTransition();
            var prevPopupTr = prevPopup?.GetTransition();

            prevSubPopupTr?.InitializerCall(() =>
            {
                prevPopupTr?.AddLocker(prevSubPopupTr);
            });
            prevSubPopupTr?.AddEndListener(() =>
            {
                prevPopupTr?.RemoveLocker(prevSubPopupTr);
            });

            prevPopupTr?.InitializerCall(() =>
            {
                prevSubPopupTr?.AddLocker(prevPopupTr);
            });
            prevPopupTr?.AddPreparedListener(() =>
            {
                prevSubPopupTr?.RemoveLocker(prevPopupTr);
            });
        }

        //SubPopup Layer
        public static T GetSubPopupMissclick<T>() where T : SubPopupMissclick
        {
            return subPopupMissclick as T;
        }

        public static bool HasSubPopupMissclick<T>() where T : SubPopupMissclick
        {
            return subPopupMissclick?.GetType() == typeof(T);
        }

        public static T ShowSubPopupMissclick<T>() where T : SubPopupMissclick
        {
            if (!HasSubPopupMissclick<T>())
            {
                HideSubPopupMissclick();

                subPopupMissclick = GetSubPopupMissclickInstance<T>();
                subPopupMissclick.Show();
            }
            return subPopupMissclick as T;
        }

        public static void HideSubPopupMissclick()
        {
            subPopupMissclick?.Hide();
            subPopupMissclick = null;
        }

        public static T GetSubPopup<T>() where T : BaseSubPopup
        {
            return currentSubPopup as T;
        }

        public static bool HasSubPopup<T>() where T : BaseSubPopup
        {
            return currentSubPopup?.GetType() == typeof(T);
        }

        public static T ShowSubPopup<T>() where T : BaseSubPopup
        {
            prevSubPopup = currentSubPopup;
            prevSubPopup?.Hide();

            currentSubPopup = GetSubPopupInstance<T>();
            currentSubPopup.Show();
            currentSubPopup.ShowMissclick();

            var prevSubPopupTr = prevSubPopup?.GetTransition();
            var curSubPopupTr = currentSubPopup.GetTransition();

            prevSubPopupTr?.InitializerCall(() => 
            {
                curSubPopupTr.AddLocker(prevSubPopupTr);
            });
            prevSubPopupTr?.AddEndListener(() => 
            {
                curSubPopupTr.RemoveLocker(prevSubPopupTr);
            });

            curSubPopupTr.InitializerCall(() => 
            {
                prevSubPopupTr?.AddLocker(curSubPopupTr);
            });
            curSubPopupTr.AddPreparedListener(() => 
            {
                prevSubPopupTr?.RemoveLocker(curSubPopupTr);
            });

            return currentSubPopup as T;
        }

        public static void HideSubPopup()
        {
            HideSubPopupMissclick();

            prevSubPopup = currentSubPopup;
            currentSubPopup = null;
            prevSubPopup?.Hide();
        }

        //Overlay Layer
        public static T GetOverlayMissclick<T>() where T : OverlayMissclick
        {
            return overlayMissclick as T;
        }

        public static bool HasOverlayMissclick<T>() where T : OverlayMissclick
        {
            return overlayMissclick?.GetType() == typeof(T);
        }

        public static T ShowOverlayMissclick<T>() where T : OverlayMissclick
        {
            if (!HasOverlayMissclick<T>())
            {
                HideOverlayMissclick();

                overlayMissclick = GetOverlayMissclickInstance<T>();
                overlayMissclick.Show();
            }
            return overlayMissclick as T;
        }

        public static void HideOverlayMissclick()
        {
            overlayMissclick?.Hide();
            overlayMissclick = null;
        }

        public static T GetOverlay<T>() where T : BaseOverlay
        {
            return currentOverlay as T;
        }

        public static bool HasOverlay<T>() where T : BaseOverlay
        {
            return currentOverlay?.GetType() == typeof(T);
        }

        public static T ShowOverlay<T>() where T : BaseOverlay
        {
            prevOverlay = currentOverlay;
            prevOverlay?.Hide();

            currentOverlay = GetOverlayInstance<T>();
            currentOverlay.Show();
            currentOverlay.ShowMissclick();

            var prevOverlayTr = prevOverlay?.GetTransition();
            var curOverlayTr = currentOverlay.GetTransition();

            prevOverlayTr?.InitializerCall(() =>
            {
                curOverlayTr.AddLocker(prevOverlayTr);
            });
            prevOverlayTr?.AddEndListener(() =>
            {
                curOverlayTr.RemoveLocker(prevOverlayTr);
            });

            curOverlayTr.InitializerCall(() =>
            {
                prevOverlayTr?.AddLocker(curOverlayTr);
            });
            curOverlayTr.AddPreparedListener(() =>
            {
                prevOverlayTr?.RemoveLocker(curOverlayTr);
            });

            return currentOverlay as T;
        }

        public static void HideOverlay()
        {
            HideOverlayMissclick();

            prevOverlay = currentOverlay;
            currentOverlay = null;
            prevOverlay?.Hide();
        }

        //Notification Layer
        public static T GetNotificationMissclick<T>() where T : NotificationMissclick
        {
            return notificationMissclick as T;
        }
        public static bool HasNotificationMissclick<T>() where T : NotificationMissclick
        {
            return notificationMissclick?.GetType() == typeof(T);
        }

        public static T ShowNotificationMissclick<T>() where T : NotificationMissclick
        {
            if (!HasNotificationMissclick<T>())
            {
                HideNotificationMissclick();

                notificationMissclick = GetNotificationMissclickInstance<T>();
                notificationMissclick.Show();
            }
            return notificationMissclick as T;
        }

        public static void HideNotificationMissclick()
        {
            notificationMissclick?.Hide();
            notificationMissclick = null;
        }

        public static T GetNotification<T>() where T : BaseNotification
        {
            return currentNotification as T;
        }

        public static bool HasNotification<T>() where T : BaseNotification
        {
            return currentNotification?.GetType() == typeof(T);
        }

        public static T ShowNotification<T>() where T : BaseNotification
        {
            prevNotification = currentNotification;
            prevNotification?.Hide();

            currentNotification = GetNotificationInstance<T>();
            currentNotification.Show();
            currentNotification.ShowMissclick();

            var prevNotificationTr = prevNotification?.GetTransition();
            var curNotificationTr = currentNotification.GetTransition();

            prevNotificationTr?.InitializerCall(() =>
            {
                curNotificationTr.AddLocker(prevNotificationTr);
            });
            prevNotificationTr?.AddEndListener(() =>
            {
                curNotificationTr.RemoveLocker(prevNotificationTr);
            });

            curNotificationTr.InitializerCall(() =>
            {
                prevNotificationTr?.AddLocker(curNotificationTr);
            });
            curNotificationTr.AddPreparedListener(() =>
            {
                prevNotificationTr?.RemoveLocker(curNotificationTr);
            });

            return currentNotification as T;
        }

        public static void HideNotification()
        {
            HideNotificationMissclick();

            prevNotification = currentNotification;
            currentNotification = null;
            prevNotification?.Hide();
        }

        //Dialog Layer
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
