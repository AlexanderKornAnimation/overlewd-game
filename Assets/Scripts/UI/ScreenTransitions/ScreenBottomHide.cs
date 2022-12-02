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

            UITools.SetStretch(screenRectTransform);
            UITools.BottomShow(screenRectTransform);
        }

        public override async Task ProgressAsync()
        {
            screen.StartHide();
            await UITools.BottomHideAsync(screenRectTransform);
            await screen.AfterHideAsync();
            DestroyImmediate(gameObject);
        }
    }
}
