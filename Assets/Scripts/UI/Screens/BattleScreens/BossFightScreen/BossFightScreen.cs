using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
	public class BossFightScreen : BaseBossFightScreen
	{
        private int stageId;

        protected override void Awake()
        {
            base.Awake();
            WannaWin(true);
        }

        public BossFightScreen SetData(int stageId)
        {
            this.stageId = stageId;
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
            await GameData.EventStageStartAsync(stageId);
        }

        public override async Task BeforeHideDataAsync()
        {
            await GameData.EventStageEndAsync(stageId);
        }
    }
}