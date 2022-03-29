using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public override async Task ProgressAsync()
        {
            await UITools.FadeShowAsync(canvasGroup);
            Destroy(this);
        }
    }
}
