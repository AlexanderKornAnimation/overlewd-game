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
                firstTimeReward.sprite = ResourceManager.InstantiateAsset<Sprite>("Common/Images/Crystal");
                reward1.sprite = ResourceManager.InstantiateAsset<Sprite>("Common/Images/Gem");
                reward2.sprite = ResourceManager.InstantiateAsset<Sprite>("Common/Images/Stone");
                reward3.sprite = ResourceManager.InstantiateAsset<Sprite>("Common/Images/Wood");
            }

            protected override void BattleButtonClick()
            {
                SoundManager.PlayOneShoot(SoundPath.UI_StartBattle);
                UIManager.ShowScreen<BossFightScreen>();
            }

            protected override void PrepareButtonClick()
            {

            }
        }
    }
}
