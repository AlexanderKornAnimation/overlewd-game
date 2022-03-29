using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Overlewd
{
    public class ScreenBottomShow : ScreenShow
    {
        protected override void Awake()
        {
            base.Awake();

            screenRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom,
                -screenRectTransform.rect.height, screenRectTransform.rect.height);
        }

        public override async Task PrepareDataAsync()
        {
            await screen.BeforeShowDataAsync();
        }

        public override async Task PrepareAsync()
        {
            await screen.BeforeShowAsync();
        }

        public override async Task ProgressAsync()
        {
            screen.StartShow();
            await UITools.BottomShowAsync(screenRectTransform);
            UITools.SetStretch(screenRectTransform);
            await screen.AfterShowAsync();
            Destroy(this);
        }
    }
}
