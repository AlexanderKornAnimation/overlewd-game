using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public abstract class BasePopup : BaseScreen
    {
        public override void StartShow()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericPopupShow);
        }

        public override void StartHide()
        {
            
        }

        public override void MakeMissclick()
        {
            UIManager.MakePopupMissclick<PopupMissclickColored>();
        }

        public override ScreenShow Show()
        {
            return gameObject.AddComponent<ScreenBottomShow>();
        }

        public override ScreenHide Hide()
        {
            return gameObject.AddComponent<ScreenBottomHide>();
        }

        public void RunShowPopupProcess()
        {
            UIManager.ShowPopupProcess();
        }
    }
}
