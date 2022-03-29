using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{

    public class ScreenLeftShow : ScreenShow
    {
        protected override void Awake()
        {
            base.Awake();

            screenRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left,
                -screenRectTransform.rect.width, screenRectTransform.rect.width);
        }

        async void Start()
        {
            await screen.BeforeShowAsync();
            OnPrepared();

            await WaitUnlocked();
            OnStart();

            await UITools.LeftShowAsync(screenRectTransform);
            UITools.SetStretch(screenRectTransform);

            await screen.AfterShowAsync();
            OnEnd();

            Destroy(this);
        }

        protected override void OnStart()
        {
            base.OnStart();
            screen.StartShow();
        }
    }
}
