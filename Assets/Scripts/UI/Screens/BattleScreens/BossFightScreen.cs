using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
	public class BossFightScreen : BaseBossFightScreen
	{
        protected BossFightScreenInData inputData;

        public BossFightScreen SetData(BossFightScreenInData data)
        {
            inputData = data;
            return this;
        }

        public override async Task BeforeShowMakeAsync()
        {
            WannaWin(true);

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
                await GameData.FTUEStartStage(inputData.ftueStageId.Value);
            }
            else
            {
                await GameData.EventStageStartAsync(inputData.eventStageId.Value);
            }
        }

        public override async Task BeforeHideDataAsync()
        {
            if (inputData.ftueStageId.HasValue)
            {
                await GameData.FTUEEndStage(inputData.ftueStageId.Value);
            }
            else
            {
                await GameData.EventStageEndAsync(inputData.eventStageId.Value);
            }
        }

        public override void OnBattleEvent(BattleEvent battleEvent)
        {

        }

        public override void OnBattleNotification(string notifKey)
        {

        }

        public override BattleManagerInData GetBattleData()
        {
            if (inputData.ftueStageId.HasValue)
            {
                return BattleManagerInData.InstFromFTUEStage(inputData?.ftueStageData);
            }
            else
            {
                return BattleManagerInData.InstFromEventStage(inputData?.eventStageData);
            }
        }
    }

    public class BossFightScreenInData : BaseScreenInData
    {
        
    }
}