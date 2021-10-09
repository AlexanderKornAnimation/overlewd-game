using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class BaseOverlay : BaseScreen
    {
        void Start()
        {

        }

        void Update()
        {

        }

        public override void Show()
        {
            gameObject.AddComponent<OverlayShow>();
        }

        public override void Hide()
        {
            gameObject.AddComponent<OverlayHide>();
        }
    }
}
