using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class MissclickColoredHide : MonoBehaviour
    {
        private float duration { get; set; } = 0.3f;

        private Image misslickScreenImage;
        private float time = 0.0f;
        private float alphaMax;

        void Awake()
        {
            misslickScreenImage = GetComponent<Image>();
        }

        void Start()
        {
            alphaMax = misslickScreenImage.color.a;
        }

        void Update()
        {
            time += Time.deltaTime;
            if (time > duration)
            {
                Destroy(gameObject);
            }
            else
            {
                float hidePercent = time / duration;
                misslickScreenImage.color = new Color(0.0f, 0.0f, 0.0f, alphaMax * (1.0f - hidePercent));
            }
        }
    }
}
