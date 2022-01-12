using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class ScreenRightHide : ScreenHide
    {
        protected override void Awake()
        {
            base.Awake();

            screenRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right,
                0.0f, screenRectTransform.rect.width);
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
            screenRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right,
                -screenRectTransform.rect.width * transitionOffsetPercent,
                screenRectTransform.rect.width);

            if (time > duration)
            {
                OnEnd();
                screen.AfterHide();
                Destroy(gameObject);
            }
        }
    }
}
