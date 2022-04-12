using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Overlewd
{

    public class ScreenLeftShow : ScreenShow
    {
        protected override void Awake()
        {
            base.Awake();

            screenRectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left,
                -screenRectTransform.rect.width, screenRectTransform.rect.width);
        }

        public override async Task ProgressAsync()
        {
            screen.StartShow();
            await UITools.LeftShowAsync(screenRectTransform);
            UITools.SetStretch(screenRectTransform);
            await screen.AfterShowAsync();
            Destroy(this);
        }
    }
}
