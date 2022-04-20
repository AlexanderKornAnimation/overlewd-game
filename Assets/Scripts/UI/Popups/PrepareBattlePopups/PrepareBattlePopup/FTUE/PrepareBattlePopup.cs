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

                    foreach (var phase in battleData.battlePhases)
                    {
                        foreach (var charId in phase.enemyCharacters)
                        {
                            var character = NSPrepareBattlePopup.EnemyCharacter.GetInstance(content);
                        }
                    }

                    if (battleData.firstRewards.Count > 0)
                    {
                        var firstReward = battleData.firstRewards[0];

                        firstTimeReward.gameObject.SetActive(true);
                        firstTimeReward.sprite = ResourceManager.LoadSprite(firstReward.icon);
                        firstTimeRewardCount.text = firstReward.amount.ToString();
                    }

                    if (battleData.rewards.Count < 1)
                        return;

                    for (int i = 0; i < battleData.rewards.Count; i++)
                    {
                        var reward = battleData.rewards[i];
                        rewards[i].gameObject.SetActive(true);
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

            protected override void EditTeamButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                UIManager.MakeScreen<TeamEditScreen>().SetDataFromMapScreen(stageData).RunShowScreenProcess();
            }

            protected override void BattleButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_StartBattle);
                UIManager.MakeScreen<BattleScreen>().SetStageData(stageData).RunShowScreenProcess();
            }
        }
    }
}