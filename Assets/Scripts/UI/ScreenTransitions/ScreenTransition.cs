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
        private List<ScreenTransition> lockers = new List<ScreenTransition>();
        public bool locked
        { 
            get 
            {
                return lockers.Count > 0;
            } 
        }

        protected Action preparedTransitionListeners;
        protected Action endTransitionListeners;

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

        public void AddLocker(ScreenTransition locker)
        {
            if (locker != null)
            {
                if (!lockers.Contains(locker))
                {
                    lockers.Add(locker);
                }
            }
        }

        public void RemoveLocker(ScreenTransition locker)
        {
            if (locker != null)
            {
                lockers.Remove(locker);
            }
        }

        public void AddPreparedListener(Action listener)
        {
            preparedTransitionListeners += listener;
        }

        public void AddEndListener(Action listenter)
        {
            endTransitionListeners += listenter;
        }

        public void InitializerCall(Action initializer)
        {
            initializer?.Invoke();
        }
    }

    public abstract class ScreenShow : ScreenTransition
    {

    }
    public abstract class ScreenHide : ScreenTransition
    {

    }
}
