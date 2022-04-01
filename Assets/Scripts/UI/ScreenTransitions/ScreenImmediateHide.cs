using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Overlewd
{
    public class ScreenImmediateHide : ScreenHide
    {
        protected override void Awake()
        {
            base.Awake();
        }

        public override async Task ProgressAsync()
        {
            screen.StartHide();
            await screen.AfterHideAsync();
            Destroy(gameObject);
        }
    }
}
