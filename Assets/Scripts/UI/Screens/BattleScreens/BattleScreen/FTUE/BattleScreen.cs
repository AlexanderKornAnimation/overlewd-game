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
            protected AdminBRO.FTUEStageItem stageData;
            public BattleScreen SetStageData(AdminBRO.FTUEStageItem data)
            {
                stageData = data;
                return this;
            }

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
                    SetStageData(stageData).RunShowPopupProcess();
            }

            public override void BattleDefeat()
            {
                UIManager.MakePopup<DefeatPopup>().
                    SetStageData(stageData).RunShowPopupProcess();
            }

            public override async Task BeforeShowDataAsync()
            {
                await GameData.FTUEStartStage(stageData.id);
            }

            public override async Task BeforeHideDataAsync()
            {
                await GameData.FTUEEndStage(stageData.id);
            }

            //
            private async void ShowStartNotifications()
            {
                switch (GameGlobalStates.ftueChapterData.key)
                {
                    case "chapter1":
                        switch (stageData.key)
                        {
                            case "battle1" :
                                UIManager.MakeNotification<DialogNotification>().
                                    SetDialogData(GameGlobalStates.GetFTUENotificationByKey("battletutor1")).
                                    RunShowNotificationProcess();
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
                        switch (stageData.key)
                        {
                            case "battle1":
                                UIManager.MakeNotification<DialogNotification>().
                                    SetDialogData(GameGlobalStates.GetFTUENotificationByKey("battletutor2")).
                                    RunShowNotificationProcess();
                                break;
                            case "battle2":
                                UIManager.MakeNotification<DialogNotification>().
                                    SetDialogData(GameGlobalStates.GetFTUENotificationByKey("battletutor3")).
                                    RunShowNotificationProcess();
                                await UIManager.WaitHideNotifications();

                                UIManager.MakeNotification<DialogNotification>().
                                    SetDialogData(GameGlobalStates.GetFTUENotificationByKey("bufftutor1")).
                                    RunShowNotificationProcess();
                                break;
                            case "battle5":
                                UIManager.MakeNotification<DialogNotification>().
                                    SetDialogData(GameGlobalStates.GetFTUENotificationByKey("castletutor")).
                                    RunShowNotificationProcess();
                                break;
                        }
                        break;
                }

                await Task.CompletedTask;
            }
        }
	}
}