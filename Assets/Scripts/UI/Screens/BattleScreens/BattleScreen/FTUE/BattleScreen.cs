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
                var win = GameGlobalStates.ftueChapterData.key switch
                {
                    "chapter1" => inputData.ftueStageData.key switch
                    {
                        "battle1" => true,
                        "battle2" => false,
                        _ => true
                    },
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
                var defeat = GameGlobalStates.ftueChapterData.key switch
                {
                    "chapter1" => inputData.ftueStageData.key switch
                    {
                        "battle1" => false,
                        "battle2" => true,
                        _ => true
                    },
                    _ => true
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
                switch (GameGlobalStates.ftueChapterData.key)
                {
                    case "chapter1":
                        switch (inputData.ftueStageData.key)
                        {
                            case "battle1" :
                                GameGlobalStates.ftueChapterData.ShowNotifByKey("battletutor1");
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
                                GameGlobalStates.ftueChapterData.ShowNotifByKey("battletutor2");
                                break;
                            case "battle2":
                                GameGlobalStates.ftueChapterData.ShowNotifByKey("battletutor3");
                                await UIManager.WaitHideNotifications();
                                GameGlobalStates.ftueChapterData.ShowNotifByKey("bufftutor1");
                                break;
                            case "battle5":
                                GameGlobalStates.ftueChapterData.ShowNotifByKey("castletutor");
                                break;
                        }
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