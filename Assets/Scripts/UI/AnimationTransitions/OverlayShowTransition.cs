using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class OverlayShowTransition : MonoBehaviour
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
                Destroy(gameObject);
            }
        }

        public static OverlayShowTransition GetInstance(Transform slave)
        {
            var screenTransition = (GameObject)Instantiate(Resources.Load("Prefabs/UI/AnimationTransitions/ShowOverlayTransition"), slave);
            screenTransition.transform.SetParent(slave, false);
            return screenTransition.GetComponent<OverlayShowTransition>();
        }
    }

    public class OverlayShow : BaseScreenTrasition
    {
        void Awake()
        {
            var screenRectTransform = GetComponent<RectTransform>();
            screenRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right,
                -screenRectTransform.rect.width, screenRectTransform.rect.width);

            OverlayShowTransition.GetInstance(screenRectTransform);
        }

        void Update()
        {
            Destroy(this);
        }
    }
}
