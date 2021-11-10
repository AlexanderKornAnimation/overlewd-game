using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class CavePopup : BuildingPopup
    {

        protected override void Start()
        {
            base.Start();
            var imagePrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Popups/BuildingPopups/CavePopup"));
            imagePrefab.transform.SetParent(imageSpawnPoint);
        }
    }

}
