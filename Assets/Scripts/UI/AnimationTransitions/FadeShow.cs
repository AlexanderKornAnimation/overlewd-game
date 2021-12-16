using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class FadeShow : BaseShowTrasition
    {
        private CanvasGroup canvasGroup;
        private bool localCanvasGroup = false;

        protected override void Awake()
        {
            base.Awake();

            canvasGroup = gameObject.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
                localCanvasGroup = true;
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
                Destroy(this);
            }
        }

        void OnDestroy()
        {
            if (localCanvasGroup)
            {
                Destroy(canvasGroup);
            }
        }
    }
}
