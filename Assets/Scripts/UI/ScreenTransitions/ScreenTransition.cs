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

        protected bool prepared { get; private set; } = false;
        protected bool started { get; private set; } = false;
        private List<ScreenTransition> lockers = new List<ScreenTransition>();
        public bool locked
        { 
            get 
            {
                return lockers.Count > 0;
            } 
        }
        private List<ScreenTransition> lockToPrepare = new List<ScreenTransition>();
        private List<ScreenTransition> lockToEnd = new List<ScreenTransition>();
        private Action preparedListeners;
        private Action startListeners;
        private Action endListeners;

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

        public void AddPreparedListener(Action listener)
        {
            preparedListeners += listener;
        }

        public void AddStartListener(Action listener)
        {
            startListeners += listener;
        }

        public void AddEndListener(Action listenter)
        {
            endListeners += listenter;
        }

        private void AddLocker(ScreenTransition locker)
        {
            if (locker != null)
            {
                if (!lockers.Contains(locker))
                {
                    lockers.Add(locker);
                }
            }
        }

        private void RemoveLocker(ScreenTransition locker)
        {
            if (locker != null)
            {
                lockers.Remove(locker);
            }
        }

        public void LockToPrepare(ScreenTransition[] transitions)
        {
            foreach (var item in transitions)
            {
                if (item != null)
                {
                    if (!lockToPrepare.Contains(item))
                    {
                        lockToPrepare.Add(item);
                    }
                    item.AddLocker(this);
                }
            }
        }

        private void FreeLockToPrepare()
        {
            foreach (var item in lockToPrepare)
            {
                item.RemoveLocker(this);
            }
            lockToPrepare.Clear();
        }

        public void LockToEnd(ScreenTransition[] transitions)
        {
            foreach (var item in transitions)
            {
                if (item != null)
                {
                    if (!lockToEnd.Contains(item))
                    {
                        lockToEnd.Add(item);
                    }
                    item.AddLocker(this);
                }
            }
        }

        private void FreeLockToEnd()
        {
            foreach (var item in lockToEnd)
            {
                item.RemoveLocker(this);
            }
            lockToEnd.Clear();
        }

        protected void OnPrepared()
        {
            prepared = true;
            preparedListeners?.Invoke();
            FreeLockToPrepare();
        }

        protected void OnStart()
        {
            if (started)
                return;
            startListeners?.Invoke();
            started = true;
        }

        protected void OnEnd()
        {
            endListeners?.Invoke();
            FreeLockToEnd();
        }
    }

    public abstract class ScreenShow : ScreenTransition
    {

    }
    public abstract class ScreenHide : ScreenTransition
    {

    }
}
