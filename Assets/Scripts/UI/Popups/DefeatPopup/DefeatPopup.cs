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

        void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Popups/DefeatPopup/DefeatPopup", transform);

            var canvas = screenInst.transform.Find("Canvas");
            var grid = canvas.Find("Grid");

            magicGuildButton = grid.Find("MagicGuildButton").GetComponent<Button>();
            magicGuildButton.onClick.AddListener(MagicGuildButtonClick);

            overlordButton = grid.Find("OverlordButton").GetComponent<Button>();
            overlordButton.onClick.AddListener(OverlordButtonClick);

            haremButton = grid.Find("HaremButton").GetComponent<Button>();
            haremButton.onClick.AddListener(HaremButtonClick);

            editTeamButton = grid.Find("EditTeamButton").GetComponent<Button>();
            editTeamButton.onClick.AddListener(EditTeamButtonClick);
        }

        public override async Task BeforeShowMakeAsync()
        {
            switch (inputData.ftueStageData?.ftueState)
            {
                case ("battle2", "chapter1"):
                    UITools.DisableButton(magicGuildButton);
                    UITools.DisableButton(overlordButton);
                    UITools.DisableButton(editTeamButton);
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
                    GameData.ftue.mapChapter.ShowNotifByKey("bufftutor1");
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
            // SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            // UIManager.ShowScreen<InventoryAndUserScreen>();
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
    }

    public class DefeatPopupInData : BasePopupInData
    {

    }
}