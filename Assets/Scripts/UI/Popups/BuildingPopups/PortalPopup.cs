using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class PortalPopup : BuildingPopup
    {

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Customize()
        {
            base.Customize();
            buildingName.text = "Portal";
        }
    }
}
