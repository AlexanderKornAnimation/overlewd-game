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

        private List<ScreenTransition> prepareLockers = new List<ScreenTransition>();
        private List<ScreenTransition> endLockers = new List<ScreenTransition>();
        public bool locked
        { 
            get 
            {
                return (prepareLockers.Count > 0) || (endLockers.Count > 0);
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

            UIManager.AddUserInputLocker(new UserInputLocker(this));
        }

        protected virtual void OnDestroy()
        {
            UIManager.RemoveUserInputLocker(new UserInputLocker(this));
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

        private void AddPrepareLocker(ScreenTransition locker)
        {
            if (locker != null)
            {
                if (!prepareLockers.Contains(locker))
                {
                    prepareLockers.Add(locker);
                }
            }
        }

        private void RemovePrepareLocker(ScreenTransition locker)
        {
            if (locker != null)
            {
                prepareLockers.Remove(locker);
            }
        }

        private void AddEndLocker(ScreenTransition locker)
        {
            if (locker != null)
            {
                if (!endLockers.Contains(locker))
                {
                    endLockers.Add(locker);
                }
            }
        }

        private void RemoveEndLocker(ScreenTransition locker)
        {
            if (locker != null)
            {
                endLockers.Remove(locker);
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
                    item.AddPrepareLocker(this);
                }
            }
        }

        private void FreeLockToPrepare()
        {
            foreach (var item in lockToPrepare)
            {
                item.RemovePrepareLocker(this);
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
                    item.AddEndLocker(this);
                }
            }
        }

        private void FreeLockToEnd()
        {
            foreach (var item in lockToEnd)
            {
                item.RemoveEndLocker(this);
            }
            lockToEnd.Clear();
        }

        protected void OnPrepared()
        {
            prepared = true;
            preparedListeners?.Invoke();
            FreeLockToPrepare();
            OnPreparedCalls();
        }

        protected virtual void OnPreparedCalls()
        {

        }

        protected void OnStart()
        {
            if (started)
                return;
            startListeners?.Invoke();
            started = true;
            OnStartCalls();
        }

        protected virtual void OnStartCalls()
        {
            
        }

        protected void OnEnd()
        {
            endListeners?.Invoke();
            FreeLockToEnd();
            OnEndCalls();
        }

        protected virtual void OnEndCalls()
        {
            
        }
    }

    public abstract class ScreenShow : ScreenTransition
    {
        
    }

    public abstract class ScreenHide : ScreenTransition
    {
        
    }
}
