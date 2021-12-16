using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class ImmediateHide : BaseHideTrasition
    {
        protected override void Awake()
        {
            base.Awake();
        }

        async void Start()
        {
            await WaitPrepareHideAsync();
        }

        async void Update()
        {
            if (!prepared)
                return;

            await WaitAfterHideAsync();

            Destroy(gameObject);
        }
    }
}
