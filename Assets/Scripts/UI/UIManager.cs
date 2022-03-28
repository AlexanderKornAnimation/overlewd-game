using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;

namespace Overlewd
{
    public class UserInputLocker
    {
        private MonoBehaviour mbLocker;
        private ScreenTransition stLocker;
        private UnityWebRequest uwrLocker;

        public UserInputLocker(MonoBehaviour mbObj)
        {
            mbLocker = mbObj;
        }

        public UserInputLocker(ScreenTransition stObj)
        {
            stLocker = stObj;
        }

        public UserInputLocker(UnityWebRequest uwrObj)
        {
            uwrLocker = uwrObj;
        }

        public bool Equals(UserInputLocker other) 
        {
            return other != null &&
                mbLocker == other.mbLocker &&
                stLocker == other.stLocker &&
                uwrLocker == other.uwrLocker;
        }

        public bool IsNull()
        {
            return mbLocker == null &&
                stLocker == null &&
                uwrLocker == null;
        }
    }

    public static class UIManager
    {
        private static Vector2 currentResolution;
        private static float currentAspectRatio;

        private static GameObject uiRootCanvasGO;
        private static CanvasScaler uiRootCanvasGO_canvasScaler;
        private static GameObject uiRootScreenLayerGO;
        private static AspectRatioFitter uiRootScreenLayerGO_aspectRatioFitter;

        private static GameObject uiEventSystem;
        private static EventSystem uiEventSystem_eventSystem;
        private static StandaloneInputModule uiEventSystem_standaloneInputModule;
        private static BaseInput uiEventSystem_baseInput;

        private static GameObject uiScreenLayerGO;
        private static GameObject uiPopupLayerGO;
        private static GameObject uiSubPopupLayerGO;
        private static GameObject uiOverlayLayerGO;
        private static GameObject uiNotificationLayerGO;
        private static GameObject uiDialogLayerGO;

        private static BaseFullScreen prevScreen;
        private static BaseFullScreen currentScreen;
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

        private static List<UserInputLocker> userInputLockers = new List<UserInputLocker>();

        public static void AddUserInputLocker(UserInputLocker locker)
        {
            if (!locker.IsNull())
            {
                if (!userInputLockers.Exists(item => item.Equals(locker)))
                {
                    userInputLockers.Add(locker);
                }
            }

            if (userInputLockers.Count > 0)
            {
                uiEventSystem.SetActive(false);
            }
        }

        public static void RemoveUserInputLocker(UserInputLocker locker)
        {
            if (!locker.IsNull())
            {
                var removeItem = userInputLockers.Find(item => item.Equals(locker));
                if (removeItem != null)
                {
                    userInputLockers.Remove(removeItem);
                }
            }

            if (userInputLockers.Count == 0)
            {
                uiEventSystem.SetActive(true);
            }
        }


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
            UITools.SetStretch(uiRootScreenLayerGO_rectTransform);

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
            UITools.SetStretch(layerGO_rectTransform);

            layerGO.transform.SetSiblingIndex(siblingIndex);
        }

        //Missclick Instantiate
        private static T GetMissclickInstance<T>(Transform parent) where T : BaseMissclick
        {
            var missclickGO = new GameObject(typeof(T).Name);
            var missclickGO_screenRectTransform = missclickGO.AddComponent<RectTransform>();
            missclickGO_screenRectTransform.SetParent(parent, false);
            missclickGO_screenRectTransform.SetAsFirstSibling();
            UITools.SetStretch(missclickGO_screenRectTransform);
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
            var screenGO_rectMask2D = screenGO.AddComponent<RectMask2D>();
            screenGO_rectTransform.SetParent(parent, false);
            UITools.SetStretch(screenGO_rectTransform);
            return screenGO.AddComponent<T>();
        }

        public static T GetScreenInstance<T>() where T : BaseFullScreen
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
            uiEventSystem_eventSystem = uiEventSystem.AddComponent<EventSystem>();
            uiEventSystem_standaloneInputModule = uiEventSystem.AddComponent<StandaloneInputModule>();
            uiEventSystem_baseInput = uiEventSystem.AddComponent<BaseInput>();
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
        public static T GetScreen<T>() where T : BaseFullScreen
        {
            return currentScreen as T;
        }

        public static bool HasScreen<T>() where T : BaseFullScreen
        {
            return currentScreen?.GetType() == typeof(T);
        }

        public static T ShowScreen<T>() where T : BaseFullScreen
        {
            MemoryOprimizer.PrepareChangeScreen();

            HideScreen();

            currentScreen = GetScreenInstance<T>();
            currentScreen.Show();

            var prevSubPopupTr = prevSubPopup?.GetTransition();
            var prevPopupTr = prevPopup?.GetTransition();
            var prevOverlayTr = prevOverlay?.GetTransition();
            var prevScreenTr = prevScreen?.GetTransition();
            var curScreenTr = currentScreen.GetTransition();

            prevSubPopupTr?.LockToEnd(new[] { prevPopupTr, prevOverlayTr, prevScreenTr, curScreenTr });
            prevPopupTr?.LockToPrepare(new[] { prevSubPopupTr, prevOverlayTr, prevScreenTr, curScreenTr });
            prevOverlayTr?.LockToPrepare(new[] { prevSubPopupTr, prevPopupTr, prevScreenTr, curScreenTr });
            prevScreenTr?.LockToPrepare(new[] { prevSubPopupTr, prevPopupTr, prevOverlayTr });
            prevScreenTr?.LockToEnd(new[] { curScreenTr });
            curScreenTr.LockToPrepare(new[] { prevSubPopupTr, prevPopupTr, prevOverlayTr, prevScreenTr });

            curScreenTr.AddEndListener(() => 
            {
                MemoryOprimizer.ChangeScreen();
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

            prevSubPopupTr?.LockToEnd(new[] { prevPopupTr, prevOverlayTr, prevScreenTr });
            prevPopupTr?.LockToPrepare(new[] { prevSubPopupTr, prevOverlayTr, prevScreenTr });
            prevOverlayTr?.LockToPrepare(new[] { prevSubPopupTr, prevPopupTr, prevScreenTr });
            prevScreenTr?.LockToPrepare(new[] { prevSubPopupTr, prevPopupTr, prevOverlayTr });
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

            var prevSubPopupTr = prevSubPopup?.GetTransition();
            var prevPopupTr = prevPopup?.GetTransition();
            var curPopupTr = currentPopup.GetTransition();

            curPopupTr.AddStartListener(() => 
            {
                currentPopup.ShowMissclick();
            });

            prevSubPopupTr?.LockToEnd(new[] { prevPopupTr, curPopupTr });
            prevPopupTr?.LockToPrepare(new[] { prevSubPopupTr });
            prevPopupTr?.LockToEnd(new[] { curPopupTr });
            curPopupTr.LockToPrepare(new[] { prevSubPopupTr, prevPopupTr });

            return currentPopup as T;
        }

        public static void HidePopup()
        {
            prevPopup = currentPopup;
            currentPopup = null;
            prevPopup?.Hide();

            HideSubPopup();

            var prevSubPopupTr = prevSubPopup?.GetTransition();
            var prevPopupTr = prevPopup?.GetTransition();

            prevPopupTr?.AddStartListener(() => 
            {
                HidePopupMissclick();
            });

            prevSubPopupTr?.LockToEnd(new[] { prevPopupTr });
            prevPopupTr?.LockToPrepare(new[] { prevSubPopupTr });
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

            var prevSubPopupTr = prevSubPopup?.GetTransition();
            var curSubPopupTr = currentSubPopup.GetTransition();

            curSubPopupTr.AddStartListener(() => 
            {
                currentSubPopup.ShowMissclick();
            });

            prevSubPopupTr?.LockToEnd(new[] { curSubPopupTr });
            curSubPopupTr.LockToPrepare(new[] { prevSubPopupTr });

            return currentSubPopup as T;
        }

        public static void HideSubPopup()
        {
            prevSubPopup = currentSubPopup;
            currentSubPopup = null;
            prevSubPopup?.Hide();

            var prevSubPopupTr = prevSubPopup?.GetTransition();

            prevSubPopupTr?.AddStartListener(() => 
            {
                HideSubPopupMissclick();
            });
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

            var prevOverlayTr = prevOverlay?.GetTransition();
            var curOverlayTr = currentOverlay.GetTransition();

            curOverlayTr.AddStartListener(() => 
            {
                currentOverlay.ShowMissclick();
            });

            prevOverlayTr?.LockToEnd(new[] { curOverlayTr });
            curOverlayTr.LockToPrepare(new[] { prevOverlayTr });

            return currentOverlay as T;
        }

        public static void HideOverlay()
        {
            prevOverlay = currentOverlay;
            currentOverlay = null;
            prevOverlay?.Hide();

            var prevOverlayTr = prevOverlay?.GetTransition();

            prevOverlayTr?.AddStartListener(() => 
            {
                HideOverlayMissclick();
            });
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

        public static async Task WaitHideNotifications()
        {
            while (currentNotification != null ||
                   prevNotification != null)
            {
                await UniTask.NextFrame();
            }
        }

        public static T ShowNotification<T>() where T : BaseNotification
        {
            prevNotification = currentNotification;
            prevNotification?.Hide();

            currentNotification = GetNotificationInstance<T>();
            currentNotification.Show();

            var prevNotificationTr = prevNotification?.GetTransition();
            var curNotificationTr = currentNotification.GetTransition();

            curNotificationTr.AddStartListener(() => 
            {
                currentNotification.ShowMissclick();
            });

            prevNotificationTr?.LockToEnd(new[] { curNotificationTr });
            curNotificationTr.LockToPrepare(new[] { prevNotificationTr });

            return currentNotification as T;
        }

        public static void HideNotification()
        {
            prevNotification = currentNotification;
            currentNotification = null;
            prevNotification?.Hide();

            var prevNotificationTr = prevNotification?.GetTransition();

            prevNotificationTr?.AddStartListener(() => 
            {
                HideNotificationMissclick();
            });
        }

        //Dialog Layer
        public static void ShowDialogBox(string title, string message, Action yes, Action no = null)
        {
            GameObject.Destroy(currentDialogBoxGO);
            currentDialogBoxGO = new GameObject(typeof(DebugDialogBox).Name);
            currentDialogBoxGO.layer = 5;
            var currentDialogBoxGO_rectTransform = currentDialogBoxGO.AddComponent<RectTransform>();
            currentDialogBoxGO_rectTransform.SetParent(uiDialogLayerGO.transform, false);
            UITools.SetStretch(currentDialogBoxGO_rectTransform);
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
