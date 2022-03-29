using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace Overlewd
{
    public static class UITools
    {
        public static void DisableButton(Button button)
        {
            button.interactable = false;
            foreach (var cr in button.GetComponentsInChildren<CanvasRenderer>())
            {
                cr.SetColor(Color.gray);
            }
        }

        public static void SetStretch(RectTransform rectTransform)
        {
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
        }

        //anim
        private static float deltaTimeInc
        {
            get
            {
                return Time.deltaTime > 1.0f / 60.0f ? 1.0f / 60.0f : Time.deltaTime;
            }
        }

        private const float durationDef = 0.3f;

        //left
        public static async Task LeftShowAsync(RectTransform uiRect, float duration = durationDef)
        {
            float time = 0.0f;

            while (time < duration)
            {
                time += deltaTimeInc;
                float transitionProgressPercent = time / duration;
                float transitionOffsetPercent = 1.0f - EasingFunction.easeOutExpo(transitionProgressPercent);

                uiRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left,
                    -uiRect.rect.width * transitionOffsetPercent,
                    uiRect.rect.width);

                await UniTask.NextFrame();
            }
        }

        public static async Task LeftHideAsync(RectTransform uiRect, float duration = durationDef)
        {
            float time = 0.0f;

            while (time < duration)
            {
                time += deltaTimeInc;
                float transitionProgressPercent = time / duration;
                float transitionOffsetPercent = EasingFunction.easeInOutQuad(transitionProgressPercent);

                uiRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left,
                    -uiRect.rect.width * transitionOffsetPercent,
                    uiRect.rect.width);

                await UniTask.NextFrame();
            }
        }

        //right
        public static async Task RightShowAsync(RectTransform uiRect, float duration = durationDef)
        {
            float time = 0.0f;

            while (time < duration)
            {
                time += deltaTimeInc;
                float transitionProgressPercent = time / duration;
                float transitionOffsetPercent = 1.0f - EasingFunction.easeOutExpo(transitionProgressPercent);

                uiRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right,
                    -uiRect.rect.width * transitionOffsetPercent,
                    uiRect.rect.width);

                await UniTask.NextFrame();
            }
        }

        public static async Task RightHideAsync(RectTransform uiRect, float duration = durationDef)
        {
            float time = 0.0f;

            while (time < duration)
            {
                time += deltaTimeInc;
                float transitionProgressPercent = time / duration;
                float transitionOffsetPercent = EasingFunction.easeInOutQuad(transitionProgressPercent);

                uiRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right,
                    -uiRect.rect.width * transitionOffsetPercent,
                    uiRect.rect.width);

                await UniTask.NextFrame();
            }
        }

        //bottom
        public static async Task BottomShowAsync(RectTransform uiRect, float duration = durationDef)
        {
            float time = 0.0f;

            while (time < duration)
            {
                time += deltaTimeInc;
                float transitionProgressPercent = time / duration;
                float transitionOffsetPercent = 1.0f - EasingFunction.easeOutExpo(transitionProgressPercent);

                uiRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom,
                    -uiRect.rect.height * transitionOffsetPercent,
                    uiRect.rect.height);

                await UniTask.NextFrame();
            }
        }

        public static async Task BottomHideAsync(RectTransform uiRect, float duration = durationDef)
        {
            float time = 0.0f;

            while (time < duration)
            {
                time += deltaTimeInc;
                float transitionProgressPercent = time / duration;
                float transitionOffsetPercent = EasingFunction.easeInOutQuad(transitionProgressPercent);

                uiRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom,
                    -uiRect.rect.height * transitionOffsetPercent,
                    uiRect.rect.height);

                await UniTask.NextFrame();
            }
        }

        //top
        public static async Task TopShowAsync(RectTransform uiRect, float duration = durationDef)
        {
            float time = 0.0f;

            while (time < duration)
            {
                time += deltaTimeInc;
                float transitionProgressPercent = time / duration;
                float transitionOffsetPercent = 1.0f - EasingFunction.easeOutExpo(transitionProgressPercent);

                uiRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top,
                    -uiRect.rect.height * transitionOffsetPercent,
                    uiRect.rect.height);

                await UniTask.NextFrame();
            }
        }

        public static async Task TopHideAsync(RectTransform uiRect, float duration = durationDef)
        {
            float time = 0.0f;

            while (time < duration)
            {
                time += deltaTimeInc;
                float transitionProgressPercent = time / duration;
                float transitionOffsetPercent = EasingFunction.easeInOutQuad(transitionProgressPercent);

                uiRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top,
                    -uiRect.rect.height * transitionOffsetPercent,
                    uiRect.rect.height);

                await UniTask.NextFrame();
            }
        }

        //fade
        public static async Task FadeShowAsync(CanvasGroup uiCanvas, float duration = durationDef)
        {
            float time = 0.0f;

            while (time < duration)
            {
                time += deltaTimeInc;
                float transitionProgressPercent = time / duration;
                float transitionPercent = EasingFunction.easeOutBack(transitionProgressPercent);

                uiCanvas.alpha = transitionPercent;

                await UniTask.NextFrame();
            }
        }

        public static async Task FadeHideAsync(CanvasGroup uiCanvas, float duration = durationDef)
        {
            float time = 0.0f;

            while (time < duration)
            {
                time += deltaTimeInc;
                float transitionProgressPercent = time / duration;
                float transitionPercent = 1.0f - EasingFunction.easeInBack(transitionProgressPercent);

                uiCanvas.alpha = transitionPercent;

                await UniTask.NextFrame();
            }
        }
    }

}