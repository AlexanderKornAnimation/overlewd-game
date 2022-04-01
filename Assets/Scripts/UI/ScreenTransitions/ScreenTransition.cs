using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Overlewd
{
    public abstract class ScreenTransition : MonoBehaviour
    {
        protected RectTransform screenRectTransform;
        protected BaseScreen screen;

        protected virtual void Awake()
        {
            screenRectTransform = GetComponent<RectTransform>();
            screen = GetComponent<BaseScreen>();

            UIManager.AddUserInputLocker(new UserInputLocker(this));
        }

        protected virtual void OnDestroy()
        {
            UIManager.RemoveUserInputLocker(new UserInputLocker(this));
        }

        public virtual async Task PrepareDataAsync()
        {
            await Task.CompletedTask;
        }

        public virtual async Task PrepareMakeAsync()
        {
            await Task.CompletedTask;
        }

        public virtual async Task PrepareAsync()
        {
            await Task.CompletedTask;
        }

        public virtual async Task ProgressAsync()
        {
            await Task.CompletedTask;
        } 
    }

    public abstract class ScreenShow : ScreenTransition
    {
        public override async Task PrepareDataAsync()
        {
            await screen.BeforeShowDataAsync();
        }

        public override async Task PrepareMakeAsync()
        {
            await screen.BeforeShowMakeAsync();
        }

        public override async Task PrepareAsync()
        {
            await screen.BeforeShowAsync();
        }
    }

    public abstract class ScreenHide : ScreenTransition
    {
        public override async Task PrepareDataAsync()
        {
            await screen.BeforeHideDataAsync();
        }

        public override async Task PrepareMakeAsync()
        {
            await screen.BeforeHideMakeAsync();
        }

        public override async Task PrepareAsync()
        {
            await screen.BeforeHideAsync();
        }
    }
}
