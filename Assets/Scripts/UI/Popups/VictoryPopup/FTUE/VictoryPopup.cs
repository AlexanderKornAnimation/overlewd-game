using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

// Resharper disable All

namespace Overlewd
{
    namespace FTUE
    {
        public class VictoryPopup : Overlewd.VictoryPopup
        {
            public override async Task BeforeShowMakeAsync()
            {
                switch (GameGlobalStates.ftueChapterData.key)
                {
                    case "chapter1":
                        switch (inputData.ftueStageData.key)
                        {
                            case "battle1":
                                UITools.DisableButton(repeatButton);
                                break;
                        }
                        break;
                }

                await Task.CompletedTask;
            }

            protected override void NextButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);

                switch (GameData.ftue.activeChapter.key)
                {
                    case "chapter1":
                        switch (inputData.ftueStageData.key)
                        {
                            case "battle4":
                                UIManager.ShowScreen<CastleScreen>();
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

            protected override void RepeatButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                UIManager.MakeScreen<BattleScreen>().
                    SetData(new BattleScreenInData
                    {
                        ftueStageId = inputData.ftueStageId
                    }).RunShowScreenProcess();
            }
        }
    }
}