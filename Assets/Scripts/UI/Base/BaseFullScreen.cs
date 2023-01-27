using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Overlewd
{
    public abstract class BaseFullScreen : BaseScreen
    {
        public BaseFullScreenInData baseInputData { get; set; }
        public void DoShow() => UIManager.ShowScreen(this);
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

    }
}
