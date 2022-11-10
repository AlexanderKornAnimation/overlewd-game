using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Overlewd
{
    public abstract class BasePopup : BaseScreen
    {
        public BasePopupInData baseInputData { get; set; }

        public override void StartShow()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericPopupShow);
        }

        public override void StartHide()
        {
            
        }

        public override BaseMissclick MakeMissclick()
        {
            return UIManager.MakePopupMissclick<PopupMissclickColored>();
        }

        public override ScreenShow Show()
        {
            return gameObject.AddComponent<ScreenBottomShow>();
        }

        public override ScreenHide Hide()
        {
            return gameObject.AddComponent<ScreenBottomHide>();
        }

        public void DoShow() => UIManager.ShowPopup(this);
        public async Task DoShowAsync() => await UIManager.ShowPopupAsync(this);
    }

    public abstract class BasePopupParent<T> : BasePopup where T : BasePopupInData
    {
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
