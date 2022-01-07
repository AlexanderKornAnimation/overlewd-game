using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Overlewd
{
    public abstract class BaseScreen : TransitionSynchronizer
    {
        protected virtual void ShowMissclick()
        {

        }

        public override async Task PrepareShowAsync()
        {
            await PrepareShowOperationsAsync();
            preparedShow = true;
            ShowMissclick();
        }

        public virtual void Show()
        {
            gameObject.AddComponent<BottomShow>();
        }

        public virtual void Hide()
        {
            gameObject.AddComponent<BottomHide>();
        }
    }
}
