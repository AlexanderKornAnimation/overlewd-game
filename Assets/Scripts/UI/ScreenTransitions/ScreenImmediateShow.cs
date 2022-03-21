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

            await WaitUnlocked();
            OnStart();

            await screen.AfterShowAsync();
            OnEnd();

            Destroy(this);
        }

        protected override void OnStart()
        {
            base.OnStart();
            screen.StartShow();
        }
    }
}
