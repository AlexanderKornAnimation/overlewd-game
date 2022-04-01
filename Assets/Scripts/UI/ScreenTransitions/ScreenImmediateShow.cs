using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Overlewd
{
    public class ScreenImmediateShow : ScreenShow
    {
        protected override void Awake()
        {
            base.Awake();
        }

        public override async Task ProgressAsync()
        {
            screen.StartShow();
            await screen.AfterShowAsync();
            Destroy(this);
        }
    }
}
