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
                firstTimeReward.sprite = Resources.Load<Sprite>("Prefabs/UI/Common/Images/Recources/Crystal");
                reward1.sprite = Resources.Load<Sprite>("Prefabs/UI/Common/Images/Recources/Gem");
                reward2.sprite = Resources.Load<Sprite>("Prefabs/UI/Common/Images/Recources/Stone");
                reward3.sprite = Resources.Load<Sprite>("Prefabs/UI/Common/Images/Recources/Wood");
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
