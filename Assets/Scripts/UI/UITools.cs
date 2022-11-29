using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TMPro;

namespace Overlewd
{
    public static class UITools
    {
        public static async void ClaimRewardsAsync(List<AdminBRO.RewardItem> rewards)
        {
            if (rewards == null)
                return;
            foreach (var r in rewards)
            {
                var notif = PopupNotifWidget.GetInstance(UIManager.systemNotifRoot);
                notif.Play("Claim", $"{r.amount} {r.tmpSprite}");
                await UniTask.Delay(1000);
            }
        }

        public static string ChangeTextSize(string text, float fontSize)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }
            
            var result = "";

            foreach (var ch in text)
            {
                result += char.IsNumber(ch) || ch == '%' || ch == '+' ? $"<size={fontSize + 8}>{ch}</size>" : ch.ToString();
            }

            return result;
        }
        
        public static List<AdminBRO.PriceItem> PriceMul(List<AdminBRO.PriceItem> price, int mul)
        {
            var result = new List<AdminBRO.PriceItem>();
            foreach (var p in price)
            {
                result.Add(p * mul);
            }
            return result;
        }

        public static string RewardsToString(List<AdminBRO.RewardItem> rewards)
        {
            string result = "";
            foreach (var reward in rewards)
            {
                var tData = reward.tradableData;
                if (tData?.type == AdminBRO.TradableItem.Type_Currency)
                {
                    var sprite = tData?.tmpCurrencySprite;
                    result += String.IsNullOrEmpty(sprite) ?
                        "" :
                        String.IsNullOrEmpty(result) ? sprite : (" " + sprite);
                }
            }
            return result;
        }

        public static string PriceToString(List<AdminBRO.PriceItem> price)
        {
            string result = "";
            foreach (var p in price)
            {
                var sprite = p.tmpSprite;
                result += String.IsNullOrEmpty(sprite) ?
                        "" :
                        String.IsNullOrEmpty(result) ? sprite + $" {p.amount}" : (" " + sprite + $" {p.amount}");
            }
            return result;
        }
        
        public static void DisableButton(Button button, bool disable = true)
        {
            var bColors = button.colors;
            button.interactable = !disable;
            bColors.disabledColor = disable ? Color.gray : Color.white;
            button.colors = bColors;
            foreach (var cr in button.GetComponentsInChildren<CanvasRenderer>(true))
            {
                cr.SetColor(disable ? Color.gray : Color.white);
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

        public static void LeftShow(RectTransform uiRect)
        {
            uiRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left,
                0.0f, uiRect.rect.width);
        }

        public static void LeftHide(RectTransform uiRect)
        {
            uiRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left,
                -uiRect.rect.width, uiRect.rect.width);
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

        public static void RightShow(RectTransform uiRect)
        {
            uiRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right,
                0.0f, uiRect.rect.width);
        }

        public static void RightHide(RectTransform uiRect)
        {
            uiRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right,
                -uiRect.rect.width, uiRect.rect.width);
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

        public static void BottomShow(RectTransform uiRect)
        {
            uiRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom,
                0.0f, uiRect.rect.height);
        }

        public static void BottomHide(RectTransform uiRect)
        {
            uiRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom,
                -uiRect.rect.height, uiRect.rect.height);
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

        public static void TopShow(RectTransform uiRect)
        {
            uiRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top,
                0.0f, uiRect.rect.height);
        }

        public static void TopHide(RectTransform uiRect)
        {
            uiRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top,
                -uiRect.rect.height, uiRect.rect.height);
        }

        //fade
        public static async Task FadeShowAsync(GameObject obj, float duration = durationDef)
        {
            var canvasGroup = obj.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = obj.AddComponent<CanvasGroup>();
            }

            float time = 0.0f;
            while (time < duration)
            {
                time += deltaTimeInc;
                float transitionProgressPercent = time / duration;
                float transitionPercent = EasingFunction.easeOutBack(transitionProgressPercent);

                canvasGroup.alpha = transitionPercent;

                await UniTask.NextFrame();
            }
        }

        public static async Task FadeHideAsync(GameObject obj, float duration = durationDef)
        {
            var canvasGroup = obj.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = obj.AddComponent<CanvasGroup>();
            }

            float time = 0.0f;
            while (time < duration)
            {
                time += deltaTimeInc;
                float transitionProgressPercent = time / duration;
                float transitionPercent = 1.0f - EasingFunction.easeInBack(transitionProgressPercent);

                canvasGroup.alpha = transitionPercent;

                await UniTask.NextFrame();
            }
        }

        public static void FadeShow(GameObject obj)
        {
            var canvasGroup = obj.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = obj.AddComponent<CanvasGroup>();
            }
            canvasGroup.alpha = 1.0f;
        }

        public static void FadeHide(GameObject obj)
        {
            var canvasGroup = obj.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = obj.AddComponent<CanvasGroup>();
            }
            canvasGroup.alpha = 0.0f;
        }
    }
}