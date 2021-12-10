using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class PortalPopup : BuildingPopup
    {
        public static bool isBuilded { get; private set; } = false;

        protected override void Start()
        {
            base.Start();
            Customize();
        }

        protected override void Customize()
        {
            base.Customize();
            buildingName.text = "Portal";
        }
        
        protected override void FreeBuildButtonClick()
        {
            base.FreeBuildButtonClick();
            isBuilded = true;
        }

        protected override void PaidBuildingButtonClick()
        {
            base.FreeBuildButtonClick();
            isBuilded = true;
        }
    }
}
