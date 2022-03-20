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
    public static class UIHelper
    {
        private static float deltaTimeInc
        {
            get
            {
                return Time.deltaTime > 1.0f / 60.0f ? 1.0f / 60.0f : Time.deltaTime;
            }
        }

        /*public static async Task ShowBottomAsync(RectTransform uiRect, float duration = 0.2f)
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

                await UniTask.Yield();
            }
        }

        public static async Task HideBottomAsync(RectTransform uiRect, float duration = 0.2f)
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

                await UniTask.Yield();
            }
        }*/

        public static void DisableButton(Button button)
        {
            button.interactable = false;
            foreach (var cr in button.GetComponentsInChildren<CanvasRenderer>())
            {
                cr.SetColor(Color.gray);
            }
        }

        public static IEnumerator ShowBottom(RectTransform uiRect, float duration = 0.2f)
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

                yield return null;
            }
        }

        public static IEnumerator HideBottom(RectTransform uiRect, float duration = 0.2f)
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

                yield return null;
            }
        }
    }

}