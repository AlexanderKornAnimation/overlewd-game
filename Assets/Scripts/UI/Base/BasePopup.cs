using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public abstract class BasePopup : BaseScreen
    {
        public override void ShowMissclick()
        {
            UIManager.ShowPopupMissclick<PopupMissclickColored>();
        }

        public override ScreenShow Show()
        {
            return gameObject.AddComponent<ScreenTopShow>();
        }

        public override ScreenHide Hide()
        {
            return gameObject.AddComponent<ScreenTopHide>();
        }
    }
}
