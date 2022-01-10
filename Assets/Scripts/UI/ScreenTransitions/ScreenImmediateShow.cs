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
            prepared = true;
            startTransitionListeners?.Invoke();
        }

        void Update()
        {
            if (!prepared || locked)
                return;

            endTransitionListeners?.Invoke();
            screen.AfterShow();
            Destroy(this);
        }
    }
}
