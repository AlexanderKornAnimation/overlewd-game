using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class FadeHide : BaseScreenTrasition
    {
        private CanvasGroup canvasGroup;

        protected override void Awake()
        {
            base.Awake();

            canvasGroup = gameObject.AddComponent<CanvasGroup>();
            canvasGroup.alpha = 1.0f;
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
            float transitionPercent = 1.0f - EasingFunction.easeInBack(transitionProgressPercent);

            canvasGroup.alpha = transitionPercent;

            if (time > duration)
            {
                await WaitAfterHideAsync();

                Destroy(gameObject);
            }
        }
    }
}
