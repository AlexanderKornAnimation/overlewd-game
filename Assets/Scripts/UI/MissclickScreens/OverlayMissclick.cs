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
        public override void Show()
        {
            gameObject.AddComponent<MissclickColoredShow>();
        }

        public override void Hide()
        {
            gameObject.AddComponent<MissclickColoredHide>();
        }
    }
}
