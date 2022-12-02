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

            UITools.SetStretch(screenRectTransform);
            UITools.LeftShow(screenRectTransform);
        }

        public override async Task ProgressAsync()
        {
            screen.StartHide();
            await UITools.LeftHideAsync(screenRectTransform);
            await screen.AfterHideAsync();
            DestroyImmediate(gameObject);
        }
    }
}
