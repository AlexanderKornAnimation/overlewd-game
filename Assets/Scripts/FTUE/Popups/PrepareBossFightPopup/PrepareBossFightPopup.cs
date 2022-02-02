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
                firstTimeReward.sprite = Resources.Load<Sprite>("Common/Images/Crystal");
                reward1.sprite = Resources.Load<Sprite>("Common/Images/Gem");
                reward2.sprite = Resources.Load<Sprite>("Common/Images/Stone");
                reward3.sprite = Resources.Load<Sprite>("Common/Images/Wood");
            }

            protected override void BattleButtonClick()
            {
                SoundManager.PlayOneShoot(SoundManager.SoundPath.UI.ButtonClick);
                UIManager.ShowScreen<BossFightScreen>();
            }

            protected override void PrepareButtonClick()
            {

            }
        }
    }
}
