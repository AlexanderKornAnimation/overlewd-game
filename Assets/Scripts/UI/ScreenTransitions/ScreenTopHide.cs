using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class ScreenTopHide : ScreenHide
    {
        protected override void Awake()
        {
            base.Awake();

            screenRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top,
                0.0f, screenRectTransform.rect.height);
        }

        async void Start()
        {
            await screen.BeforeHideAsync();
            OnPrepared();

            await WaitUnlocked();
            OnStart();

            await UIHelper.TopHideAsync(screenRectTransform);

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
