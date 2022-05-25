using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public abstract class BaseOverlay : BaseScreen
    {
        public virtual BaseOverlayInData baseInputData => null;

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

    public abstract class BaseOverlayParent<T> : BaseOverlay where T : BaseOverlayInData
    {
        public T inputData { get; private set; }
        public override BaseOverlayInData baseInputData => inputData;

        public BaseOverlay SetData(T data)
        {
            inputData = data;
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
