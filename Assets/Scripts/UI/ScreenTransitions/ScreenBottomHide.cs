using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Overlewd
{
    public class ScreenBottomHide : ScreenHide
    {
        protected override void Awake()
        {
            base.Awake();

            screenRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom,
                0.0f, screenRectTransform.rect.height);
        }

        public override async Task ProgressAsync()
        {
            screen.StartHide();
            await UITools.BottomHideAsync(screenRectTransform);
            await screen.AfterHideAsync();
            Destroy(gameObject);
        }
    }
}
