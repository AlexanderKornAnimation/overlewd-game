using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Overlewd
{
    public abstract class ScreenTransition : MonoBehaviour
    {
        protected RectTransform screenRectTransform;
        protected BaseScreen screen;

        protected float duration = 0.3f;
        protected float time = 0.0f;

        protected bool prepared { get; set; } = false;
        public bool locked { get; set; } = false;

        public Action startTransitionListeners;
        public Action endTransitionListeners;

        protected float deltaTimeInc
        {
            get
            {
                return Time.deltaTime > 1.0f / 60.0f ? 1.0f / 60.0f : Time.deltaTime;
            }
        }

        protected virtual void Awake()
        {
            screenRectTransform = GetComponent<RectTransform>();
            screen = GetComponent<BaseScreen>();
        }
    }

    public abstract class ScreenShow : ScreenTransition
    {

    }
    public abstract class ScreenHide : ScreenTransition
    {

    }
}
