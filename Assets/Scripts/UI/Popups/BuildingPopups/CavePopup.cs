using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class CavePopup : BuildingPopup
    {
        public static bool isBuilded { get; private set; } = false;

        protected override void Start()
        {
            base.Start();
            Instantiate(Resources.Load("Prefabs/UI/Popups/BuildingPopups/CaveImage"), imageSpawnPoint);
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
