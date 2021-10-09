using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class ScreenHide : UIAnimationBase
    {
        private AnimationCurve curve;
        private RectTransform screenRectTransform;
        private float time = 0.0f;
        public float duration { get; set; } = 0.3f;
        public bool hideToLeft { get; set; } = true;

        void Awake()
        {
            screenRectTransform = GetComponent<RectTransform>();

            if (hideToLeft)
            {
                curve = new AnimationCurve(new Keyframe(0.0f, 0.0f), new Keyframe(duration, -screenRectTransform.rect.width));
            }
            else
            {
                curve = new AnimationCurve(new Keyframe(0.0f, 0.0f), new Keyframe(duration, -screenRectTransform.rect.height));
            }
            
            curve.preWrapMode = WrapMode.Once;
            curve.postWrapMode = WrapMode.Once;
        }

        void Start()
        {

        }

        void Update()
        {
            time += Time.deltaTime;

            if (hideToLeft)
            {
                screenRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, curve.Evaluate(time), screenRectTransform.rect.width);
            }
            else
            {
                screenRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, curve.Evaluate(time), screenRectTransform.rect.height);
            }

            if (time > duration)
            {
                Destroy(gameObject);
            }
        }
    }

}
