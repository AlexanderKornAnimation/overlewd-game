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

    public class SubPopupMissclickClear : SubPopupMissclick
    {

    }

    public class SubPopupMissclickColored : SubPopupMissclick
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
