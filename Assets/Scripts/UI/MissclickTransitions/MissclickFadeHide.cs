using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Overlewd
{
    public class MissclickFadeHide : MissclickHide
    {
        protected override void Awake()
        {
            base.Awake();
        }

        public override async Task ProgressAsync()
        {
            await UITools.FadeHideAsync(gameObject);
            Destroy(gameObject);
        }
    }
}
