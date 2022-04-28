using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
	public class BossFightScreen : BaseBossFightScreen
	{
        protected BossFightScreenInData inputData;

        protected override void Awake()
        {
            base.Awake();
            WannaWin(true);
        }

        public BossFightScreen SetData(BossFightScreenInData data)
        {
            inputData = data;
            return this;
        }

        protected override void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<EventMapScreen>();
        }

        public override void BattleWin()
        {
            UIManager.ShowPopup<VictoryPopup>();
        }

        public override void BattleDefeat()
        {
            UIManager.ShowPopup<DefeatPopup>();
        }

        public override async Task BeforeShowDataAsync()
        {
            await GameData.EventStageStartAsync(inputData.eventStageId.Value);
        }

        public override async Task BeforeHideDataAsync()
        {
            await GameData.EventStageEndAsync(inputData.eventStageId.Value);
        }
    }

    public class BossFightScreenInData : BaseScreenInData
    {
        
    }
}