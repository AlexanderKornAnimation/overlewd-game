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

            screenRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top,
                0.0f, screenRectTransform.rect.height);
        }

        public override async Task ProgressAsync()
        {
            screen.StartHide();
            await UITools.TopHideAsync(screenRectTransform);
            await screen.AfterHideAsync();
            Destroy(gameObject);
        }
    }
}
