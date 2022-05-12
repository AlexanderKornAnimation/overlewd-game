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
                switch (inputData.ftueStageData.ftueState)
                {
                    case ("battle2", "chapter1"):
                        UITools.DisableButton(magicGuildButton);
                        UITools.DisableButton(inventoryButton);
                        UITools.DisableButton(editTeamButton);
                        break;

                    default:

                        break;
                }

                await Task.CompletedTask;
            }

            protected override void HaremButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_DefeatPopupHaremButtonClick);

                switch (inputData.ftueStageData.ftueState)
                {
                    case ("battle2", "chapter1"):
                        UIManager.MakeScreen<SexScreen>().
                            SetData(new SexScreenInData
                            {
                                ftueStageId = GameData.ftue.mapChapter.GetStageByKey("sex2")?.id
                            }).RunShowScreenProcess();
                        break;

                    default:
                        UIManager.ShowScreen<MapScreen>();
                        break;
                }
            }
        }
    }
}