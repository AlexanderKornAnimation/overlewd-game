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

        protected override void ShowMissclick()
        {
            UIManager.ShowPopupMissclick<PopupMissclickColored>();
        }

        public override void Show()
        {
            ShowMissclick();
            gameObject.AddComponent<FadeShow>();
        }

        public override void Hide()
        {
            gameObject.AddComponent<FadeHide>();
        }
    }
}
