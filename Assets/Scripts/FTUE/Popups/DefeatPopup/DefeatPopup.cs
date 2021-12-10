using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Resharper disable All

namespace Overlewd
{
    namespace FTUE
    {
        public class DefeatPopup : Overlewd.DefeatPopup
        {
            protected override void MagicGuildButtonClick()
            {
                UIManager.ShowScreen<MapScreen>();
            }

            protected override void InventoryButtonClick()
            {

            }

            protected override void HaremButtonClick()
            {

            }
        }
    }
}