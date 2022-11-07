using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Overlewd
{
    public abstract class BaseFullScreen : BaseScreen
    {
        public BaseFullScreenInData baseInputData { get; protected set; }
        public void DoShow() => UIManager.ShowScreen(this);
        public async Task DoShowAsync() => await UIManager.ShowScreenAsync(this);
    }

    public abstract class BaseFullScreenParent<T> : BaseFullScreen where T : BaseFullScreenInData
    {
        public T inputData => (T)baseInputData;

        public BaseFullScreen SetData(T data)
        {
            baseInputData = data;
            return this;
        }
    }

    public abstract class BaseFullScreenInData : BaseScreenInData
    {
        public BaseFullScreenInData prevScreenInData { get; set; }

        public new bool IsType<T>() where T : BaseFullScreenInData =>
            base.IsType<T>();
        public new T As<T>() where T : BaseFullScreenInData =>
            base.As<T>();
    }
}
