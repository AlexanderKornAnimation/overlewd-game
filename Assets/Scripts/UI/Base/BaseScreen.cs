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
            ShowMissclick();
        }

        public async Task AfterShowAsync()
        {
            await AfterShowOperationsAsync();
        }

        public async Task PrepareHideAsync()
        {
            await PrepareHideOperationsAsync();
        }

        public async Task AfterHideAsync()
        {
            await AfterHideOperationsAsync();
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
