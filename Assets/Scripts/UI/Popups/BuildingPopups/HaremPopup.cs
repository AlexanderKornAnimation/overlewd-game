using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class HaremPopup : BuildingPopup
    {

        protected override void Awake()
        {
            base.Awake();
            ResourceManager.InstantiateWidgetPrefab("Prefabs/UI/Popups/BuildingPopups/HaremImage", imageSpawnPoint);
        }

    }

}
