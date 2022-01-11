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
            prepared = true;
            preparedTransitionListeners?.Invoke();
        }

        void Update()
        {
            if (!prepared || locked)
                return;

            time += deltaTimeInc;
            float transitionProgressPercent = time / duration;
            float transitionOffsetPercent = EasingFunction.easeInOutQuad(transitionProgressPercent);
            screenRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left,
                -screenRectTransform.rect.width * transitionOffsetPercent,
                screenRectTransform.rect.width);

            if (time > duration)
            {
                endTransitionListeners?.Invoke();
                screen.AfterHide();
                Destroy(gameObject);
            }
        }
    }
}
