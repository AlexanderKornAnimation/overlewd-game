using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;
using System.Linq;

namespace Overlewd
{
    public class UIEvent
    {
        public Type type { get; set; } = Type.None;
        public BaseScreen uiSender { get; set; }
        public System.Type uiSenderType { get; set; }

        public enum Type
        {
            None,
            RestoreScreenFocusAfterPopup,
            RestoreScreenFocusAfterOverlay
        }
    }

    public class UserInputLocker
    {
        public MonoBehaviour mbLocker { get; private set; }
        public ScreenTransition stLocker { get; private set; }
        public UnityWebRequest uwrLocker { get; private set; }

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
        private static RectTransform uiRootCanvasGO_rectTransform;
        private static Canvas uiRootCanvasGO_canvas;
        private static GameObject uiRootScreenLayerGO;
        private static AspectRatioFitter uiRootScreenLayerGO_aspectRatioFitter;

        private static GameObject uiEventSystem;
        private static EventSystem uiEventSystem_eventSystem;
        private static StandaloneInputModule uiEventSystem_standaloneInputModule;
        private static BaseInput uiEventSystem_baseInput;

        private static GameObject uiScreenLayerGO;
        private static GameObject uiPopupLayerGO;
        private static GameObject uiOverlayLayerGO;
        private static GameObject uiNotificationLayerGO;
        private static GameObject uiSystemNotifsLayerGO;

        private static BaseFullScreen screen;
        private static BasePopup popup;
        private static BaseMissclick popupMiss;
        private static BaseOverlay overlay;
        private static BaseMissclick overlayMiss;
        private static BaseNotification notif;
        private static BaseMissclick notifMiss;

        public static event Action<GameDataEvent> widgetsGameDataListeners;

        public enum UserInputLockerMode
        {
            Manual,
            Auto
        }
        private static UserInputLockerMode userInputLockerMode = UserInputLockerMode.Auto;
        public static void SetUserInputLockerMode(UserInputLockerMode mode, bool lockUserInput = false)
        {
            userInputLockerMode = mode;
            switch (userInputLockerMode)
            {
                case UserInputLockerMode.Manual:
                    uiEventSystem.SetActive(!lockUserInput);
                    break;
                case UserInputLockerMode.Auto:
                    uiEventSystem.SetActive(userInputLockers.Count == 0);
                    break;
            }
        }

        private static List<UserInputLocker> userInputLockers = new List<UserInputLocker>();
        public static void PushUserInputLocker(UserInputLocker locker)
        {
            if (!locker.IsNull())
            {
                if (!userInputLockers.Exists(item => item.Equals(locker)))
                {
                    userInputLockers.Add(locker);
                    switch (userInputLockerMode)
                    {
                        case UserInputLockerMode.Manual:
                            break;
                        case UserInputLockerMode.Auto:
                            uiEventSystem.SetActive(false);
                            break;
                    }
                }
            }
        }

        public static void PopUserInputLocker(UserInputLocker locker)
        {
            if (!locker.IsNull())
            {
                var removeItem = userInputLockers.Find(item => item.Equals(locker));
                if (removeItem != null)
                {
                    userInputLockers.Remove(removeItem);
                    switch (userInputLockerMode)
                    {
                        case UserInputLockerMode.Manual:
                            break;
                        case UserInputLockerMode.Auto:
                            uiEventSystem.SetActive(userInputLockers.Count == 0);
                            break;
                    }
                }
            }
        }


        private static Vector2 SelectResolution()
        {
            var aspectRatio = (float)Screen.width / (float)Screen.height;
            var resoulutions = new List<Vector2>()
            {
                new Vector2(1920, 1080),
                new Vector2(2160, 1080),
                new Vector2(2400, 1080)
            };
            return resoulutions.OrderBy(res => Mathf.Abs(res.x / res.y - aspectRatio)).First();
        }

        public static void ChangeResolution()
        {
            currentResolution = SelectResolution();
            currentAspectRatio = currentResolution.x / currentResolution.y;

            uiRootCanvasGO_canvasScaler.referenceResolution = currentResolution;
            uiRootScreenLayerGO_aspectRatioFitter.aspectRatio = currentAspectRatio;
        }

        public static Rect GetScreenWorldRect()
        {
            return uiRootScreenLayerGO.GetComponent<RectTransform>().WorldRect();
        }

        public static float GetScreenScaleFactor()
        {
            return uiRootCanvasGO_canvas.scaleFactor;
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

        private static GameObject ConfigureLayer(string name)
        {
            var layerGO = new GameObject(name);
            layerGO.layer = 5;
            var layerGO_rectTransform = layerGO.AddComponent<RectTransform>();
            layerGO_rectTransform.SetParent(uiRootScreenLayerGO.transform, false);
            UITools.SetStretch(layerGO_rectTransform);
            return layerGO;
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

        public static T GetOverlayMissclickInstance<T>() where T : OverlayMissclick
        {
            return GetMissclickInstance<T>(uiOverlayLayerGO.transform);
        }

        public static T GetNotificationMissclickInstance<T>() where T : NotificationMissclick
        {
            return GetMissclickInstance<T>(uiNotificationLayerGO.transform);
        }

        //Screen Instantiate
        private static Component GetScreenInstance(Type type, Transform parent)
        {
            var screenGO = new GameObject(type.Name);
            screenGO.layer = 5;
            var screenGO_rectTransform = screenGO.AddComponent<RectTransform>();
            var screenGO_rectMask2D = screenGO.AddComponent<RectMask2D>();
            screenGO_rectTransform.SetParent(parent, false);
            UITools.SetStretch(screenGO_rectTransform);
            return screenGO.AddComponent(type);
        }
        private static T GetScreenInstance<T>(Transform parent) where T : BaseScreen =>
            GetScreenInstance(typeof(T), parent) as T;

        public static BaseFullScreen GetScreenInstance(Type type) =>
            GetScreenInstance(type, uiScreenLayerGO.transform) as BaseFullScreen;
        public static T GetScreenInstance<T>() where T : BaseFullScreen =>
            GetScreenInstance<T>(uiScreenLayerGO.transform);

        public static BasePopup GetPopupInstance(Type type) =>
            GetScreenInstance(type, uiPopupLayerGO.transform) as BasePopup;
        public static T GetPopupInstance<T>() where T : BasePopup =>
            GetScreenInstance<T>(uiPopupLayerGO.transform);

        public static BaseOverlay GetOverlayInstance(Type type) =>
            GetScreenInstance(type, uiOverlayLayerGO.transform) as BaseOverlay;
        public static T GetOverlayInstance<T>() where T : BaseOverlay =>
            GetScreenInstance<T>(uiOverlayLayerGO.transform);

        public static T GetNotificationInstance<T>() where T : BaseNotification =>
            GetScreenInstance<T>(uiNotificationLayerGO.transform);

        public static void Initialize()
        {
#if UNITY_ANDROID
            Application.targetFrameRate = 60;
#endif

            currentResolution = SelectResolution();
            currentAspectRatio = currentResolution.x / currentResolution.y;

            uiRootCanvasGO = new GameObject("UIManagerRootCanvas");
            uiRootCanvasGO.layer = 5;
            uiRootCanvasGO_rectTransform = uiRootCanvasGO.AddComponent<RectTransform>();
            uiRootCanvasGO_canvas = uiRootCanvasGO.AddComponent<Canvas>();
            uiRootCanvasGO_canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            uiRootCanvasGO_canvasScaler = uiRootCanvasGO.AddComponent<CanvasScaler>();
            uiRootCanvasGO_canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            uiRootCanvasGO_canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
            uiRootCanvasGO_canvasScaler.referenceResolution = currentResolution;
            var uiRootCanvasGO_graphicRaycaster = uiRootCanvasGO.AddComponent<GraphicRaycaster>();

            ConfigureRootScreenLayer();

            uiScreenLayerGO = ConfigureLayer("UIScreenLayer");
            uiPopupLayerGO = ConfigureLayer("UIPopupLayer");
            uiOverlayLayerGO = ConfigureLayer("UIOverlayLayer");
            uiNotificationLayerGO = ConfigureLayer("UINotificationLayer");
            uiSystemNotifsLayerGO = ConfigureLayer("UISystemNotifsLayer");

            uiEventSystem = new GameObject("UIManagerEventSystem");
            uiEventSystem_eventSystem = uiEventSystem.AddComponent<EventSystem>();
            uiEventSystem_standaloneInputModule = uiEventSystem.AddComponent<StandaloneInputModule>();
            uiEventSystem_baseInput = uiEventSystem.AddComponent<BaseInput>();
        }

        public static void ThrowGameDataEvent(GameDataEvent eventData)
        {
            screen?.OnGameDataEvent(eventData);
            popup?.OnGameDataEvent(eventData);
            overlay?.OnGameDataEvent(eventData);

            widgetsGameDataListeners?.Invoke(eventData);
        }

        public static void ThrowUIEvent(UIEvent eventData)
        {
            screen?.OnUIEvent(eventData);
            popup?.OnUIEvent(eventData);
            overlay?.OnUIEvent(eventData);
        }

        //transition tools
        private static async Task WaitScreensPrepare(List<BaseScreen> screens)
        {
            foreach (var screen in screens)
            {
                var screenTr = screen?.GetTransition();
                if (screenTr != null) await screenTr.PrepareDataAsync();
            }
            foreach (var screen in screens)
            {
                var screenTr = screen?.GetTransition();
                if (screenTr != null) await screenTr.PrepareMakeAsync();
            }
        }

        private static async Task WaitScreenTransitions(List<BaseScreen> screens, List<BaseMissclick> missclicks)
        {
            var prepareTasks = new List<Task>();
            foreach (var screen in screens)
            {
                var screenTr = screen?.GetTransition();
                if (screenTr != null) prepareTasks.Add(screenTr.PrepareAsync());
            }
            await Task.WhenAll(prepareTasks);

            var processTasks = new List<Task>();
            foreach (var screen in screens)
            {
                var screenTr = screen?.GetTransition();
                if (screenTr != null) processTasks.Add(screenTr.ProgressAsync());
            }
            foreach (var missclick in missclicks)
            {
                var missclickTr = missclick?.GetTransition();
                if (missclickTr != null) processTasks.Add(missclickTr.ProgressAsync());
            }
            await Task.WhenAll(processTasks);
        }

        //UI states
        private const int PrevStatesStackCapacity = 20;
        public class State
        {
            public Type screenType { get; set; }
            public BaseFullScreenInData screenInData { get; set; }
            public bool ScreenTypeIs<T>() where T : BaseFullScreen =>
                screenType == typeof(T);

            public bool showPopup { get; set; } = true;
            public Type popupType { get; set; }
            public BasePopupInData popupInData { get; set; }
            public bool PopupTypeIs<T>() where T : BasePopup =>
                popupType == typeof(T);

            public bool showOverlay { get; set; } = true;
            public Type overlayType { get; set; }
            public BaseOverlayInData overlayInData { get; set; }
            public bool OverlayTypeIs<T>() where T : BaseOverlay =>
                overlayType == typeof(T);

            public State prevState { get; set; }
        }

        private static LinkedList<State> prevStatesStack = new LinkedList<State>();
        private static void PushPrevState(State state)
        {
            if (state == null)
                return;

            prevStatesStack.AddFirst(state);
            while (prevStatesStack.Count > PrevStatesStackCapacity)
            {
                prevStatesStack.Last.Previous.Value.prevState = null;
                prevStatesStack.RemoveLast();
            }
        }
        private static State PopPrevState()
        {
            var popState = PeakPrevState();
            prevStatesStack.RemoveFirst();
            return popState;
        }
        private static State PeakPrevState() =>
            prevStatesStack.First?.Value;

        public static State currentState => screen != null ?
            new State
            {
                screenType = screen.GetType(),
                screenInData = screen.baseInputData,
                popupType = popup?.GetType(),
                popupInData = popup?.baseInputData,
                overlayType = overlay?.GetType(),
                overlayInData = overlay?.baseInputData,
                prevState = PeakPrevState()
            } : null;
        public static bool pushPrevState { get; set; } = true;
        public static async void ToState(State state)
        {
            if (state == null)
                return;

            var screen = MakeScreen(state.screenType);
            screen.baseInputData = state.screenInData;
            await ShowScreenAsync(screen);

            if (state.popupType != null && state.showPopup)
            {
                var popup = MakePopup(state.popupType);
                popup.baseInputData = state.popupInData;
                await ShowPopupAsync(popup);
            }

            if (state.overlayType != null && state.showOverlay)
            {
                var overlay = MakeOverlay(state.overlayType);
                overlay.baseInputData = state.overlayInData;
                await ShowOverlayAsync(overlay);
            }
        }
        public static void ToPrevState()
        {
            var prevState = PopPrevState();
            pushPrevState = false;
            ToState(prevState);
        }
        public static void RemakeState()
        {
            var curState = currentState;
            pushPrevState = false;
            ToState(curState);
        }

        //Screen Layer
        public static T GetScreen<T>() where T : BaseFullScreen => screen as T;
        public static bool HasScreen<T>() where T : BaseFullScreen => screen?.GetType() == typeof(T);
        public static BaseFullScreenInData screenInData => screen?.baseInputData;
        private static BaseFullScreen MakeScreen(Type type) => GetScreenInstance(type);
        public static T MakeScreen<T>() where T : BaseFullScreen => GetScreenInstance<T>();
        public static void ShowScreen<T>() where T : BaseFullScreen => ShowScreen(MakeScreen<T>());
        public static async void ShowScreen(BaseFullScreen _screen) => await ShowScreenAsync(_screen);
        private static async Task ShowScreenAsync(BaseFullScreen _screen)
        {
            if (_screen == null)
                return;

            if (pushPrevState)
            {
                PushPrevState(currentState);
            }
            //restore push prev UI states to stack
            pushPrevState = true;

            MemoryOprimizer.PrepareChangeScreen();

            var screenPrev = screen;
            screen = _screen;
            var popupPrev = popup;
            var popupMissPrev = popupMiss;
            popup = null;
            popupMiss = null;
            var overlayPrev = overlay;
            var overlayMissPrev = overlayMiss;
            overlay = null;
            overlayMiss = null;

            screenPrev?.Hide();
            popupPrev?.Hide();
            popupMissPrev?.Hide();
            overlayPrev?.Hide();
            overlayMissPrev?.Hide();
            screen?.Show();

            await WaitScreensPrepare(new List<BaseScreen>
            {
                popupPrev,
                overlayPrev,
                screenPrev,
                screen
            });
            await WaitScreenTransitions(new List<BaseScreen> { popupPrev, overlayPrev, screenPrev },
                                        new List<BaseMissclick> { popupMissPrev, overlayMissPrev });
            await WaitScreenTransitions(new List<BaseScreen> { screen },
                                        new List<BaseMissclick>());

            MemoryOprimizer.ChangeScreen();
        }

        //Popup Layer
        public static T GetPopupMissclick<T>() where T : PopupMissclick => popupMiss as T;
        public static bool HasPopupMissclick<T>() where T : PopupMissclick => popupMiss?.GetType() == typeof(T);
        public static T MakePopupMissclick<T>() where T : PopupMissclick
        {
            var miss = HasPopupMissclick<T>() ? popupMiss : GetPopupMissclickInstance<T>();
            miss.transform.SetAsFirstSibling();
            return miss as T;
        }
        public static T GetPopup<T>() where T : BasePopup => popup as T;
        public static bool HasPopup<T>() where T : BasePopup => popup?.GetType() == typeof(T);
        private static BasePopup MakePopup(Type type) => GetPopupInstance(type);
        public static T MakePopup<T>() where T : BasePopup => GetPopupInstance<T>();
        public static void ShowPopup<T>() where T : BasePopup => ShowPopup(MakePopup<T>());
        public static async void ShowPopup(BasePopup _popup) => await ShowPopupAsync(_popup);
        public static async Task ShowPopupAsync(BasePopup _popup)
        {
            if (_popup == null)
                return;

            var popupPrev = popup;
            popup = _popup;
            var popupMissPrev = popupMiss;
            popupMiss = popup?.MakeMissclick();

            popupPrev?.Hide();
            popup?.Show();
            if (popupMiss != popupMissPrev)
            {
                popupMissPrev?.Hide();
                popupMiss?.Show();
            }

            await WaitScreensPrepare(new List<BaseScreen>
            {
                popupPrev,
                popup
            });
            await WaitScreenTransitions(new List<BaseScreen> { popupPrev },
                                        new List<BaseMissclick> { popupMissPrev });
            await WaitScreenTransitions(new List<BaseScreen> { popup },
                                        new List<BaseMissclick> { popupMiss });
        }
        public static async void HidePopup() => await HidePopupAsync();
        public static async Task HidePopupAsync()
        {
            if (popup == null)
                return;

            var uiEventData = new UIEvent
            {
                type = UIEvent.Type.RestoreScreenFocusAfterPopup,
                uiSenderType = popup?.GetType()
            };

            popup?.Hide();
            popupMiss?.Hide();

            await WaitScreensPrepare(new List<BaseScreen>
            {
                popup
            });
            await WaitScreenTransitions(new List<BaseScreen> { popup },
                                        new List<BaseMissclick> { popupMiss });

            ThrowUIEvent(uiEventData);

            popup = null;
            popupMiss = null;
        }

        //Overlay Layer
        public static T GetOverlayMissclick<T>() where T : OverlayMissclick => overlayMiss as T;
        public static bool HasOverlayMissclick<T>() where T : OverlayMissclick => overlayMiss?.GetType() == typeof(T);
        public static T MakeOverlayMissclick<T>() where T : OverlayMissclick
        {
            var miss = HasOverlayMissclick<T>() ? overlayMiss : GetOverlayMissclickInstance<T>();
            miss.transform.SetAsFirstSibling();
            return miss as T;
        }
        public static T GetOverlay<T>() where T : BaseOverlay => overlay as T;
        public static bool HasOverlay<T>() where T : BaseOverlay => overlay?.GetType() == typeof(T);
        private static BaseOverlay MakeOverlay(Type type) => GetOverlayInstance(type);
        public static T MakeOverlay<T>() where T : BaseOverlay => GetOverlayInstance<T>();
        public static void ShowOverlay<T>() where T : BaseOverlay => ShowOverlay(MakeOverlay<T>());
        public static async void ShowOverlay(BaseOverlay _overlay) => await ShowOverlayAsync(_overlay);
        public static async Task ShowOverlayAsync(BaseOverlay _overlay)
        {
            if (_overlay == null)
                return;

            var overlayPrev = overlay;
            overlay = _overlay;
            var overlayMissPrev = overlayMiss;
            overlayMiss = overlay?.MakeMissclick();

            overlayPrev?.Hide();
            overlay?.Show();
            if (overlayMiss != overlayMissPrev)
            {
                overlayMissPrev?.Hide();
                overlayMiss?.Show();
            }

            await WaitScreensPrepare(new List<BaseScreen>
            {
                overlayPrev,
                overlay
            });
            await WaitScreenTransitions(new List<BaseScreen> { overlayPrev },
                                        new List<BaseMissclick> { overlayMissPrev });
            await WaitScreenTransitions(new List<BaseScreen> { overlay },
                                        new List<BaseMissclick> { overlayMiss });
        }
        public static async void HideOverlay() => await HideOverlayAsync();
        private static async Task HideOverlayAsync()
        {
            if (overlay == null)
                return;

            var uiEventData = new UIEvent
            {
                type = UIEvent.Type.RestoreScreenFocusAfterOverlay,
                uiSenderType = overlay.GetType()
            };

            overlay?.Hide();
            overlayMiss?.Hide();

            await WaitScreensPrepare(new List<BaseScreen> { overlay });
            await WaitScreenTransitions(new List<BaseScreen> { overlay },
                                        new List<BaseMissclick> { overlayMiss });

            ThrowUIEvent(uiEventData);

            overlay = null;
            overlayMiss = null;
        }

        //Notification Layer
        public static T GetNotificationMissclick<T>() where T : NotificationMissclick => notifMiss as T;
        public static bool HasNotificationMissclick<T>() where T : NotificationMissclick => notifMiss?.GetType() == typeof(T);
        public static T MakeNotificationMissclick<T>() where T : NotificationMissclick
        {
            var miss = HasNotificationMissclick<T>() ? notifMiss : GetNotificationMissclickInstance<T>();
            miss.transform.SetAsFirstSibling();
            return miss as T;
        }
        public static T GetNotification<T>() where T : BaseNotification => notif as T;
        public static bool HasNotification<T>() where T : BaseNotification => notif?.GetType() == typeof(T);
        public static async Task WaitHideNotifications()
        {
            while (notif != null)
                await UniTask.NextFrame();
        }
        public static T MakeNotification<T>() where T : BaseNotification => GetNotificationInstance<T>();
        public static void ShowNotification<T>() where T : BaseNotification => MakeNotification<T>().DoShow();
        public static async void ShowNotification(BaseNotification _notif) => await ShowNotificationAsync(_notif);
        public static async Task ShowNotificationAsync(BaseNotification _notif)
        {
            if (_notif == null)
                return;

            var notifPrev = notif;
            notif = _notif;
            var notifMissPrev = notifMiss;
            notifMiss = notif?.MakeMissclick();

            notifPrev?.Hide();
            notif?.Show();
            if (notifMiss != notifMissPrev)
            {
                notifMissPrev?.Hide();
                notifMiss?.Show();
            }

            await WaitScreensPrepare(new List<BaseScreen> 
            {
                notifPrev,
                notif
            });
            await WaitScreenTransitions(new List<BaseScreen> { notifPrev },
                                        new List<BaseMissclick> { notifMissPrev });
            await WaitScreenTransitions(new List<BaseScreen> { notif },
                                        new List<BaseMissclick> { notifMiss });
        }
        public static async void HideNotification() => await HideNotificationAsync();
        private static async Task HideNotificationAsync()
        {
            if (notif == null)
                return;

            notif?.Hide();
            notifMiss?.Hide();

            await WaitScreensPrepare(new List<BaseScreen> { notif });
            await WaitScreenTransitions(new List<BaseScreen> { notif },
                                        new List<BaseMissclick> { notifMiss });

            notif = null;
            notifMiss = null;
        }

        //System notifications layer
        public static void ShowServerConnectionNotif()
        {
            var notif = uiSystemNotifsLayerGO.GetComponentInChildren<ServerConnectionNotif>();
            if (notif != null)
            {
                notif.Show();
            }
            else
            {
                ServerConnectionNotif.GetInstance(uiSystemNotifsLayerGO.transform).Show(); ;
            }
        }

        public static void HideServerConnectionNotif()
        {
            uiSystemNotifsLayerGO.GetComponentInChildren<ServerConnectionNotif>()?.Hide();
        }

        public static T MakeSystemNotif<T>() where T : BaseSystemNotif
        {
            if (uiSystemNotifsLayerGO.GetComponentsInChildren<T>().Count() > 0)
            {
                return BaseSystemNotif.GetInstance<T>(uiSystemNotifsLayerGO.transform);
            }
            var notif = BaseSystemNotif.GetInstance<T>(uiSystemNotifsLayerGO.transform);
            notif.Show();
            return notif;
        }

        public static void PeakSystemNotif()
        {
            uiSystemNotifsLayerGO.GetComponentsInChildren<BaseSystemNotif>(true).FirstOrDefault()?.Show();
        }

        public static bool HasSystemNotif<T>() where T : BaseSystemNotif
        {
            return uiSystemNotifsLayerGO.GetComponentsInChildren<T>(true).Count() > 0;
        }
    }

}
