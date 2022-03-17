using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class ScreenImmediateHide : ScreenHide
    {
        protected override void Awake()
        {
            base.Awake();
        }

        async void Start()
        {
            await screen.BeforeHideAsync();
            OnPrepared();
        }

        async void Update()
        {
            if (!prepared || locked)
                return;

            OnStart();

            await screen.AfterHideAsync();
            OnEnd();
            Destroy(gameObject);
        }

        protected override void OnStartCalls()
        {
            screen.StartHide();
        }
    }
}
