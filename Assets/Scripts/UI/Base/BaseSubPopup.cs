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

        protected override void ShowMissclick()
        {
            UIManager.ShowSubPopupMissclick<SubPopupMissclickColored>();
        }

        public override void Show()
        {
            ShowMissclick();
            gameObject.AddComponent<ImmediateShow>();
        }

        public override void Hide()
        {
            gameObject.AddComponent<ImmediateHide>();
        }
    }
}
