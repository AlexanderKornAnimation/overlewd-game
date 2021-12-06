using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Overlewd
{
    public abstract class BaseScreen : MonoBehaviour
    {
        void Start()
        {

        }

        protected virtual void ShowMissclick()
        {

        }

        public virtual async Task PrepareShow()
        {
            await Task.CompletedTask;
        }

        public virtual async Task AfterShow()
        {
            await Task.CompletedTask;
        }

        public virtual async Task PrepareHide()
        {
            await Task.CompletedTask;
        }

        public virtual async void AfterHide()
        {
            await Task.CompletedTask;
        }

        public virtual void Show()
        {
            gameObject.AddComponent<ScreenShow>();
        }

        public virtual void Hide()
        {
            gameObject.AddComponent<ScreenHide>();
        }
    }
}
