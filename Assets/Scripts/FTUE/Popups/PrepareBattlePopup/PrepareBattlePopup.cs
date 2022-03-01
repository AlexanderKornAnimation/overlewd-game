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
                firstTimeReward.sprite = ResourceManager.InstantiateAsset<Sprite>("Common/Images/Crystal");
                reward1.sprite = ResourceManager.InstantiateAsset<Sprite>("Common/Images/Gold");
                reward2.sprite = ResourceManager.InstantiateAsset<Sprite>("Common/Images/Stone");
                reward3.sprite = ResourceManager.InstantiateAsset<Sprite>("Common/Images/Wood");
            }

            protected override void BattleButtonClick()
            {
                SoundManager.PlayOneShoot(SoundPath.UI_GenericButtonClick);
                UIManager.ShowScreen<BattleScreen>();
            }

            protected override void PrepareButtonClick()
            {

            }
        }
    }
}
