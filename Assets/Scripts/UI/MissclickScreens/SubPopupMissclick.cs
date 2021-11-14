using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Overlewd
{
    public abstract class SubPopupMissclick : BaseMissclick
    {
        protected override void OnClick()
        {
            UIManager.HideSubPopup();
        }
    }

    public class SubPopupMissclickTransparency : SubPopupMissclick
    {


    }

    public class SubPopupMissclickColored : SubPopupMissclick
    {
        private float alphaMax = 0.8f;

        public override void Show()
        {
            gameObject.AddComponent<MissclickShow>();
        }

        public override void Hide()
        {
            gameObject.AddComponent<MissclickHide>();
        }

        public override void UpdateShow(float showPercent)
        {
            image.color = new Color(0.0f, 0.0f, 0.0f, alphaMax * showPercent);
        }

        public override void UpdateHide(float hidePercent)
        {
            image.color = new Color(0.0f, 0.0f, 0.0f, alphaMax * (1.0f - hidePercent));
        }
    }
}
