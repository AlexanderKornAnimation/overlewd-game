using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace FTUE
    {
        public class PrepareBossFightPopup : Overlewd.PrepareBossFightPopup
        {
            protected override void Customize()
            {
               
            }

            protected override void BattleButtonClick()
            {
                UIManager.ShowScreen<BossFightScreen>();
            }

            protected override void PrepareButtonClick()
            {

            }
        }
    }
}
