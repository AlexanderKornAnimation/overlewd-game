using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class FadeHide : BaseScreenTrasition
    {
        private CanvasGroup canvasGroup;

        private float duration = 0.3f;
        private float time = 0.0f;

        void Awake()
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
            canvasGroup.alpha = 1.0f;
        }

        void Update()
        {
            time += Time.deltaTime;
            float transitionProgressPercent = time / duration;
            float transitionPercent = 1.0f - EasingFunction.easeInBack(transitionProgressPercent);

            canvasGroup.alpha = transitionPercent;

            if (time > duration)
            {
                Destroy(gameObject);
            }
        }
    }
}
