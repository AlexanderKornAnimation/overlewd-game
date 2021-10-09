using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public abstract class BaseScreen : MonoBehaviour
    {
        void Start()
        {

        }

        void Update()
        {

        }

        public virtual void Show()
        {
            gameObject.AddComponent<ScreenShow>();
        }

        public virtual void Hide()
        {
            gameObject.AddComponent<ScreenHide>();
        }
    }
}
