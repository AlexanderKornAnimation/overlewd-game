using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class OverlayHideTransition : MonoBehaviour
    {
        public AnimationCurve curveTransition;
        public float duration;

        private float time;
        private RectTransform slaveRectTransform;

        void Start()
        {
            slaveRectTransform = GetComponentInParent<RectTransform>();
        }

        void Update()
        {
            time += Time.deltaTime;
            float transitionProgressPrecent = time / duration;
            slaveRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right,
                -slaveRectTransform.rect.width * curveTransition.Evaluate(transitionProgressPrecent),
                slaveRectTransform.rect.width);

            if (time > duration)
            {
                UIManager.SetStretch(slaveRectTransform);
                Destroy(slaveRectTransform.gameObject);
            }
        }

        public static OverlayHideTransition GetInstance(Transform slave)
        {
            var screenTransition = (GameObject)Instantiate(Resources.Load("Prefabs/UI/AnimationTransitions/HideOverlayTransition"), slave);
            screenTransition.transform.SetParent(slave, false);
            return screenTransition.GetComponent<OverlayHideTransition>();
        }
    }

    public class OverlayHide : BaseScreenTrasition
    {
        void Awake()
        {
            var screenRectTransform = GetComponent<RectTransform>();
            screenRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right,
                0.0f, screenRectTransform.rect.width);

            OverlayHideTransition.GetInstance(screenRectTransform);
        }

        void Update()
        {
            Destroy(this);
        }
    }
}
