using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Overlewd
{
    public class UpInfoFX : BaseWidget
    {
        private TextMeshProUGUI text;

        void Awake()
        {
            text = transform.Find("Canvas").Find("Text").GetComponent<TextMeshProUGUI>();
        }

        public void Run(string title,
            Color color, float tSize,
            float xOffset = 100.0f,
            float yOffset = 0.0f,
            float duration = 1.5f)
        {
            text.color = color;
            text.fontSize = tSize;
            text.text = title;

            var seq = DOTween.Sequence();
            seq.Join(transform.DOMoveY(transform.position.x + xOffset, duration));
            seq.Join(transform.DOMoveY(transform.position.y + yOffset, duration));
            seq.Join(text.DOFade(0.0f, duration));
            seq.onComplete = () => Destroy(gameObject);
            seq.Play();
        }

        public void RunX(string title,
            Color color, float tSize,
            float xOffset = 100.0f,
            float duration = 1.5f)
        {
            Run(title, color, tSize, xOffset, 0.0f, duration);
        }

        public void RunY(string title,
            Color color, float tSize,
            float yOffset = 100.0f,
            float duration = 1.5f)
        {
            Run(title, color, tSize, 0.0f, yOffset, duration);
        }

        public static UpInfoFX GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateScreenPrefab<UpInfoFX>
                ("Prefabs/UI/Widgets/InfoFX/UpInfoFX", parent);
        }
    }
}

