using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Overlewd
{
    public abstract class BaseTrasition : MonoBehaviour
    {
        protected TransitionSynchronizer transitionSync;
        protected RectTransform screenRectTransform;

        protected float duration = 0.3f;
        protected float time = 0.0f;

        protected bool prepared = false;

        protected virtual void Awake()
        {
            transitionSync = GetComponent<TransitionSynchronizer>();
            screenRectTransform = GetComponent<RectTransform>();
        }
    }

    public abstract class BaseShowTrasition : BaseTrasition
    {
        protected async Task WaitPrepareShowAsync()
        {
            await transitionSync.PrepareShowAsync();
            prepared = true;
        }

        protected async Task WaitAfterShowAsync()
        {
             await transitionSync.AfterShowAsync();
        }
    }
    public abstract class BaseHideTrasition : BaseTrasition
    {
        protected async Task WaitPrepareHideAsync()
        {
            await transitionSync.PrepareHideAsync();
            prepared = true;
        }

        protected async Task WaitAfterHideAsync()
        {
            await transitionSync.AfterHideAsync();
        }
    }

    public abstract class TransitionSynchronizer : MonoBehaviour
    {
        protected bool preparedShow = false;
        protected bool preparedHide = false;

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

        public virtual async Task PrepareShowAsync()
        {
            await PrepareShowOperationsAsync();
            preparedShow = true;
        }

        public virtual async Task AfterShowAsync()
        {
            await AfterShowOperationsAsync();
        }

        public virtual async Task PrepareHideAsync()
        {
            await PrepareHideOperationsAsync();
            preparedHide = true;
        }

        public virtual async Task AfterHideAsync()
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

        public bool IsTransitionState()
        {
            return GetComponent<BaseTrasition>() != null;
        }

        public bool IsShowTransitionState()
        {
            return GetComponent<BaseShowTrasition>() != null;
        }

        public bool IsHideTransitionState()
        {
            return GetComponent<BaseHideTrasition>() != null;
        }
    }
}
