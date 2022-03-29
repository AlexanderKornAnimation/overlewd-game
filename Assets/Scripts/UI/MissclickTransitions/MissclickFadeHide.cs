using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class MissclickFadeHide : MissclickHide
    {
        protected override void Awake()
        {
            base.Awake();
            canvasGroup.alpha = 1.0f;
        }

        async void Start()
        {
            await UITools.FadeHideAsync(canvasGroup);
            Destroy(gameObject);
        }
    }
}
