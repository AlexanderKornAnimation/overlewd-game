using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
	public class BossFightScreen : BaseBossFightScreen
	{
        private bool battleIsWin;

        public override void StartBattle()
        {
            backButton.gameObject.SetActive(false);
            skipButton.gameObject.SetActive(true);
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

        public override void BattleWin()
        {
            battleIsWin = true;
            UIManager.MakePopup<VictoryPopup>().
                SetData(new VictoryPopupInData
                {
                    ftueStageId = inputData.ftueStageId,
                    eventStageId = inputData.eventStageId
                }).RunShowPopupProcess();
        }

        public override void BattleDefeat()
        {
            UIManager.MakePopup<DefeatPopup>().
                SetData(new DefeatPopupInData
                {
                    ftueStageId = inputData.ftueStageId,
                    eventStageId = inputData.eventStageId
                }).RunShowPopupProcess();
        }

        public override async Task BeforeShowDataAsync()
        {
            if (inputData.ftueStageId.HasValue)
            {
                await GameData.ftue.StartStage(inputData.ftueStageId.Value);
            }
            else
            {
                await GameData.EventStageStartAsync(inputData.eventStageId.Value);
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
                       win = battleIsWin 
                    });
            }
            else
            {
                await GameData.EventStageEndAsync(inputData.eventStageId.Value,
                    new AdminBRO.EventStageEndData
                    {
                        win = battleIsWin
                    });
            }
        }
    }
}