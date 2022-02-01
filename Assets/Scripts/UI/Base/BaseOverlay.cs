using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public abstract class BaseOverlay : BaseScreen
    {
        public override void StartShow()
        {
            
        }

        public override void StartHide()
        {
            
        }
        
        public override void ShowMissclick()
        {
            UIManager.ShowOverlayMissclick<OverlayMissclickColored>();
        }

        public override ScreenShow Show()
        {
            return gameObject.AddComponent<ScreenRightShow>();
        }

        public override ScreenHide Hide()
        {
            return gameObject.AddComponent<ScreenRightHide>();
        }
    }
}
