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

            UITools.SetStretch(screenRectTransform);
            UITools.RightShow(screenRectTransform);
        }

        public override async Task ProgressAsync()
        {
            screen.StartHide();
            await UITools.RightHideAsync(screenRectTransform);
            await screen.AfterHideAsync();
            DestroyImmediate(gameObject);
        }
    }
}
