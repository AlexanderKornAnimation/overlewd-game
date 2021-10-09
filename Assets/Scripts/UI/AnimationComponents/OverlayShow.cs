using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class OverlayShow : UIAnimationBase
    {
        private AnimationCurve curve;
        private RectTransform screenRectTransform;
        private float time = 0.0f;
        public float duration { get; set; } = 0.3f;

        void Awake()
        {
            screenRectTransform = GetComponent<RectTransform>();
            screenRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right,
                -screenRectTransform.rect.width,
                screenRectTransform.rect.width);

            curve = new AnimationCurve(new Keyframe(0.0f, -screenRectTransform.rect.width), new Keyframe(duration, 0.0f));
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
                UIManager.SetStretch(screenRectTransform);
                Destroy(this);
            }
        }
    }

}
