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
        }
        
        protected virtual void OnDestroy()
        {
            UIManager.widgetsGameDataListeners -= OnGameDataEvent;
        }

        public virtual void OnGameDataEvent(GameDataEvent eventData)
        {

        }
    }
}
