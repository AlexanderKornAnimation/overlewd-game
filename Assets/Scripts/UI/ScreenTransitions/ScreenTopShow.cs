using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Overlewd
{
    public class ScreenTopShow : ScreenShow
    {
        protected override void Awake()
        {
            base.Awake();

            UITools.SetStretch(screenRectTransform);
            UITools.TopHide(screenRectTransform);
        }

        public override async Task ProgressAsync()
        {
            screen.StartShow();
            await UITools.TopShowAsync(screenRectTransform);
            UITools.SetStretch(screenRectTransform);
            await screen.AfterShowAsync();
            DestroyImmediate(this);
        }
    }
}
