using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace Overlewd
{
    namespace FTUE
    {
        public class DialogScreen : Overlewd.DialogScreen
        {
            public override async Task AfterShowAsync()
            {
                SoundManager.GetEventInstance(FMODEventPath.Music_DialogScreen);
                await Task.CompletedTask;
            }

            public override async Task BeforeShowDataAsync()
            {
                dialogData = inputData.ftueStageData.dialogData;
                await GameData.FTUEStartStage(inputData.ftueStageData.id);
            }

            public override async Task BeforeHideDataAsync()
            {
                await GameData.FTUEEndStage(inputData.ftueStageData.id);
            }

            protected override void LeaveScreen()
            {
                switch (GameGlobalStates.ftueChapterData.key)
                {
                    case "chapter1":
                        switch (inputData.ftueStageData.key)
                        {
                            case "dialogue1":
                                UIManager.MakeScreen<BattleScreen>().
                                    SetData(new BattleScreenInData
                                    {
                                        ftueStageId = GameGlobalStates.ftueChapterData.GetStageByKey("battle1")?.id
                                    }).RunShowScreenProcess();
                                break;

                            default:
                                UIManager.ShowScreen<MapScreen>();
                                break;
                        }
                        break;

                    default:
                        UIManager.ShowScreen<MapScreen>();
                        break;
                }
            }
        }
    }
}