using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class ForgePopup : BuildingPopup
    {
        protected override void Start()
        {
            base.Start();
            Instantiate(Resources.Load("Prefabs/UI/Popups/BuildingPopups/ForgePopup"), imageSpawnPoint);
        }
    }

}
