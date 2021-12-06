using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class ScreenHide : BaseScreenTrasition
    {
        private RectTransform screenRectTransform;

        private float duration = 0.3f;
        private float time = 0.0f;

        void Awake()
        {
            screenRectTransform = GetComponent<RectTransform>();
            screenRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom,
                0.0f, screenRectTransform.rect.height);
        }

        void Update()
        {
            time += Time.deltaTime;
            float transitionProgressPercent = time / duration;
            float transitionOffsetPercent = EasingFunction.easeInExpo(transitionProgressPercent);
            screenRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom,
                -screenRectTransform.rect.height * transitionOffsetPercent,
                screenRectTransform.rect.height);

            if (time > duration)
            {
                Destroy(gameObject);
            }
        }
    }
}
