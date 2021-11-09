using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public abstract class BasePopup : BaseScreen
    {
        void Start()
        {

        }

        void Update()
        {

        }

        public override void Show()
        {
            UIManager.ShowPopupMissclick();
            gameObject.AddComponent<ImmediateShow>();
        }

        public override void Hide()
        {
            UIManager.HidePopupMissclick();
            gameObject.AddComponent<ImmediateHide>();
        }
    }
}
