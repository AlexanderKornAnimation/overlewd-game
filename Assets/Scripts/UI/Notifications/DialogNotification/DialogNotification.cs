using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class DialogNotification : BuyingNotification
    {
        protected override void Awake()
        {
            base.Awake();

            button.gameObject.SetActive(false);
        }
    }
}
