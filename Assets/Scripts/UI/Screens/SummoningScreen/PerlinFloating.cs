using UnityEngine;

namespace Overlewd
{
    namespace NSSummoningScreen
    {
        public class PerlinFloating : MonoBehaviour
        {

            [Tooltip("position on the Perlin's wave")]
            public float perlinPos = 0f;
            [Tooltip("+- strength around Y axis in pixels")]
            public float perlinLenght = 30f;
            public float perlinSpeed = 1f;
            private float perlinNoise = 0f;

            private float tfY = 0f;

            RectTransform rt;

            private void Awake()
            {
                rt = GetComponent<RectTransform>();
            }
            private void Start()
            {
                tfY = rt.localPosition.y - perlinLenght / 2;
            }

            void Update()
            {
                float t = Time.time * perlinSpeed;
                perlinNoise = Mathf.PerlinNoise(t, perlinPos) * perlinLenght;
                rt.anchoredPosition = new Vector2(0, tfY + perlinNoise);
            }
        }
    }
}