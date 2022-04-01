using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
	public class BossFightScreen : BaseBossFightScreen
	{
        protected override void Awake()
        {
            base.Awake();
            WannaWin(true);
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
            await GameData.EventStageStartAsync(GameGlobalStates.bossFight_EventStageData);
        }

        public override async Task BeforeHideDataAsync()
        {
            await GameData.EventStageEndAsync(GameGlobalStates.bossFight_EventStageData);
        }
    }
}