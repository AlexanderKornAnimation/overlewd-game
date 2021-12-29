using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class SizePulseSelector : Selector
    {
        private Vector3 startScale;
        private RectTransform rectTransform;

        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            startScale = rectTransform.localScale;
        }

        void Start()
        {
            StartCoroutine(PulsePeriod());
        }

        void OnDestroy()
        {
            StopAllCoroutines();
            rectTransform.localScale = startScale;
        }

        private IEnumerator PulsePeriod()
        {
            while (true)
            {
                yield return new WaitForSeconds(1.3f);
                StartCoroutine(Pulse());
            }
        }

        private IEnumerator Pulse()
        {
            var pulseLen = 0.2f;
            var pulseTime = 0.0f;
            var dt = 0.01f;

            while (pulseTime < pulseLen)
            {
                yield return new WaitForSeconds(dt);
                pulseTime += dt;
                var scale = 1.0f + Mathf.Sin(Mathf.PI * pulseTime / pulseLen) * 0.15f;
                rectTransform.localScale = new Vector3(scale, scale, 1.0f);
            }
        }
    }
}