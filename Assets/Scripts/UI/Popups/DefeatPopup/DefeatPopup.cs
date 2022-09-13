using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class DefeatPopup : BasePopupParent<DefeatPopupInData>
    {
        private Button magicGuildButton;
        private Button overlordButton;
        private Button haremButton;
        private Button editTeamButton;
        private Button repeatButton;
        private Button mapButton;

        void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Popups/DefeatPopup/DefeatPopup", transform);

            var canvas = screenInst.transform.Find("Canvas");

            magicGuildButton = canvas.Find("MagicGuildButton").GetComponent<Button>();
            magicGuildButton.onClick.AddListener(MagicGuildButtonClick);

            overlordButton = canvas.Find("OverlordButton").GetComponent<Button>();
            overlordButton.onClick.AddListener(OverlordButtonClick);

            haremButton = canvas.Find("HaremButton").GetComponent<Button>();
            haremButton.onClick.AddListener(HaremButtonClick);

            editTeamButton = canvas.Find("EditTeamButton").GetComponent<Button>();
            editTeamButton.onClick.AddListener(EditTeamButtonClick);

            repeatButton = canvas.Find("RepeatButton").GetComponent<Button>();
            repeatButton.onClick.AddListener(RepeatButtonClick);

            mapButton = canvas.Find("MapButton").GetComponent<Button>();
            mapButton.onClick.AddListener(MapButtonClick);
        }

        public override async Task BeforeShowMakeAsync()
        {
            switch (inputData.ftueStageData?.ftueState)
            {
                case ("battle2", "chapter1"):
                    UITools.DisableButton(magicGuildButton);
                    UITools.DisableButton(overlordButton);
                    UITools.DisableButton(editTeamButton);
                    UITools.DisableButton(repeatButton);
                    UITools.DisableButton(mapButton);
                    break;
                default:
                    break;
            }

            await Task.CompletedTask;
        }

        public override async Task AfterShowAsync()
        {
            switch (inputData.ftueStageData?.ftueState)
            {
                case ("battle2", "chapter1"):
                    GameData.ftue.mapChapter.ShowNotifByKey("bufftutor1", false);
                    break;
                case (_, _):
                    switch (GameData.ftue.activeChapter.key)
                    {
                        case "chapter1":
                            SoundManager.PlayOneShot(FMODEventPath.VO_Ulvi_Losing_a_battle);
                            break;
                        case "chapter2":
                            SoundManager.PlayOneShot(FMODEventPath.VO_Adriel_Losing_a_battle);
                            break;
                        case "chapter3":
                            SoundManager.PlayOneShot(FMODEventPath.VO_Inge_Losing_a_battle);
                            break;
                    }
                    break;
            }

            await Task.CompletedTask;
        }

        public override void MakeMissclick()
        {
            var missClick = UIManager.MakePopupMissclick<PopupMissclickColored>();
            missClick.missClickEnabled = false;
        }

        private void EditTeamButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakeScreen<TeamEditScreen>().
                SetData(new TeamEditScreenInData
            {
                prevScreenInData = UIManager.prevScreenInData.prevScreenInData,
                ftueStageId = UIManager.prevScreenInData.ftueStageId
            }).RunShowScreenProcess();
        }

        private void MagicGuildButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<MagicGuildScreen>();
        }

        private void OverlordButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<OverlordScreen>();
        }

        private void HaremButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_DefeatPopupHaremButtonClick);

            switch (inputData.ftueStageData?.ftueState)
            {
                case ("battle2", "chapter1"):
                    UIManager.MakeScreen<SexScreen>().
                        SetData(new SexScreenInData
                        {
                            ftueStageId = GameData.ftue.info.chapter1.GetStageByKey("sex2")?.id
                        }).RunShowScreenProcess();
                    break;

                default:
                    UIManager.MakeScreen<HaremScreen>().
                        SetData(new HaremScreenInData
                    {
                        prevScreenInData = UIManager.prevScreenInData.prevScreenInData,
                    }).RunShowScreenProcess();
                    break;
            }
        }

        private void RepeatButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakeScreen<BattleScreen>().
                SetData(new BaseBattleScreenInData
                {
                    ftueStageId = inputData?.ftueStageId,
                    eventStageId = inputData?.eventStageId
                }).RunShowScreenProcess();
        }

        private void MapButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);

            if (inputData.hasFTUEStage)
            {
                UIManager.ShowScreen<MapScreen>();
            }
            else if (inputData.hasEventStage)
            {
                UIManager.ShowScreen<EventMapScreen>();
            }
        }
    }

    public class DefeatPopupInData : BasePopupInData
    {

    }
}