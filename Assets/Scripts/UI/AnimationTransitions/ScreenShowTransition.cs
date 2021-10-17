using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class ScreenShowTransition : MonoBehaviour
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
                Destroy(gameObject);
            }
        }

        public static ScreenShowTransition GetInstance(Transform slave)
        {
            var screenTransition = (GameObject)Instantiate(Resources.Load("Prefabs/UI/AnimationTransitions/ShowScreenTransition"), slave);
            screenTransition.transform.SetParent(slave, false);
            return screenTransition.GetComponent<ScreenShowTransition>();
        }
    }

    public class ScreenShow : BaseScreenTrasition
    {
        void Awake()
        {
            var screenRectTransform = GetComponent<RectTransform>();
            screenRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom,
                -screenRectTransform.rect.height, screenRectTransform.rect.height);

            ScreenShowTransition.GetInstance(screenRectTransform);
        }

        void Update()
        {
            Destroy(this);
        }
    }
}
