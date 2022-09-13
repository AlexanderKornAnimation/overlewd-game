using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public abstract class BaseWidget : MonoBehaviour
    {
        public virtual void OnGameDataEvent(GameDataEvent eventData)
        {

        }

        protected virtual void OnDestroy()
        {
            UIManager.widgetsGameDataListeners -= OnGameDataEvent;
        }
    }
}
