using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Overlewd
{
    public class ScreenFadeHide : ScreenHide
    {
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        public override async Task ProgressAsync()
        {
            screen.StartHide();
            await UITools.FadeHideAsync(gameObject);
            await screen.AfterHideAsync();
            Destroy(gameObject);
        }
    }
}
