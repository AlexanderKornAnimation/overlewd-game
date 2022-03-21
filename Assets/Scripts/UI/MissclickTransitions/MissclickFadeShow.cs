using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class MissclickFadeShow : MissclickShow
    {
        protected override void Awake()
        {
            base.Awake();
            canvasGroup.alpha = 0.0f;
        }

        async void Start()
        {
            await UIHelper.FadeShowAsync(canvasGroup);
            Destroy(this);
        }
    }
}
