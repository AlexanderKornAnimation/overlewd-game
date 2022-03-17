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
        }

        async void Update()
        {
            if (!prepared || locked)
                return;

            OnStart();

            time += deltaTimeInc;
            float transitionProgressPercent = time / duration;
            float transitionOffsetPercent = 1.0f - EasingFunction.easeOutExpo(transitionProgressPercent);
            screenRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left,
                -screenRectTransform.rect.width * transitionOffsetPercent,
                screenRectTransform.rect.width);

            if (time > duration)
            {
                UIManager.SetStretch(screenRectTransform);
                await screen.AfterShowAsync();
                OnEnd();
                Destroy(this);
            }
        }

        protected override void OnStartCalls()
        {
            screen.StartShow();
        }
    }
}
