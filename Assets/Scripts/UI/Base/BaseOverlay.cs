using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public abstract class BaseOverlay : BaseScreen
    {
        public BaseOverlayInData baseInputData { get; protected set; }

        public override void StartShow()
        {
            
        }

        public override void StartHide()
        {
            
        }
        
        public override void MakeMissclick()
        {
            UIManager.MakeOverlayMissclick<OverlayMissclickColored>();
        }

        public override ScreenShow Show()
        {
            return gameObject.AddComponent<ScreenRightShow>();
        }

        public override ScreenHide Hide()
        {
            return gameObject.AddComponent<ScreenRightHide>();
        }

        public void RunShowOverlayProcess()
        {
            UIManager.ShowOverlayProcess();
        }
    }

    public abstract class BaseOverlayParent<T> : BaseOverlay where T : BaseOverlayInData, new()
    {
        public BaseOverlayParent()
        {
            baseInputData = new T();
        }

        public T inputData => (T)baseInputData;

        public BaseOverlay SetData(T data)
        {
            baseInputData = data;
            return this;
        }
    }

    public abstract class BaseOverlayInData : BaseScreenInData
    {
        public new bool IsType<T>() where T : BaseOverlayInData =>
            base.IsType<T>();
        public new T As<T>() where T : BaseOverlayInData =>
            base.As<T>();
    }
}
