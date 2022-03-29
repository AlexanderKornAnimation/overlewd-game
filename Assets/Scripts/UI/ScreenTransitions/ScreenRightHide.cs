using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Overlewd
{
    public class ScreenRightHide : ScreenHide
    {
        protected override void Awake()
        {
            base.Awake();

            screenRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right,
                0.0f, screenRectTransform.rect.width);
        }

        public override async Task PrepareDataAsync()
        {
            await screen.BeforeHideDataAsync();
        }

        public override async Task PrepareAsync()
        {
            await screen.BeforeHideAsync();
        }

        public override async Task ProgressAsync()
        {
            screen.StartHide();
            await UITools.RightHideAsync(screenRectTransform);
            await screen.AfterHideAsync();
            Destroy(gameObject);
        }
    }
}
