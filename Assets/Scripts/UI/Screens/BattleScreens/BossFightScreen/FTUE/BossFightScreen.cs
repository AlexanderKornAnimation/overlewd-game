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
                    SetData(new VictoryPopupInData
                    {
                        ftueStageId = inputData.ftueStageId
                    }).RunShowPopupProcess();
            }

            public override void BattleDefeat()
            {
                UIManager.MakePopup<DefeatPopup>().
                    SetData(new DefeatPopupInData
                    {
                        ftueStageId = inputData.ftueStageId
                    }).RunShowPopupProcess();
            }

            public override async Task BeforeShowDataAsync()
            {
                await GameData.FTUEStartStage(inputData.ftueStageId.Value);
            }

            public override async Task BeforeHideDataAsync()
            {
                await GameData.FTUEEndStage(inputData.ftueStageId.Value);
            }

            public override void OnBattleEvent(BattleEvent battleEvent)
            {

            }

            public override string GetFTUEChapterKey()
            {
                return GameGlobalStates.ftueChapterData?.key;
            }

            public override string GetFTUEStageKey()
            {
                return inputData?.ftueStageData?.key;
            }

            public override BattleManagerInData GetBattleData()
            {
                return BattleManagerInData.InstFromBattleData(inputData?.ftueStageData?.battleData);
            }
        }
	}
}