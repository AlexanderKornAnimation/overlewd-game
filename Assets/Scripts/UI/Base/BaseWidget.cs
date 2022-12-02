using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public abstract class BaseWidget : MonoBehaviour
    {
        protected virtual void Awake()
        {
            UIManager.widgetsGameDataListeners += OnGameDataEvent;
            UIManager.widgetsUIEventListeners += OnUIEvent;
        }
        
        protected virtual void OnDestroy()
        {
            UIManager.widgetsGameDataListeners -= OnGameDataEvent;
            UIManager.widgetsUIEventListeners -= OnUIEvent;
        }

        public virtual void OnGameDataEvent(GameDataEvent eventData)
        {

        }

        public virtual void OnUIEvent(UIEvent eventData)
        {

        }
    }
}
