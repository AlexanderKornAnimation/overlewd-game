using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class OverlayHide : BaseScreenTrasition
    {
        protected override void Awake()
        {
            base.Awake();

            screenRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right,
                0.0f, screenRectTransform.rect.width);
        }

        async void Start()
        {
            await WaitPrepareHideAsync();
        }

        async void Update()
        {
            if (!prepared)
                return;

            time += Time.deltaTime;
            float transitionProgressPercent = time / duration;
            float transitionOffsetPercent = EasingFunction.easeInOutQuad(transitionProgressPercent);
            screenRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right,
                -screenRectTransform.rect.width * transitionOffsetPercent,
                screenRectTransform.rect.width);

            if (time > duration)
            {
                await WaitAfterHideAsync();

                Destroy(gameObject);
            }
        }
    }
}
