using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
	public class BattleScreen : BaseBattleScreen
	{
        protected BattleScreenInData inputData;

        public BattleScreen SetData(BattleScreenInData data)
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
            var win = inputData.ftueStageData?.ftueState switch
            {
                ("battle2", "chapter1") => false,
                _ => true
            };

            if (win)
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

        public override void BattleDefeat()
        {
            var defeat = inputData.ftueStageData?.ftueState switch
            {
                ("battle2", "chapter1") => true,
                _ => false
            };

            if (defeat)
            {
                UIManager.MakePopup<DefeatPopup>().
                    SetData(new DefeatPopupInData
                    {
                        ftueStageId = inputData.ftueStageId,
                        eventStageId = inputData.eventStageId
                    }).RunShowPopupProcess();
            }
            else
            {
                UIManager.MakePopup<VictoryPopup>().
                    SetData(new VictoryPopupInData
                    {
                        ftueStageId = inputData.ftueStageId,
                        eventStageId = inputData.eventStageId
                    }).RunShowPopupProcess();
            }
        }

        public override async Task BeforeShowDataAsync()
        {
            if (inputData.ftueStageId.HasValue)
            {
                await GameData.FTUEStartStage(inputData.ftueStageData.id);
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
                await GameData.FTUEEndStage(inputData.ftueStageData.id);
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
            return inputData.ftueStageId.HasValue ?
                BattleManagerInData.InstFromFTUEStage(inputData.ftueStageData) :
                BattleManagerInData.InstFromEventStage(inputData.eventStageData);
        }

        //
        private async void ShowStartNotifications()
        {
            switch (inputData.ftueStageData?.ftueState)
            {
                case ("battle1", "chapter1"):
                    GameData.ftue.mapChapter.ShowNotifByKey("battletutor1");
                    break;
            }

            await Task.CompletedTask;
        }

        private async void ShowEndNotifications()
        {
            var waitPopupsEndTr = true;
            while (waitPopupsEndTr)
            {
                var victoryPopupTrState = UIManager.GetPopup<VictoryPopup>()?.IsTransitionState() ?? false;
                var defeatPopupTrState = UIManager.GetPopup<DefeatPopup>()?.IsTransitionState() ?? false;
                waitPopupsEndTr = victoryPopupTrState || defeatPopupTrState;
                await UniTask.NextFrame();
            }

            switch (inputData.ftueStageData?.ftueState)
            {
                case ("battle1", "chapter1"):
                    GameData.ftue.mapChapter.ShowNotifByKey("battletutor2");
                    break;
                case ("battle2", "chapter1"):
                    GameData.ftue.mapChapter.ShowNotifByKey("battletutor3");
                    await UIManager.WaitHideNotifications();
                    GameData.ftue.mapChapter.ShowNotifByKey("bufftutor1");
                    break;
                case ("battle5", "chapter1"):
                    GameData.ftue.mapChapter.ShowNotifByKey("castletutor");
                    break;
            }

            await Task.CompletedTask;
        }
    }

    public class BattleScreenInData : BaseScreenInData
    {
        
    }
}