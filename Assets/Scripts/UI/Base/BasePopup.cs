using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public abstract class BasePopup : BaseScreen
    {
        public override void StartShow()
        {
            SoundManager.PlayOneShoot(SoundManager.SoundPath.UI.PopupSlideOn);
        }

        public override void StartHide()
        {
            
        }

        public override void ShowMissclick()
        {
            UIManager.ShowPopupMissclick<PopupMissclickColored>();
        }

        public override ScreenShow Show()
        {
            return gameObject.AddComponent<ScreenBottomShow>();
        }

        public override ScreenHide Hide()
        {
            return gameObject.AddComponent<ScreenBottomHide>();
        }
    }
}
