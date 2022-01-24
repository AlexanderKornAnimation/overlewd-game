using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class ForgePopup : BuildingPopup
    {
        protected override void Awake()
        {
            base.Awake();
            ResourceManager.InstantiateWidgetPrefab("Prefabs/UI/Popups/BuildingPopups/ForgeImage", imageSpawnPoint);
        }
    }

}
