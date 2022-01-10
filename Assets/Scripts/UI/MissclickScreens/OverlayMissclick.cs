using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Overlewd
{
    public abstract class OverlayMissclick : BaseMissclick
    {
        protected override void OnClick()
        {
            UIManager.HideOverlay();
        }
    }

    public class OverlayMissclickClear : OverlayMissclick
    {

    }

    public class OverlayMissclickColored : OverlayMissclick
    {
        protected override void Awake()
        {
            base.Awake();

            image.color = new Color(0.0f, 0.0f, 0.0f, 0.8f);
        }

        public override MissclickShow Show()
        {
            return gameObject.AddComponent<MissclickFadeShow>();
        }

        public override MissclickHide Hide()
        {
            return gameObject.AddComponent<MissclickFadeHide>();
        }
    }
}
