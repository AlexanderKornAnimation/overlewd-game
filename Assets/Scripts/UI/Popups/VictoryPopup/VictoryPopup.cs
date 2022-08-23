using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class VictoryPopup : BasePopupParent<VictoryPopupInData>
    {
        private Transform grid;

        private Button nextButton;
        private Button repeatButton;

        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Popups/VictoryPopup/VictoryPopup", transform);

            var canvas = screenInst.transform.Find("Canvas");
            grid = canvas.Find("Grid");

            nextButton = canvas.Find("NextButton").GetComponent<Button>();
            nextButton.onClick.AddListener(NextButtonClick);

            repeatButton = canvas.Find("RepeatButton").GetComponent<Button>();
            repeatButton.onClick.AddListener(RepeatButtonClick);
        }

        public override async Task BeforeShowMakeAsync()
        {
            var battleData = inputData?.ftueStageData?.battleData ?? inputData?.eventStageData?.battleData;

            int revId = 0;
            foreach (Transform reward in grid)
            {
                if (revId < battleData?.rewards?.Count)
                {
                    var icon = reward.GetComponent<Image>();
                    var count = reward.Find("Count").GetComponent<TextMeshProUGUI>();
                    icon.sprite = ResourceManager.LoadSprite(battleData?.rewards[revId].icon);
                    count.text = battleData?.rewards[revId].amount?.ToString() ?? "_";
                    reward.gameObject.SetActive(true);
                }
                else
                {
                    reward.gameObject.SetActive(false);
                }
                revId++;
            }

            switch (inputData.ftueStageData?.ftueState)
            {
                case ("battle1", "chapter1"):
                    UITools.DisableButton(repeatButton);
                    break;
            }

            await Task.CompletedTask;
        }

        public override async Task AfterShowAsync()
        {
            switch (inputData.ftueStageData?.ftueState)
            {
                case (_, _):
                    switch (GameData.ftue.activeChapter.key)
                    {
                        case "chapter1":
                            SoundManager.PlayOneShot(FMODEventPath.VO_Ulvi_Winning_a_battle);
                            break;
                        case "chapter2":
                            SoundManager.PlayOneShot(FMODEventPath.VO_Adriel_Winning_a_battle);
                            break;
                        case "chapter3":
                            SoundManager.PlayOneShot(FMODEventPath.VO_Inge_Winning_a_battle);
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

        private void NextButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);            
            switch (inputData.ftueStageData?.ftueState)
            {
                case ("battle4", "chapter1"):
                    UIManager.ShowScreen<CastleScreen>();
                    break;

                default:
                    if (inputData.ftueStageId.HasValue)
                    {
                        UIManager.ShowScreen<MapScreen>();
                    }
                    else if (inputData.eventStageId.HasValue)
                    {
                        UIManager.ShowScreen<EventMapScreen>();
                    }
                    break;
            }
        }

        private void RepeatButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakeScreen<BattleScreen>().
                SetData(new BaseBattleScreenInData
                {
                    ftueStageId = inputData.ftueStageId,
                    eventStageId = inputData.eventStageId
                }).RunShowScreenProcess();
        }
    }

    public class VictoryPopupInData : BasePopupInData
    {
        
    }
}