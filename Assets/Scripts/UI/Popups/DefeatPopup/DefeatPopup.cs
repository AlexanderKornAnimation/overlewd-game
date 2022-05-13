using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class DefeatPopup : BasePopup
    {
        private Button magicGuildButton;
        private Button inventoryButton;
        private Button haremButton;
        private Button editTeamButton;

        private DefeatPopupInData inputData;

        void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Popups/DefeatPopup/DefeatPopup", transform);

            var canvas = screenInst.transform.Find("Canvas");

            magicGuildButton = canvas.Find("MagicGuildButton").GetComponent<Button>();
            magicGuildButton.onClick.AddListener(MagicGuildButtonClick);

            inventoryButton = canvas.Find("InventoryButton").GetComponent<Button>();
            inventoryButton.onClick.AddListener(InventoryButtonClick);

            haremButton = canvas.Find("HaremButton").GetComponent<Button>();
            haremButton.onClick.AddListener(HaremButtonClick);

            editTeamButton = canvas.Find("EditTeamButton").GetComponent<Button>();
            editTeamButton.onClick.AddListener(EditTeamButtonClick);
        }

        public override async Task BeforeShowMakeAsync()
        {
            switch (inputData.ftueStageData?.ftueState)
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

        public DefeatPopup SetData(DefeatPopupInData data)
        {
            inputData = data;
            return this;
        }

        public override void MakeMissclick()
        {
            var missClick = UIManager.MakePopupMissclick<PopupMissclickColored>();
            missClick.missClickEnabled = false;
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

        private void InventoryButtonClick()
        {
            // SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            // UIManager.ShowScreen<InventoryAndUserScreen>();
        }

        private void HaremButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_DefeatPopupHaremButtonClick);

            if (inputData.ftueStageId.HasValue)
            {
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
            else
            {
                UIManager.ShowScreen<EventMapScreen>();
            }
        }
    }

    public class DefeatPopupInData : BaseScreenInData
    {

    }
}