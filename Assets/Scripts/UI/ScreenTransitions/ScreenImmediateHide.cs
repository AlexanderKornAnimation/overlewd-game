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

        void Update()
        {
            if (!prepared || locked)
                return;

            OnStart();

            OnEnd();
            screen.AfterHide();
            Destroy(gameObject);
        }

        protected override void OnStartCalls()
        {
            screen.StartHide();
        }
    }
}
