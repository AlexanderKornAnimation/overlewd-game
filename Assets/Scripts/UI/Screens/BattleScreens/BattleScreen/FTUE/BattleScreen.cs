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
                var win = inputData.ftueStageData.ftueState switch
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
                        }).RunShowPopupProcess();
                }
                else
                {
                    UIManager.MakePopup<DefeatPopup>().
                    SetData(new DefeatPopupInData
                    {
                        ftueStageId = inputData.ftueStageId
                    }).RunShowPopupProcess();
                }
            }

            public override void BattleDefeat()
            {
                var defeat = inputData.ftueStageData.ftueState switch
                {
                    ("battle2", "chapter1") => true,
                    _ => false
                };

                if (defeat)
                {
                    UIManager.MakePopup<DefeatPopup>().
                        SetData(new DefeatPopupInData
                        {
                            ftueStageId = inputData.ftueStageId
                        }).RunShowPopupProcess();
                }
                else
                {
                    UIManager.MakePopup<VictoryPopup>().
                        SetData(new VictoryPopupInData
                        {
                            ftueStageId = inputData.ftueStageId
                        }).RunShowPopupProcess();
                }
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
                switch (inputData.ftueStageData.ftueState)
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

                switch (inputData.ftueStageData.ftueState)
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

            public override void OnBattleEvent(BattleEvent battleEvent)
            {

            }

            public override BattleManagerInData GetBattleData()
            {
                return BattleManagerInData.InstFromFTUEStage(inputData?.ftueStageData);
            }
        }
	}
}