using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public abstract class BaseFullScreen : BaseScreen
    {
        public virtual BaseFullScreenInData baseInputData => null;

        public void RunShowScreenProcess()
        {
            UIManager.ShowScreenProcess();
        }
    }

    public abstract class BaseFullScreenParent<T> : BaseFullScreen where T : BaseFullScreenInData, new()
    {
        public T inputData { get; private set; } = new T();
        public override BaseFullScreenInData baseInputData => inputData;

        public BaseFullScreen SetData(T data)
        {
            inputData = data;
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
