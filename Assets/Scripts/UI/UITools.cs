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
        public static string AvailableTimeToString(string availableTime)
        {
            if (String.IsNullOrEmpty(availableTime))
                return null;

            var available = DateTime.Parse(availableTime);
            var now = DateTime.Now;
            return now < available ? TimeToString(available - now) : null;
        }

        public static string TimeToString(TimeSpan time)
        {
            var s = "";
            var sec = time.Seconds;
            var min = time.Minutes;
            var hour = time.Hours;
            var day = time.Days;

            if (day > 0)
            {
                s += day.ToString() + "d ";
                s += hour.ToString("D2") + "h ";
                s += min.ToString("D2") + "m";
            }
            else if (hour > 0)
            {
                s += hour.ToString() + "h ";
                s += min.ToString("D2") + "m";
            }
            else
            {
                s += min.ToString("D2") + "m ";
                s += sec.ToString("D2") + "s";
            }

            return s;
        }

        public static string RewardsToString(List<AdminBRO.RewardItem> rewards)
        {
            string str = "";
            
            foreach (var reward in rewards)
            {
                var item = GameData.markets.GetTradableById(reward.tradableId);
                
                if (item.type == AdminBRO.TradableItem.Type_Currency)
                {
                    str += GameData.currencies.GetById(item.currencyId).sprite + " ";
                }
            }

            return str;
        }
        
        public static void FillWallet(Transform transform)
        {
            var crystal = transform.Find("Crystal").GetComponent<TextMeshProUGUI>();
            var wood = transform.Find("Wood").GetComponent<TextMeshProUGUI>();
            var stone = transform.Find("Stone").GetComponent<TextMeshProUGUI>();
            var copper = transform.Find("Copper").GetComponent<TextMeshProUGUI>();
            var gold = transform.Find("Gold").GetComponent<TextMeshProUGUI>();
            var gems = transform.Find("Gems").GetComponent<TextMeshProUGUI>();
           
            crystal.text = $"<size=40>{GameData.currencies.Crystals.sprite}<size=44> {GameData.player.Crystal.amount}";
            wood.text = $"<size=40>{GameData.currencies.Wood.sprite}<size=44> {GameData.player.Wood.amount}";
            stone.text = $"<size=40>{GameData.currencies.Stone.sprite}<size=44> {GameData.player.Stone.amount}";
            copper.text = $"<size=40>{GameData.currencies.Copper.sprite}<size=44> {GameData.player.Copper.amount}";
            gold.text = $"<size=40>{GameData.currencies.Gold.sprite}<size=44> {GameData.player.Gold.amount}";
            gems.text = $"<size=40>{GameData.currencies.Gems.sprite}<size=44> {GameData.player.Gems.amount}";
        }
        
        public static void DisableButton(Button button, bool disable = true)
        {
            if (disable)
            {
                button.interactable = false;
                foreach (var cr in button.GetComponentsInChildren<CanvasRenderer>())
                {
                    cr.SetColor(Color.gray);
                }
            }
            else
            {
                button.interactable = true;
                foreach (var cr in button.GetComponentsInChildren<CanvasRenderer>())
                {
                    cr.SetColor(Color.white);
                }
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
            var deleteCanvasGroupAfter = false;
            if (canvasGroup == null)
            {
                canvasGroup = obj.AddComponent<CanvasGroup>();
                deleteCanvasGroupAfter = true;
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

            if (deleteCanvasGroupAfter)
            {
                UnityEngine.Object.Destroy(canvasGroup);
            }
        }

        public static async Task FadeHideAsync(GameObject obj, float duration = durationDef)
        {
            var canvasGroup = obj.GetComponent<CanvasGroup>();
            var deleteCanvasGroupAfter = false;
            if (canvasGroup == null)
            {
                canvasGroup = obj.AddComponent<CanvasGroup>();
                deleteCanvasGroupAfter = true;
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

            if (deleteCanvasGroupAfter)
            {
                UnityEngine.Object.Destroy(canvasGroup);
            }
        }

        public static void FadeShow(GameObject obj)
        {
            var canvasGroup = obj.GetComponent<CanvasGroup>();
            var deleteCanvasGroupAfter = false;
            if (canvasGroup == null)
            {
                canvasGroup = obj.AddComponent<CanvasGroup>();
                deleteCanvasGroupAfter = true;
            }

            canvasGroup.alpha = 1.0f;

            if (deleteCanvasGroupAfter)
            {
                UnityEngine.Object.Destroy(canvasGroup);
            }
        }

        public static void FadeHide(GameObject obj)
        {
            var canvasGroup = obj.GetComponent<CanvasGroup>();
            var deleteCanvasGroupAfter = false;
            if (canvasGroup == null)
            {
                canvasGroup = obj.AddComponent<CanvasGroup>();
                deleteCanvasGroupAfter = true;
            }

            canvasGroup.alpha = 0.0f;

            if (deleteCanvasGroupAfter)
            {
                UnityEngine.Object.Destroy(canvasGroup);
            }
        }
    }
}