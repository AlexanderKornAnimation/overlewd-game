using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class VictoryPopup : BasePopupParent<VictoryPopupInData>
    {
        private Button nextButton;
        private Button repeatButton;

        private const int rewardsCount = 3;
        private Image[] rewards = new Image[rewardsCount];


        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Popups/VictoryPopup/VictoryPopup", transform);

            var canvas = screenInst.transform.Find("Canvas");
            var grid = canvas.Find("Grid");

            nextButton = canvas.Find("NextButton").GetComponent<Button>();
            nextButton.onClick.AddListener(NextButtonClick);

            repeatButton = canvas.Find("RepeatButton").GetComponent<Button>();
            repeatButton.onClick.AddListener(RepeatButtonClick);

            for (int i = 0; i < rewards.Length; i++)
            {
                rewards[i] = grid.Find($"Reward{i + 1}").GetComponent<Image>();
                rewards[i].gameObject.SetActive(false);
            }
        }

        public override async Task BeforeShowMakeAsync()
        {
            var battleData = inputData?.ftueStageData?.battleData ?? inputData?.eventStageData?.battleData;

            for (int i = 0; i < battleData?.rewards?.Count; i++)
            {
                rewards[i].gameObject.SetActive(true);
                rewards[i].sprite = ResourceManager.LoadSprite(battleData?.rewards[i].icon);
            }

            switch (inputData.ftueStageData?.ftueState)
            {
                case ("battle1", "chapter1"):
                    UITools.DisableButton(repeatButton);
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