using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class ScreenBottomHide : ScreenHide
    {
        protected override void Awake()
        {
            base.Awake();

            screenRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom,
                0.0f, screenRectTransform.rect.height);
        }

        async void Start()
        {
            await screen.BeforeHideAsync();
            OnPrepared();

            await WaitUnlocked();
            OnStart();

            await UITools.BottomHideAsync(screenRectTransform);

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
