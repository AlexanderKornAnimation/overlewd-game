using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Overlewd
{
    public class ScreenTopHide : ScreenHide
    {
        protected override void Awake()
        {
            base.Awake();

            UITools.SetStretch(screenRectTransform);
            UITools.TopShow(screenRectTransform);
        }

        public override async Task ProgressAsync()
        {
            screen.StartHide();
            await UITools.TopHideAsync(screenRectTransform);
            await screen.AfterHideAsync();
            DestroyImmediate(gameObject);
        }
    }
}
