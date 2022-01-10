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
            prepared = true;
            startTransitionListeners?.Invoke();
        }

        void Update()
        {
            if (!prepared || locked)
                return;

            time += deltaTimeInc;
            float transitionProgressPercent = time / duration;
            float transitionOffsetPercent = 1.0f - EasingFunction.easeOutExpo(transitionProgressPercent);
            screenRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right,
                -screenRectTransform.rect.width * transitionOffsetPercent,
                screenRectTransform.rect.width);

            if (time > duration)
            {
                UIManager.SetStretch(screenRectTransform);
                endTransitionListeners?.Invoke();
                screen.AfterShow();
                Destroy(this);
            }
        }
    }
}
