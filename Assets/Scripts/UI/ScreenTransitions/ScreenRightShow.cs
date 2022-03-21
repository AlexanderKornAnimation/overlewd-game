using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{

    public class ScreenRightShow : ScreenShow
    {
        protected override void Awake()
        {
            base.Awake();

            screenRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right,
                -screenRectTransform.rect.width, screenRectTransform.rect.width);
        }

        async void Start()
        {
            await screen.BeforeShowAsync();
            OnPrepared();

            await WaitUnlocked();
            OnStart();

            await UIHelper.RightShowAsync(screenRectTransform);
            UIManager.SetStretch(screenRectTransform);

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
