using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        
        public override BaseMissclick MakeMissclick()
        {
            return UIManager.MakeOverlayMissclick<OverlayMissclickColored>();
        }

        public override ScreenShow Show()
        {
            return gameObject.AddComponent<ScreenRightShow>();
        }

        public override ScreenHide Hide()
        {
            return gameObject.AddComponent<ScreenRightHide>();
        }

        public void DoShow() => UIManager.ShowOverlay(this);
        public async Task DoShowAsync() => await UIManager.ShowOverlayAsync(this);
    }

    public abstract class BaseOverlayParent<T> : BaseOverlay where T : BaseOverlayInData
    {
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
