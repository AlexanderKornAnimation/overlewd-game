using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{

    public class OverlayShow : BaseScreenTrasition
    {
        private RectTransform screenRectTransform;

        private float duration = 0.3f;
        private float time = 0.0f;

        void Awake()
        {
            screenRectTransform = GetComponent<RectTransform>();
            screenRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right,
                -screenRectTransform.rect.width, screenRectTransform.rect.width);
        }

        void Update()
        {
            time += Time.deltaTime;
            float transitionProgressPercent = time / duration;
            float transitionOffsetPercent = 1.0f - EasingFunction.easeOutExpo(transitionProgressPercent);
            screenRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right,
                -screenRectTransform.rect.width * transitionOffsetPercent,
                screenRectTransform.rect.width);

            if (time > duration)
            {
                UIManager.SetStretch(screenRectTransform);
                Destroy(this);
            }
        }
    }
}
