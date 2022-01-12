using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Overlewd
{
    public class ScreenTopShow : ScreenShow
    {
        protected override void Awake()
        {
            base.Awake();

            screenRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top,
                -screenRectTransform.rect.height, screenRectTransform.rect.height);
        }

        async void Start()
        {
            await screen.BeforeShowAsync();
            OnPrepared();
        }

        void Update()
        {
            if (!prepared || locked)
                return;

            OnStart();

            time += deltaTimeInc;
            float transitionProgressPercent = time / duration;
            float transitionOffsetPercent = 1.0f - EasingFunction.easeOutExpo(transitionProgressPercent);
            screenRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top,
                -screenRectTransform.rect.height * transitionOffsetPercent,
                screenRectTransform.rect.height);

            if (time > duration)
            {
                UIManager.SetStretch(screenRectTransform);
                OnEnd();
                screen.AfterShow();
                Destroy(this);
            }
        }
    }
}
