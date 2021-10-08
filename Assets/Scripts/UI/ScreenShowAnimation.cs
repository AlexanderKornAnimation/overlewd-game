using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class ScreenShowAnimation : MonoBehaviour
    {
        private AnimationCurve curve;
        private RectTransform screenRectTransform;
        private float time = 0.0f;

        void Awake()
        {
            screenRectTransform = GetComponent<RectTransform>();
            screenRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, -2000.0f, 1920.0f);

            curve = new AnimationCurve(new Keyframe(0.0f, -2000.0f), new Keyframe(1.0f, 0.0f));
            curve.preWrapMode = WrapMode.Once;
            curve.postWrapMode = WrapMode.Once;
        }

        void Start()
        {

        }

        void Update()
        {
            time += Time.deltaTime;
            screenRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, curve.Evaluate(time), 1920.0f);

            if (time > 6.1f)
            {
                Destroy(this);
            }
        }
    }

}
