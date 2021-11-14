using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Overlewd
{
    public abstract class PopupMissclick : BaseMissclick
    {
        protected override void OnClick()
        {
            UIManager.HidePopup();
        }
    }

    public class PopupMissclickClear : PopupMissclick
    {

    }

    public class PopupMissclickColored : PopupMissclick
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
