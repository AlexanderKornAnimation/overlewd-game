using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class VictoryPopup : BasePopup
    {
        protected Button nextButton;
        protected Button repeatButton;

        protected Image reward1;
        protected Image reward2;
        protected Image reward3;

        protected VictoryPopupInData inputData;

        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Popups/VictoryPopup/VictoryPopup", transform);

            var canvas = screenInst.transform.Find("Canvas");

            nextButton = canvas.Find("NextButton").GetComponent<Button>();
            nextButton.onClick.AddListener(NextButtonClick);

            repeatButton = canvas.Find("RepeatButton").GetComponent<Button>();
            repeatButton.onClick.AddListener(RepeatButtonClick);

            reward1 = canvas.Find("Reward1").Find("Resource").GetComponent<Image>();
            reward2 = canvas.Find("Reward2").Find("Resource").GetComponent<Image>();
            reward3 = canvas.Find("Reward3").Find("Resource").GetComponent<Image>();
            
            reward1.sprite = ResourceManager.InstantiateAsset<Sprite>("Common/Images/Crystal");
            reward2.sprite = ResourceManager.InstantiateAsset<Sprite>("Common/Images/Gem");
            reward3.sprite = ResourceManager.InstantiateAsset<Sprite>("Common/Images/Gold");
        }

        public VictoryPopup SetData(VictoryPopupInData data)
        {
            inputData = data;
            return this;
        }

        public override void MakeMissclick()
        {
            var missClick = UIManager.MakePopupMissclick<PopupMissclickColored>();
            missClick.missClickEnabled = false;
        }

        protected virtual void NextButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<EventMapScreen>();
        }

        protected virtual void RepeatButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<BattleScreen>();
        }
    }

    public class VictoryPopupInData
    {
        public AdminBRO.FTUEStageItem ftueStageData;
    }
}