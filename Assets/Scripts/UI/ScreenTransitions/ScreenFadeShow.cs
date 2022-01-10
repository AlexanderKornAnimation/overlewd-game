using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class ScreenFadeShow : ScreenShow
    {
        private CanvasGroup canvasGroup;
        private bool localCanvasGroup = false;

        protected override void Awake()
        {
            base.Awake();

            canvasGroup = gameObject.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
                localCanvasGroup = true;
            }
            canvasGroup.alpha = 0.0f;
        }

        async void Start()
        {
            await screen.BeforeShowAsync();
            prepared = true;
            startTransitionListeners?.Invoke();
        }

        void Update()
        {
            if (!prepared || locked)
                return;

            time += deltaTimeInc;
            float transitionProgressPercent = time / duration;
            float transitionPercent = EasingFunction.easeOutBack(transitionProgressPercent);

            canvasGroup.alpha = transitionPercent;

            if (time > duration)
            {
                canvasGroup.alpha = 1.0f;
                endTransitionListeners?.Invoke();
                screen.AfterShow();
                Destroy(this);
            }
        }

        void OnDestroy()
        {
            if (localCanvasGroup)
            {
                Destroy(canvasGroup);
            }
        }
    }
}
