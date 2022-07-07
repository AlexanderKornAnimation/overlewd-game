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

        private Image reward1;
        private Image reward2;
        private Image reward3;

        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Popups/VictoryPopup/VictoryPopup", transform);

            var canvas = screenInst.transform.Find("Canvas");

            nextButton = canvas.Find("NextButton").GetComponent<Button>();
            nextButton.onClick.AddListener(NextButtonClick);

            repeatButton = canvas.Find("RepeatButton").GetComponent<Button>();
            repeatButton.onClick.AddListener(RepeatButtonClick);

            reward1 = canvas.Find("Reward1").GetComponent<Image>();
            reward2 = canvas.Find("Reward2").GetComponent<Image>();
            reward3 = canvas.Find("Reward3").GetComponent<Image>();
            
            reward1.sprite = ResourceManager.InstantiateAsset<Sprite>("Common/Images/186x186/CopperWithSubstrate186x186");
            reward2.sprite = ResourceManager.InstantiateAsset<Sprite>("Common/Images/186x186/GemsWithSubstrate186x186");
            reward3.sprite = ResourceManager.InstantiateAsset<Sprite>("Common/Images/186x186/GoldWithSubstrate186x186");
        }

        public override async Task BeforeShowMakeAsync()
        {
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