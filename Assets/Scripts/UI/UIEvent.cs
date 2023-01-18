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
    }
}
