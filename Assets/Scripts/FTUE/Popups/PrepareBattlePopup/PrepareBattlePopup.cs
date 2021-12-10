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
                firstTimeReward.sprite = Resources.Load<Sprite>("Prefabs/UI/Common/Images/Recources/Crystal");
                reward1.sprite = Resources.Load<Sprite>("Prefabs/UI/Common/Images/Recources/Gold");
                reward2.sprite = Resources.Load<Sprite>("Prefabs/UI/Common/Images/Recources/Stone");
                reward3.sprite = Resources.Load<Sprite>("Prefabs/UI/Common/Images/Recources/Wood");
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
