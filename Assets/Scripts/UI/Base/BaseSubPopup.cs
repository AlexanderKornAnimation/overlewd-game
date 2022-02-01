using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public abstract class BaseSubPopup : BaseScreen
    {
        public override void StartShow()
        {
            
        }

        public override void StartHide()
        {
            
        }
        
        public override void ShowMissclick()
        {
            UIManager.ShowSubPopupMissclick<SubPopupMissclickColored>();
        }

        public override ScreenShow Show()
        {
            return gameObject.AddComponent<ScreenImmediateShow>();
        }

        public override ScreenHide Hide()
        {
            return gameObject.AddComponent<ScreenImmediateHide>();
        }
    }
}
