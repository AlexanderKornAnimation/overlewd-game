using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public abstract class BaseSubPopup : BaseScreen
    {
        void Start()
        {

        }

        void Update()
        {

        }

        public override void Show()
        {
            UIManager.ShowSubPopupMissclick();
            gameObject.AddComponent<ImmediateShow>();
        }

        public override void Hide()
        {
            UIManager.HideSubPopupMissclick();
            gameObject.AddComponent<ImmediateHide>();
        }
    }
}
