using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public abstract class BasePopup : BaseScreen
    {
        public BasePopupInData baseInputData { get; protected set; }

        public override void StartShow()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericPopupShow);
        }

        public override void StartHide()
        {
            
        }

        public override void MakeMissclick()
        {
            UIManager.MakePopupMissclick<PopupMissclickColored>();
        }

        public override ScreenShow Show()
        {
            return gameObject.AddComponent<ScreenBottomShow>();
        }

        public override ScreenHide Hide()
        {
            return gameObject.AddComponent<ScreenBottomHide>();
        }

        public void RunShowPopupProcess()
        {
            UIManager.ShowPopupProcess();
        }
    }

    public abstract class BasePopupParent<T> : BasePopup where T : BasePopupInData, new()
    {
        public BasePopupParent()
        {
            baseInputData = new T();
        }

        public T inputData => (T)baseInputData;

        public BasePopup SetData(T data)
        {
            baseInputData = data;
            return this;
        }
    }

    public abstract class BasePopupInData : BaseScreenInData
    {
        public BasePopupInData prevPopupInData { get; set; }

        public new bool IsType<T>() where T : BasePopupInData =>
            base.IsType<T>();
        public new T As<T>() where T : BasePopupInData =>
            base.As<T>();
    }
}
