using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Overlewd
{
    public abstract class BaseNotification : BaseScreen
    {
        public BaseNotificationInData baseInputData { get; protected set; }

        public override void StartShow()
        {
            
        }

        public override void StartHide()
        {
            
        }
        
        public override void OnMissclick()
        {
            UIManager.HideNotification();
        }

        public override ScreenShow Show()
        {
            return gameObject.AddComponent<ScreenImmediateShow>();
        }

        public override ScreenHide Hide()
        {
            return gameObject.AddComponent<ScreenImmediateHide>();
        }

        public void DoShow() => UIManager.ShowNotification(this);
        public async Task DoShowAsync() => await UIManager.ShowNotificationAsync(this);
    }

    public abstract class BaseNotificationParent<T> : BaseNotification where T : BaseNotificationInData
    {
        public T inputData => (T)baseInputData;

        public BaseNotification SetData(T data)
        {
            baseInputData = data;
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
