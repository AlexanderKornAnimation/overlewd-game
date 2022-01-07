using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{

    public class RightShow : BaseShowTransition
    {
        protected override void Awake()
        {
            base.Awake();

            screenRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right,
                -screenRectTransform.rect.width, screenRectTransform.rect.width);
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
            screenRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right,
                -screenRectTransform.rect.width * transitionOffsetPercent,
                screenRectTransform.rect.width);

            if (time > duration)
            {
                await WaitAfterShowAsync();

                UIManager.SetStretch(screenRectTransform);
                Destroy(this);
            }
        }
    }
}