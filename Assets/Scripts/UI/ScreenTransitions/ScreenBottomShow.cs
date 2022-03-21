using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Overlewd
{
    public class ScreenBottomShow : ScreenShow
    {
        protected override void Awake()
        {
            base.Awake();

            screenRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom,
                -screenRectTransform.rect.height, screenRectTransform.rect.height);
        }

        async void Start()
        {
            await screen.BeforeShowAsync();
            OnPrepared();

            await WaitUnlocked();
            OnStart();

            await UIHelper.BottomShowAsync(screenRectTransform);
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
