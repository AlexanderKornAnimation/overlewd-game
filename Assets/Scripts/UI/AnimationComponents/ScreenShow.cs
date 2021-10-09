using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class ScreenShow : UIAnimationBase
    {
        private AnimationCurve curve;
        private RectTransform screenRectTransform;
        private float time = 0.0f;
        public float duration { get; set; } = 0.3f;

        void Awake()
        {
            screenRectTransform = GetComponent<RectTransform>();
            screenRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom,
                -screenRectTransform.rect.height,
                screenRectTransform.rect.height);

            curve = new AnimationCurve(new Keyframe(0.0f, -screenRectTransform.rect.height), new Keyframe(duration, 0.0f));
            curve.preWrapMode = WrapMode.Once;
            curve.postWrapMode = WrapMode.Once;
        }

        void Start()
        {

        }

        void Update()
        {
            time += Time.deltaTime;
            screenRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, curve.Evaluate(time), screenRectTransform.rect.height);

            if (time > duration)
            {
                UIManager.SetStretch(screenRectTransform);
                Destroy(this);
            }
        }
    }

}
