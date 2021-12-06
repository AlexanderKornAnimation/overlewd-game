using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class ScreenShow : BaseScreenTrasition
    {
        private RectTransform screenRectTransform;

        private float duration = 0.5f;
        private float time = 0.0f;

        void Awake()
        {
            screenRectTransform = GetComponent<RectTransform>();
            screenRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom,
                -screenRectTransform.rect.height, screenRectTransform.rect.height);
        }

        void Update()
        {
            time += Time.deltaTime;
            float transitionProgressPercent = time / duration;
            float transitionOffsetPercent = 1.0f - EasingFunction.easeOutBack(transitionProgressPercent);
            screenRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom,
                -screenRectTransform.rect.height * transitionOffsetPercent,
                screenRectTransform.rect.height);

            if (time > duration)
            {
                UIManager.SetStretch(screenRectTransform);
                Destroy(this);
            }
        }
    }
}
