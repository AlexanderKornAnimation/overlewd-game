using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Overlewd
{
    public abstract class BaseScreen : MonoBehaviour
    {
        protected bool preparedShow = false;
        protected bool preparedHide = false;

        void Start()
        {

        }

        protected virtual void ShowMissclick()
        {

        }

        protected virtual async Task PrepareShowOperationsAsync()
        {
            await Task.CompletedTask;
        }

        protected virtual async Task AfterShowOperationsAsync()
        {
            await Task.CompletedTask;
        }

        protected virtual async Task PrepareHideOperationsAsync()
        {
            await Task.CompletedTask;
        }

        protected virtual async Task AfterHideOperationsAsync()
        {
            await Task.CompletedTask;
        }

        public async Task PrepareShowAsync()
        {
            await PrepareShowOperationsAsync();
            preparedShow = true;
            ShowMissclick();
        }

        public async Task AfterShowAsync()
        {
            await AfterShowOperationsAsync();
        }

        public async Task PrepareHideAsync()
        {
            await PrepareHideOperationsAsync();
            preparedHide = true;
        }

        public async Task AfterHideAsync()
        {
            await AfterHideOperationsAsync();
        }

        public async Task WaitPreparedShowAsync()
        {
            while (!preparedShow)
            {
                await Task.Delay(10);
            }
        }

        public async Task WaitPreparedHideAsync()
        {
            while (!preparedHide)
            {
                await Task.Delay(10);
            }
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
