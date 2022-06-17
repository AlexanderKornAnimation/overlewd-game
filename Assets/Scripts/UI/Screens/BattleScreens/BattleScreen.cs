using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
	public class BattleScreen : BaseBattleScreen
	{
        public override void StartBattle()
        {
            base.StartBattle();
        }

        public override void EndBattle(BattleManagerOutData data)
        {
            base.EndBattle(data);

            endBattleData.battleWin = inputData.ftueStageData?.ftueState switch
            {
                ("battle2", "chapter1") => GameData.ftue.info.StageIsComplete("sex2", "chapter1"),
                _ => endBattleData.battleWin
            };

            if (endBattleData.battleWin)
            {
                UIManager.MakePopup<VictoryPopup>().
                    SetData(new VictoryPopupInData
                    {
                        ftueStageId = inputData.ftueStageId,
                        eventStageId = inputData.eventStageId
                    }).RunShowPopupProcess();
            }
            else
            {
                UIManager.MakePopup<DefeatPopup>().
                SetData(new DefeatPopupInData
                {
                    ftueStageId = inputData.ftueStageId,
                    eventStageId = inputData.eventStageId
                }).RunShowPopupProcess();
            }
        }

        public override async Task BeforeShowMakeAsync()
        {
            backButton.gameObject.SetActive(false);

            await Task.CompletedTask;
        }

        protected override void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            if (inputData.ftueStageId.HasValue)
            {
                UIManager.ShowScreen<MapScreen>();
            }
            else
            {
                UIManager.ShowScreen<EventMapScreen>();
            }
        }

        public override async Task BeforeShowDataAsync()
        {
            if (inputData.ftueStageId.HasValue)
            {
                await GameData.ftue.StartStage(inputData.ftueStageData.id);
            }
            else
            {
                await GameData.events.StageStart(inputData.eventStageId.Value);
            }
        }

        public override async Task AfterShowAsync()
        {
            bm.AfterShowBattleScreen();

            await Task.CompletedTask;
        }

        public override async Task BeforeHideDataAsync()
        {
            if (inputData.ftueStageId.HasValue)
            {
                await GameData.ftue.EndStage(inputData.ftueStageId.Value,
                    new AdminBRO.FTUEStageEndData
                    {
                        win = endBattleData.battleWin,
                        mana = endBattleData.manaSpent,
                        hp = endBattleData.hpSpent
                    });

                await GameData.player.Get();
            }
            else
            {
                await GameData.events.StageEnd(inputData.eventStageId.Value,
                    new AdminBRO.EventStageEndData
                    {
                        win = endBattleData.battleWin,
                        mana = endBattleData.manaSpent,
                        hp = endBattleData.hpSpent
                    });

                await GameData.player.Get();
            }
        }
    }
}