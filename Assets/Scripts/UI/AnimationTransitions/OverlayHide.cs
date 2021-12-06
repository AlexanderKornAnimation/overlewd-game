using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class OverlayHide : BaseScreenTrasition
    {
        private RectTransform screenRectTransform;

        private float duration = 0.3f;
        private float time = 0.0f;

        void Awake()
        {
            screenRectTransform = GetComponent<RectTransform>();
            screenRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right,
                0.0f, screenRectTransform.rect.width);
        }

        void Update()
        {
            time += Time.deltaTime;
            float transitionProgressPercent = time / duration;
            float transitionOffsetPercent = EasingFunction.easeInExpo(transitionProgressPercent);
            screenRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right,
                -screenRectTransform.rect.width * transitionOffsetPercent,
                screenRectTransform.rect.width);

            if (time > duration)
            {
                Destroy(gameObject);
            }
        }
    }
}
