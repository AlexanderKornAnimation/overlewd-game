using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class OverlayHide : UIAnimationBase
    {
        private AnimationCurve curve;
        private RectTransform screenRectTransform;
        private float time = 0.0f;
        public float duration { get; set; } = 0.3f;

        void Awake()
        {
            screenRectTransform = GetComponent<RectTransform>();
            
            curve = new AnimationCurve(new Keyframe(0.0f, 0.0f), new Keyframe(duration, -screenRectTransform.rect.width));
            curve.preWrapMode = WrapMode.Once;
            curve.postWrapMode = WrapMode.Once;
        }

        void Start()
        {

        }

        void Update()
        {
            time += Time.deltaTime;
            screenRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, curve.Evaluate(time), screenRectTransform.rect.width);

            if (time > duration)
            {
                Destroy(gameObject);
            }
        }
    }

}
