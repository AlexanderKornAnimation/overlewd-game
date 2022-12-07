using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public enum UIEventId
    {
        None,

        HidePopup,
        HideOverlay,

        ChangeScreenComplete,
    }

    public class UIEvent
    {
        public UIEventId id { get; set; } = UIEventId.None;
        public Type senderType { get; set; }
        public bool SenderTypeIs<T>() =>
            senderType == typeof(T);
    }
}
