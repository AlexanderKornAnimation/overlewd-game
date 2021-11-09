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

        void Update()
        {

        }

        public override void Show()
        {
            UIManager.ShowOverlayMissclick();
            gameObject.AddComponent<OverlayShow>();
        }

        public override void Hide()
        {
            UIManager.HideOverlayMissclick();
            gameObject.AddComponent<OverlayHide>();
        }
    }
}
