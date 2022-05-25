using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public abstract class BaseNotification : BaseScreen
    {
        public virtual BaseNotificationInData baseInputData => null;

        public override void StartShow()
        {
            
        }

        public override void StartHide()
        {
            
        }
        
        public override void MakeMissclick()
        {
            UIManager.MakeNotificationMissclick<NotificationMissclickColored>();
        }

        public override ScreenShow Show()
        {
            return gameObject.AddComponent<ScreenImmediateShow>();
        }

        public override ScreenHide Hide()
        {
            return gameObject.AddComponent<ScreenImmediateHide>();
        }

        public void RunShowNotificationProcess()
        {
            UIManager.ShowNotificationProcess();
        }
    }

    public abstract class BaseNotificationParent<T> : BaseNotification where T : BaseNotificationInData
    {
        public T inputData { get; private set; }
        public override BaseNotificationInData baseInputData => inputData;

        public BaseNotification SetData(T data)
        {
            inputData = data;
            return this;
        }
    }

    public abstract class BaseNotificationInData : BaseScreenInData
    {
        public new bool IsType<T>() where T : BaseNotificationInData =>
            base.IsType<T>();
        public new T As<T>() where T : BaseNotificationInData =>
            base.As<T>();
    }
}
