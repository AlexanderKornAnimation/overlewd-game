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
            switch (inputData.ftueStageData?.lerningKey)
            {
                case (FTUE.CHAPTER_1, FTUE.BATTLE_2):
                    UITools.DisableButton(editTeamButton);
                    UITools.DisableButton(magicGuildButton);
                    UITools.DisableButton(overlordButton);
                    UITools.DisableButton(repeatButton);
                    UITools.DisableButton(mapButton);
                    break;
                case (FTUE.CHAPTER_2, FTUE.BATTLE_4):
                    UITools.DisableButton(magicGuildButton);
                    UITools.DisableButton(overlordButton);
                    UITools.DisableButton(haremButton);
                    UITools.DisableButton(repeatButton);
                    UITools.DisableButton(mapButton);
                    break;
                case (FTUE.CHAPTER_1, _):
                    UITools.DisableButton(editTeamButton);
                    UITools.DisableButton(magicGuildButton);
                    UITools.DisableButton(overlordButton);
                    UITools.DisableButton(haremButton, !GameData.buildings.harem.meta.isBuilt);
                    break;
                default:
                    UITools.DisableButton(haremButton, !GameData.buildings.harem.meta.isBuilt);
                    UITools.DisableButton(magicGuildButton, !GameData.buildings.magicGuild.meta.isBuilt);
                    break;
            }

            await Task.CompletedTask;
        }

        public override async Task AfterShowAsync()
        {
            switch (inputData.ftueStageData?.lerningKey)
            {
                case (FTUE.CHAPTER_1, FTUE.BATTLE_2):
                    SoundManager.PlayOneShot(FMODEventPath.VO_Ulvi_Losing_a_battle);
                    GameData.ftue.mapChapter.ShowNotifByKey("bufftutor1", false);
                    break;
                case (FTUE.CHAPTER_1, _):
                    SoundManager.PlayOneShot(FMODEventPath.VO_Ulvi_Losing_a_battle);
                    break;
                case (FTUE.CHAPTER_2, _):
                    SoundManager.PlayOneShot(FMODEventPath.VO_Adriel_Losing_a_battle);
                    break;
                case (FTUE.CHAPTER_3, _):
                    SoundManager.PlayOneShot(FMODEventPath.VO_Inge_Losing_a_battle);
                    break;
            }

            await Task.CompletedTask;
        }

        public override void OnMissclick()
        {

        }

        private void EditTeamButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<TeamEditScreen>();
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

            switch (inputData.ftueStageData?.lerningKey)
            {
                case (FTUE.CHAPTER_1, FTUE.BATTLE_2):
                    UIManager.MakeScreen<SexScreen>().
                        SetData(new SexScreenInData
                        {
                            ftueStageId = GameData.ftue.chapter1_sex2?.id
                        }).DoShow();
                    break;
                default:
                    UIManager.ShowScreen<HaremScreen>();
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