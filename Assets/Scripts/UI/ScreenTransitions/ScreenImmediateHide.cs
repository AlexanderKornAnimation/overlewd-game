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
            prepared = true;
            startTransitionListeners?.Invoke();
        }

        void Update()
        {
            if (!prepared || locked)
                return;

            endTransitionListeners?.Invoke();
            screen.AfterHide();
            Destroy(gameObject);
        }
    }
}
