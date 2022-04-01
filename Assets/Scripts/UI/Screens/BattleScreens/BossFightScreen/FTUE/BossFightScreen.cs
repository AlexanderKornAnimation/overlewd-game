using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
	namespace FTUE
	{
		public class BossFightScreen : Overlewd.BossFightScreen
		{
            protected AdminBRO.FTUEStageItem stageData;
            public BossFightScreen SetStageData(AdminBRO.FTUEStageItem data)
            {
                stageData = data;
                return this;
            }

            protected override void Awake()
            {
                base.Awake();
                WannaWin(true);
            }

            protected override void BackButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                UIManager.ShowScreen<MapScreen>();
            }

            public override void BattleWin()
            {
                UIManager.MakePopup<VictoryPopup>().
                    SetStageData(stageData).RunShowPopupProcess();
            }

            public override void BattleDefeat()
            {
                UIManager.MakePopup<DefeatPopup>().
                    SetStageData(stageData).RunShowPopupProcess();
            }

            public override async Task BeforeShowDataAsync()
            {
                await GameData.FTUEStartStage(stageData.id);
            }

            public override async Task BeforeHideDataAsync()
            {
                await GameData.FTUEEndStage(stageData.id);
            }
        }
	}
}