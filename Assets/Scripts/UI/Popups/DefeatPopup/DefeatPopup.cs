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
            GameData.ftue.DoLern(
                inputData.ftueStageData,
                new FTUELernActions
                {
                    ch1_b2 = () =>
                    {
                        UITools.DisableButton(editTeamButton);
                        UITools.DisableButton(magicGuildButton);
                        UITools.DisableButton(overlordButton);
                        UITools.DisableButton(repeatButton);
                        UITools.DisableButton(mapButton);
                    },
                    ch1_any = () => 
                    {
                        UITools.DisableButton(editTeamButton);
                        UITools.DisableButton(magicGuildButton);
                        UITools.DisableButton(overlordButton);
                    }
                });

            await Task.CompletedTask;
        }

        public override async Task AfterShowAsync()
        {
            GameData.ftue.DoLern(
                inputData.ftueStageData,
                new FTUELernActions
                {
                    ch1_b2 = () =>
                    {
                        SoundManager.PlayOneShot(FMODEventPath.VO_Ulvi_Losing_a_battle);
                        GameData.ftue.mapChapter.ShowNotifByKey("bufftutor1", false);
                    },
                    ch1_any = () => SoundManager.PlayOneShot(FMODEventPath.VO_Ulvi_Losing_a_battle),
                    ch2_any = () => SoundManager.PlayOneShot(FMODEventPath.VO_Adriel_Losing_a_battle),
                    ch3_any = () => SoundManager.PlayOneShot(FMODEventPath.VO_Inge_Losing_a_battle)
                });

            await Task.CompletedTask;
        }

        public override BaseMissclick MakeMissclick()
        {
            var missClick = UIManager.MakePopupMissclick<PopupMissclickColored>();
            missClick.missClickEnabled = false;
            return missClick;
        }

        private void EditTeamButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakeScreen<TeamEditScreen>().
                SetData(new TeamEditScreenInData
            {
                prevScreenInData = UIManager.screenInData.prevScreenInData,
                ftueStageId = UIManager.screenInData.ftueStageId
            }).DoShow();
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

            GameData.ftue.DoLern(
                inputData.ftueStageData,
                new FTUELernActions
                {
                    ch1_b2 = () =>
                    {
                        UIManager.MakeScreen<SexScreen>().
                        SetData(new SexScreenInData
                        {
                            ftueStageId = GameData.ftue.chapter1_stages.sex2?.id
                        }).DoShow();
                    },
                    def = () =>
                    {
                        UIManager.MakeScreen<HaremScreen>().
                        SetData(new HaremScreenInData
                        {
                            prevScreenInData = UIManager.screenInData.prevScreenInData,
                        }).DoShow();
                    }
                });
        }

        private void RepeatButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakeScreen<BattleScreen>().
                SetData(new BaseBattleScreenInData
                {
                    ftueStageId = inputData?.ftueStageId,
                    eventStageId = inputData?.eventStageId
                }).DoShow();
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