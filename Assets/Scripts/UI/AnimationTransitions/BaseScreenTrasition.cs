using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Overlewd
{
    public abstract class BaseScreenTrasition : MonoBehaviour
    {
        protected BaseScreen screen;
        protected RectTransform screenRectTransform;

        protected float duration = 0.3f;
        protected float time = 0.0f;

        protected bool prepared = false;

        protected virtual void Awake()
        {
            screen = GetComponent<BaseScreen>();
            screenRectTransform = GetComponent<RectTransform>();
        }

        protected async Task WaitPrepareShowAsync()
        {
            if (screen != null)
            {
                await screen.PrepareShowAsync();
            }
            prepared = true;
        }

        protected async Task WaitAfterShowAsync()
        {
            if (screen != null)
            {
                await screen.AfterShowAsync();
            }
        }

        protected async Task WaitPrepareHideAsync()
        {
            if (screen != null)
            {
                await screen.PrepareHideAsync();
            }
            prepared = true;
        }

        protected async Task WaitAfterHideAsync()
        {
            if (screen != null)
            {
                await screen.AfterHideAsync();
            }
        }
    }
}
