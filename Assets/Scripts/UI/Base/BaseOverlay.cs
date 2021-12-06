using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public abstract class BaseOverlay : BaseScreen
    {
        void Start()
        {

        }

        protected override void ShowMissclick()
        {
            UIManager.ShowOverlayMissclick<OverlayMissclickColored>();
        }

        public override void Show()
        {
            ShowMissclick();
            gameObject.AddComponent<OverlayShow>();
        }

        public override void Hide()
        {
            gameObject.AddComponent<OverlayHide>();
        }
    }
}
