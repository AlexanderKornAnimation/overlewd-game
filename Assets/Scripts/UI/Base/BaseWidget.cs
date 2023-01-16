using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public abstract class BaseWidget : MonoBehaviour
    {
        protected virtual void Awake()
        {
            UIManager.RegisterWidget(this);
        }
        
        protected virtual void OnDestroy()
        {
            UIManager.UnregisterWidget(this);
        }

        public virtual void OnGameDataEvent(GameDataEvent eventData)
        {

        }

        public virtual void OnUIEvent(UIEvent eventData)
        {

        }
    }
}
