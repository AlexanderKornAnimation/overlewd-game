using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
	namespace FTUE
	{
		public class BattleScreen : Overlewd.BattleScreen
		{
            protected override void Awake()
            {
                base.Awake();
                WannaWin(true);
            }

            protected override void BackButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                UIManager.ShowScreen<MapScreen>();
            }

            public override void BattleWin()
            {
                UIManager.MakePopup<VictoryPopup>().
                    SetData(new VictoryPopupInData
                    {
                        ftueStageData = inputData.ftueStageData
                    }).RunShowPopupProcess();
            }

            public override void BattleDefeat()
            {
                UIManager.MakePopup<DefeatPopup>().
                    SetData(new DefeatPopupInData
                    {
                        ftueStageData = inputData.ftueStageData
                    }).RunShowPopupProcess();
            }

            public override async Task BeforeShowDataAsync()
            {
                await GameData.FTUEStartStage(inputData.ftueStageData.id);
            }

            public override async Task BeforeHideDataAsync()
            {
                await GameData.FTUEEndStage(inputData.ftueStageData.id);
            }

            //
            private async void ShowStartNotifications()
            {
                switch (GameGlobalStates.ftueChapterData.key)
                {
                    case "chapter1":
                        switch (inputData.ftueStageData.key)
                        {
                            case "battle1" :
                                UIManager.MakeNotification<DialogNotification>().
                                    SetData(new DialogNotificationInData 
                                    {
                                        dialogData = GameGlobalStates.GetFTUENotificationByKey("battletutor1")
                                    }).RunShowNotificationProcess();
                                break;
                        }
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

                switch (GameGlobalStates.ftueChapterData.key)
                {
                    case "chapter1":
                        switch (inputData.ftueStageData.key)
                        {
                            case "battle1":
                                UIManager.MakeNotification<DialogNotification>().
                                    SetData(new DialogNotificationInData
                                    {
                                        dialogData = GameGlobalStates.GetFTUENotificationByKey("battletutor2")
                                    }).RunShowNotificationProcess();
                                break;
                            case "battle2":
                                UIManager.MakeNotification<DialogNotification>().
                                    SetData(new DialogNotificationInData
                                    {
                                        dialogData = GameGlobalStates.GetFTUENotificationByKey("battletutor3")
                                    }).RunShowNotificationProcess();
                                await UIManager.WaitHideNotifications();

                                UIManager.MakeNotification<DialogNotification>().
                                    SetData(new DialogNotificationInData
                                    {
                                        dialogData = GameGlobalStates.GetFTUENotificationByKey("bufftutor1")
                                    }).RunShowNotificationProcess();
                                break;
                            case "battle5":
                                UIManager.MakeNotification<DialogNotification>().
                                    SetData(new DialogNotificationInData
                                    {
                                        dialogData = GameGlobalStates.GetFTUENotificationByKey("castletutor")
                                    }).RunShowNotificationProcess();
                                break;
                        }
                        break;
                }

                await Task.CompletedTask;
            }
        }
	}
}