using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public enum UIEventId
    {
        None,

        ShowPopup,
        HidePopup,

        ShowOverlay,
        HideOverlay,

        ChangeScreenComplete,
        RestoreStateComplete
    }

    public class UIEvent
    {
        public UIEventId id { get; set; } = UIEventId.None;
        public T As<T>() where T : UIEvent =>
            this as T;
        public bool Is<T>() where T : UIEvent =>
            this is T;

        public Type senderType { get; set; }
        public bool SenderTypeIs<T>() =>
            senderType == typeof(T);
        public virtual void Handle()
        {

        }
    }

    public class ChangeScreenCompleteUIEvent : UIEvent
    {
        public override void Handle()
        {
            if (UIManager.currentState.prevScreenState.ScreenTypeIs<LoadingScreen>())
            {
                if (GameData.dailyLogin.isValid)
                {
                    if (!GameData.dailyLogin.info.isViewedToday && 
                        GameData.buildings.aerostat.meta.isBuilt)
                    {
                        UIManager.ShowOverlay<DailyLoginOverlay>();
                    }
                }
            }
        }
    }
}
