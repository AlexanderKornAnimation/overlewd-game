using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public abstract class BaseSubPopup : BaseScreen
    {
        public virtual BaseSubPopupInData baseInputData => null;

        public override void StartShow()
        {
            
        }

        public override void StartHide()
        {
            
        }
        
        public override void MakeMissclick()
        {
            UIManager.MakeSubPopupMissclick<SubPopupMissclickColored>();
        }

        public override ScreenShow Show()
        {
            return gameObject.AddComponent<ScreenImmediateShow>();
        }

        public override ScreenHide Hide()
        {
            return gameObject.AddComponent<ScreenImmediateHide>();
        }

        public void RunShowSubPopupProcess()
        {
            UIManager.ShowSubPopupProcess();
        }
    }

    public abstract class BaseSubPopupParent<T> : BaseSubPopup where T : BaseSubPopupInData
    {
        public T inputData { get; private set; }
        public override BaseSubPopupInData baseInputData => inputData;

        public BaseSubPopup SetData(T data)
        {
            inputData = data;
            return this;
        }
    }

    public abstract class BaseSubPopupInData : BaseScreenInData
    {
        public new bool IsType<T>() where T : BaseSubPopupInData =>
            base.IsType<T>();
        public new T As<T>() where T : BaseSubPopupInData =>
            base.As<T>();
    }
}
