using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class ScreenHideTransition : MonoBehaviour
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
            slaveRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom,
                -slaveRectTransform.rect.height * curveTransition.Evaluate(transitionProgressPrecent),
                slaveRectTransform.rect.height);

            if (time > duration)
            {
                UIManager.SetStretch(slaveRectTransform);
                Destroy(slaveRectTransform.gameObject);
            }
        }

        public static ScreenHideTransition GetInstance(Transform slave)
        {
            var screenTransition = (GameObject)Instantiate(Resources.Load("Prefabs/UI/AnimationTransitions/HideScreenTransition"), slave);
            screenTransition.transform.SetParent(slave, false);
            return screenTransition.GetComponent<ScreenHideTransition>();
        }
    }

    public class ScreenHide : BaseScreenTrasition
    {
        void Awake()
        {
            var screenRectTransform = GetComponent<RectTransform>();
            screenRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom,
                0.0f, screenRectTransform.rect.height);

            ScreenHideTransition.GetInstance(screenRectTransform);
        }

        void Update()
        {
            Destroy(this);
        }
    }
}
