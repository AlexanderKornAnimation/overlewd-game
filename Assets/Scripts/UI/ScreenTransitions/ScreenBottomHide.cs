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
        }

        void Update()
        {
            if (!prepared || locked)
                return;

            OnStart();

            time += deltaTimeInc;
            float transitionProgressPercent = time / duration;
            float transitionOffsetPercent = EasingFunction.easeInOutQuad(transitionProgressPercent);
            screenRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom,
                -screenRectTransform.rect.height * transitionOffsetPercent,
                screenRectTransform.rect.height);

            if (time > duration)
            {
                OnEnd();
                screen.AfterHide();
                Destroy(gameObject);
            }
        }
    }
}
