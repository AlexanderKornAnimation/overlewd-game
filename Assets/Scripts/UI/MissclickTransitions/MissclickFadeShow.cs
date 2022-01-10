using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class MissclickFadeShow : MissclickShow
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

        void Update()
        {
            time += deltaTimeInc;
            float transitionProgressPercent = time / duration;
            float transitionPercent = EasingFunction.easeOutBack(transitionProgressPercent);

            canvasGroup.alpha = transitionPercent;

            if (time > duration)
            {
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
