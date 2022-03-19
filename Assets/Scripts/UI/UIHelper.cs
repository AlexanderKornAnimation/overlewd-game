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
        public static void DisableButton(Button button)
        {
            button.interactable = false;
            foreach (var cr in button.GetComponentsInChildren<CanvasRenderer>())
            {
                cr.SetColor(Color.gray);
            }
        }

        public static async Task ShowBottomAsync(RectTransform uiRect, float duration = 0.2f)
        {
            float time = 0.0f;

            while (time < duration)
            {
                float deltaTimeInc = Time.deltaTime > 1.0f / 60.0f ? 1.0f / 60.0f : Time.deltaTime;
                float transitionProgressPercent = time / duration;
                float transitionOffsetPercent = 1.0f - EasingFunction.easeOutExpo(transitionProgressPercent);

                time += deltaTimeInc;
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
                float deltaTimeInc = Time.deltaTime > 1.0f / 60.0f ? 1.0f / 60.0f : Time.deltaTime;
                float transitionProgressPercent = time / duration;
                float transitionOffsetPercent = EasingFunction.easeInOutQuad(transitionProgressPercent);

                time += deltaTimeInc;
                uiRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom,
                    -uiRect.rect.height * transitionOffsetPercent,
                    uiRect.rect.height);

                await UniTask.Yield();
            }
        }
    }

}