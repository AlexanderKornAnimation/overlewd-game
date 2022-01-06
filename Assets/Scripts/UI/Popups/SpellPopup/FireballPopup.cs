using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Resharper disable All

namespace Overlewd
{
    public class FireballPopup : SpellPopup
    {
        protected override void Awake()
        {
            base.Awake();
            ResourceManager.InstantiateWidgetPrefab("Prefabs/UI/Popups/SpellPopup/FireballImage", spawnPoint);
        }

    }
}