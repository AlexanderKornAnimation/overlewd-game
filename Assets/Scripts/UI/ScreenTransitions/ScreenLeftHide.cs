using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class ScreenLeftHide : ScreenHide
    {
        protected override void Awake()
        {
            base.Awake();

            screenRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left,
                0.0f, screenRectTransform.rect.width);
        }

        async void Start()
        {
            await screen.BeforeHideAsync();
            OnPrepared();

            await WaitUnlocked();
            OnStart();

            await UITools.LeftHideAsync(screenRectTransform);

            await screen.AfterHideAsync();
            OnEnd();

            Destroy(gameObject);
        }

        protected override void OnStart()
        {
            base.OnStart();
            screen.StartHide();
        }
    }
}
