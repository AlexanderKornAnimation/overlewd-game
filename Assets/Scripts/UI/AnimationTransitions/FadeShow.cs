using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class FadeShow : BaseScreenTrasition
    {
        private CanvasGroup canvasGroup;

        protected override void Awake()
        {
            base.Awake();

            canvasGroup = gameObject.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
            canvasGroup.alpha = 0.0f;
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
            float transitionPercent = EasingFunction.easeOutBack(transitionProgressPercent);

            canvasGroup.alpha = transitionPercent;

            if (time > duration)
            {
                await WaitAfterShowAsync();

                canvasGroup.alpha = 1.0f;
                Destroy(canvasGroup);
                Destroy(this);
            }
        }
    }
}
