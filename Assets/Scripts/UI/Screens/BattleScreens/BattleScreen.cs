using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
	public class BattleScreen : BaseBattleScreen
	{
        public override void StartBattle()
        {
            base.StartBattle();
        }

        public override async void EndBattle(BattleManagerOutData data)
        {
            base.EndBattle(data);

            switch (inputData.ftueStageData?.lerningKey)
            {
                case (FTUE.CHAPTER_1, FTUE.BATTLE_2):
                    endBattleData.battleWin = GameData.ftue.chapter1_sex2.isComplete;
                    break;
            }

            if (endBattleData.battleWin)
            {
                UIManager.MakePopup<VictoryPopup>().
                    SetData(new VictoryPopupInData
                    {
                        ftueStageId = inputData.ftueStageId,
                        eventStageId = inputData.eventStageId
                    }).DoShow();
            }
            else
            {
                UIManager.MakePopup<DefeatPopup>().
                SetData(new DefeatPopupInData
                {
                    ftueStageId = inputData.ftueStageId,
                    eventStageId = inputData.eventStageId
                }).DoShow();
            }
            
            switch (inputData.ftueStageData?.lerningKey)
            {
                case (FTUE.CHAPTER_2, FTUE.BATTLE_2):
                    if (!GameData.ftue.chapter2_battle2.isComplete)
                    {
                        GameData.ftue.chapter2.ShowNotifByKey("ch2teamupgradetutor1");
                        await UIManager.WaitHideNotifications();
                        UIManager.MakeScreen<BattleGirlScreen>().
                            SetData(new BattleGirlScreenInData
                            {
                                characterId = GameData.characters.slot1Ch?.id,
                            }).DoShow();
                    }
                    break;
                case (FTUE.CHAPTER_2, FTUE.BATTLE_4):
                    if (!GameData.ftue.chapter2_battle4.isComplete)
                    {
                        GameData.ftue.chapter2.ShowNotifByKey("ch2teamequiptutor1");
                        await UIManager.WaitHideNotifications();
                        UIManager.MakeScreen<WeaponScreen>().
                            SetData(new WeaponScreenInData
                            {
                                characterId = GameData.characters.slot1Ch.id,
                            }).DoShow();
                    }
                    break;
            }
        }

        public override async Task BeforeShowMakeAsync()
        {
            await Task.CompletedTask;
        }

        public override async Task BeforeShowDataAsync()
        {
            if (inputData.ftueStageId.HasValue)
            {
                await GameData.ftue.StartStage(inputData.ftueStageData.id);
            }
            else
            {
                await GameData.events.StageStart(inputData.eventStageId.Value);
            }
        }

        public override async Task AfterShowAsync()
        {
            bm.AfterShowBattleScreen();

            SoundManager.PlayBGMusic(FMODEventPath.Music_Battle_BGM_1);
            await Task.CompletedTask;
        }

        public override async Task BeforeHideAsync()
        {
            SoundManager.StopAll();
            await Task.CompletedTask;
        }

        public override async Task BeforeHideDataAsync()
        {
            if (inputData.ftueStageId.HasValue)
            {
                await GameData.ftue.EndStage(inputData.ftueStageId.Value,
                    new AdminBRO.BattleEndData
                    {
                        win = endBattleData.battleWin,
                        mana = endBattleData.manaSpent,
                        hp = endBattleData.hpSpent
                    });
            }
            else
            {
                await GameData.events.StageEnd(inputData.eventStageId.Value,
                    new AdminBRO.BattleEndData
                    {
                        win = endBattleData.battleWin,
                        mana = endBattleData.manaSpent,
                        hp = endBattleData.hpSpent
                    });
            }
        }
    }
}