using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace FTUE
    {
        public class DefeatPopup : Overlewd.DefeatPopup
        {
            public override async Task BeforeShowMakeAsync()
            {
                switch (GameGlobalStates.ftueChapterData.key)
                {
                    case "chapter1":
                        switch (inputData.ftueStageData.key)
                        {
                            case "battle2":
                                UITools.DisableButton(magicGuildButton);
                                UITools.DisableButton(inventoryButton);
                                UITools.DisableButton(editTeamButton);
                                break;
                        }
                        break;
                }

                await Task.CompletedTask;
            }

            protected override void HaremButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_DefeatPopupHaremButtonClick);

                switch (GameGlobalStates.ftueChapterData.key)
                {
                    case "chapter1":
                        switch (inputData.ftueStageData.key)
                        {
                            case "battle2":
                                UIManager.MakeScreen<SexScreen>().
                                    SetData(new SexScreenInData
                                    {
                                        ftueStageId = GameGlobalStates.ftueChapterData.GetStageByKey("sex2")?.id
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