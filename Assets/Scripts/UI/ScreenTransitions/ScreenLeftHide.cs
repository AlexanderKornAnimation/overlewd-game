using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Overlewd
{
    public class ScreenLeftHide : ScreenHide
    {
        protected override void Awake()
        {
            base.Awake();

            screenRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left,
                0.0f, screenRectTransform.rect.width);
        }

        public override async Task ProgressAsync()
        {
            screen.StartHide();
            await UITools.LeftHideAsync(screenRectTransform);
            await screen.AfterHideAsync();
            Destroy(gameObject);
        }
    }
}
