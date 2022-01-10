using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class MissclickFadeHide : MissclickHide
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
            canvasGroup.alpha = 1.0f;
        }

        void Update()
        {
            time += deltaTimeInc;
            float transitionProgressPercent = time / duration;
            float transitionPercent = 1.0f - EasingFunction.easeInBack(transitionProgressPercent);

            canvasGroup.alpha = transitionPercent;

            if (time > duration)
            {
                Destroy(gameObject);
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
