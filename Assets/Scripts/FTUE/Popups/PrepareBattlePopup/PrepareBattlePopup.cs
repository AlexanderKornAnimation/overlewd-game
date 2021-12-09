using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Overlewd
{
    namespace FTUE
    {
        public class PrepareBattlePopup : Overlewd.PrepareBattlePopup
        {
            protected override void Customize()
            {
                
            }

            protected override void BattleButtonClick()
            {
                UIManager.ShowScreen<BattleScreen>();
            }

            protected override void PrepareButtonClick()
            {

            }
        }
    }
}
