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
        public class VictoryPopup : Overlewd.VictoryPopup
        {
            protected override void NextButtonClick()
            {
                UIManager.ShowScreen<BossFightScreen>();
            }

            protected override void RepeatButtonClick()
            {
                UIManager.ShowScreen<BattleScreen>();
            }
        }
    }
}