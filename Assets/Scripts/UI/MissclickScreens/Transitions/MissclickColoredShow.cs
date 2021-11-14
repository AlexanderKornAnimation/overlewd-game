using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class MissclickColoredShow : MonoBehaviour
    {
        public float alphaMax { get; set; } = 0.8f;
        private float duration { get; set; } = 0.3f;

        private Image misslickScreenImage;
        private float time = 0.0f;

        void Awake()
        {
            misslickScreenImage = GetComponent<Image>();
        }

        void Start()
        {
            misslickScreenImage.color = Color.clear;
        }

        void Update()
        {
            time += Time.deltaTime;
            if (time > duration)
            {
                Destroy(this);
            }
            else
            {
                float showPercent = time / duration;
                misslickScreenImage.color = new Color(0.0f, 0.0f, 0.0f, alphaMax * showPercent);
            }
        }
    }
}
