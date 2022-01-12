using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class ScreenImmediateShow : ScreenShow
    {
        protected override void Awake()
        {
            base.Awake();
        }

        async void Start()
        {
            await screen.BeforeShowAsync();
            OnPrepared();
        }

        void Update()
        {
            if (!prepared || locked)
                return;

            OnStart();

            OnEnd();
            screen.AfterShow();
            Destroy(this);
        }
    }
}
