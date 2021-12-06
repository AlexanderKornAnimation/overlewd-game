using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public static class EasingFunction
    {
        public static float linear(float x)
        {
            return Mathf.Clamp01(x);
        }

        public static float easeInExpo(float x)
        {
            float cx = Mathf.Clamp01(x);
            return cx == 0.0f ? 0.0f : Mathf.Pow(2.0f, 10.0f * cx - 10.0f);
        }

        public static float easeOutExpo(float x)
        {
            float cx = Mathf.Clamp01(x);
            return 1.0f - Mathf.Pow(2.0f, -10.0f * cx);
        }

        public static float easeInBack(float x)
        {
            float cx = Mathf.Clamp01(x);
            const float c1 = 1.70158f;
            const float c3 = c1 + 1.0f;
            return c3 * cx * cx * cx - c1 * cx * cx;
        }

        public static float easeOutBack(float x)
        {
            float cx = Mathf.Clamp01(x);
            const float c1 = 1.70158f;
            const float c3 = c1 + 1.0f;
            return 1.0f + c3 * Mathf.Pow(cx - 1.0f, 3.0f) + c1 * Mathf.Pow(cx - 1.0f, 2.0f);
        }

        public static float easeInOutBack(float x)
        {
            float cx = Mathf.Clamp01(x);
            const float c1 = 1.70158f;
            const float c2 = c1 * 1.525f;

            return cx < 0.5f ? (Mathf.Pow(2.0f * cx, 2.0f) * ((c2 + 1.0f) * 2.0f * cx - c2)) / 2.0f : 
                (Mathf.Pow(2.0f * cx - 2.0f, 2.0f) * ((c2 + 1.0f) * (cx * 2.0f - 2.0f) + c2) + 2.0f) / 2.0f;
        }

        public static float easeOutElastic(float x)
        {
            float cx = Mathf.Clamp01(x);
            const float c4 = (2.0f * Mathf.PI) / 3.0f;
            return cx == 0.0f ? 0.0f : cx == 1.0f ? 1.0f : Mathf.Pow(2.0f, -10.0f * cx) * Mathf.Sin((cx * 10.0f - 0.75f) * c4) + 1.0f;
        }
    }
}
