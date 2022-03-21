using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class ScreenFadeHide : ScreenHide
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
            canvasGroup.alpha = 1.0f;
        }

        async void Start()
        {
            await screen.BeforeHideAsync();
            OnPrepared();

            await WaitUnlocked();
            OnStart();

            await UIHelper.FadeHideAsync(canvasGroup);

            await screen.AfterHideAsync();
            OnEnd();

            Destroy(gameObject);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (localCanvasGroup)
            {
                Destroy(canvasGroup);
            }
        }

        protected override void OnStart()
        {
            base.OnStart();
            screen.StartHide();
        }
    }
}
