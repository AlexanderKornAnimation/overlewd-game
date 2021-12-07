using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class ImmediateShow : BaseScreenTrasition
    {
        protected override void Awake()
        {
            base.Awake();
        }

        async void Start()
        {
            await WaitPrepareShowAsync();
        }

        async void Update()
        {
            if (!prepared)
                return;

            await WaitAfterShowAsync();

            Destroy(this);
        }
    }
}
