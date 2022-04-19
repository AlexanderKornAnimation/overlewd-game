using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


namespace Overlewd
{
    namespace FTUE
    {
        public class PrepareBattlePopup : Overlewd.PrepareBattlePopup
        {
            private AdminBRO.FTUEStageItem stageData;

            protected override void Customize()
            {
                if (stageData.battleId.HasValue)
                {
                    var battleData = GameData.GetBattleById(stageData.battleId.Value);
                    
                    if (battleData.rewards.Count < 1 || battleData.firstRewards.Count < 1)
                        return;                    
                    
                    var firstReward = battleData.firstRewards[0];

                    firstTimeReward.sprite = ResourceManager.LoadSprite(firstReward.icon);
                    firstTimeRewardCount.text = firstReward.amount.ToString();

                    for (int i = 0; i < rewards.Length; i++)
                    {
                        var reward = battleData.rewards[i];
                        rewards[i].sprite = ResourceManager.LoadSprite(reward.icon);
                        rewardsAmount[i].text = reward.amount.ToString();
                    }
                }
            }

            public override async Task BeforeShowMakeAsync()
            {
                Customize();

                await Task.CompletedTask;
            }

            public PrepareBattlePopup SetStageData(AdminBRO.FTUEStageItem data)
            {
                stageData = data;
                return this;
            }

            protected override void BattleButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_StartBattle);
                UIManager.MakeScreen<BattleScreen>().
                    SetStageData(stageData).RunShowScreenProcess();
            }
        }
    }
}
