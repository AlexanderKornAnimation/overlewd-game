using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Overlewd
{
    public class BottomShow : BaseShowTransition
    {
        protected override void Awake()
        {
            base.Awake();

            screenRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom,
                -screenRectTransform.rect.height, screenRectTransform.rect.height);
        }

        async void Start()
        {
            await WaitPrepareShowAsync();
        }

        async void Update()
        {
            if (!prepared)
                return;

            time += Time.deltaTime;
            float transitionProgressPercent = time / duration;
            float transitionOffsetPercent = 1.0f - EasingFunction.easeOutExpo(transitionProgressPercent);
            screenRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom,
                -screenRectTransform.rect.height * transitionOffsetPercent,
                screenRectTransform.rect.height);

            if (time > duration)
            {
                await WaitAfterShowAsync();

                UIManager.SetStretch(screenRectTransform);
                Destroy(this);
            }
        }
    }
}
