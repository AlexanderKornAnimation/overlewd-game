using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class FadeShow : BaseScreenTrasition
    {
        private CanvasGroup canvasGroup;

        private float duration = 0.3f;
        private float time = 0.0f;

        void Awake()
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
            canvasGroup.alpha = 0.0f;
        }

        void Update()
        {
            time += Time.deltaTime;
            float transitionProgressPercent = time / duration;
            float transitionPercent = EasingFunction.easeOutBack(transitionProgressPercent);

            canvasGroup.alpha = transitionPercent;

            if (time > duration)
            {
                canvasGroup.alpha = 1.0f;
                Destroy(canvasGroup);
                Destroy(this);
            }
        }
    }
}
