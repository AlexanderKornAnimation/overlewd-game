using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
	public class BaseBattleScreen : BaseFullScreenParent<BaseBattleScreenInData>
	{
		protected Button skipButton;
		protected BattleManager bm;
		protected BattleManagerOutData endBattleData;

		void Awake()
		{
			var screenInst = ResourceManager.InstantiateScreenPrefab(
				"Prefabs/UI/Screens/BattleScreens/BaseBattleScreen", transform);

			bm = screenInst.GetComponent<BattleManager>();

			var canvas = screenInst.transform.Find("Canvas");

			skipButton = canvas.Find("SkipButton").GetComponent<Button>();
			skipButton.onClick.AddListener(SkipButtonClick);
#if UNITY_EDITOR
			skipButton.gameObject.SetActive(true);
#else
			skipButton.gameObject.SetActive(false);
#endif
		}

		public override void StartShow()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_BattleScreenShow);
        }

		private void SkipButtonClick()
		{
			SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
			EndBattle(new BattleManagerOutData
			{
				battleWin = true
			});
		}

		public virtual void StartBattle()
        {
			skipButton.gameObject.SetActive(false);
		}

		public virtual void EndBattle(BattleManagerOutData data)
        {
			endBattleData = data;
			skipButton.gameObject.SetActive(false);
		}

		public virtual void OnBattleEvent(BattleEvent battleEvent)
        {
			
        }

		public virtual void OnBattleNotification(string stageKey, string chapterKey, string notifKey)
		{
			GameData.ftue.GetChapterByKey(chapterKey)?.ShowNotifByKey(notifKey, false);
		}

		public BattleManagerInData GetBattleData()
		{
			return inputData.hasFTUEStage ? BattleManagerInData.InstFromFTUEStage(inputData.ftueStageData) :
				inputData.hasEventStage ? BattleManagerInData.InstFromEventStage(inputData.eventStageData) : null;
		}
	}

	public class BasePrepareBattlePopupInData : BasePopupInData
    {
		public int energyCost => ftueStageData?.ftueChapterData?.battleEnergyPointsCost ??
			eventStageData?.eventChapterData?.battleEnergyPointsCost ?? 0;
		public int replayCost => 1;
    }

	public class BaseBattleScreenInData : BaseFullScreenInData
	{

	}

	public class BattleManagerInData
    {
		public enum BattleFlow
        {
			Normal,
			Win,
			Defeat
        }

		public List<AdminBRO.Character> myTeam { get; private set; } = new List<AdminBRO.Character>();
		public List<EnemyWave> enemyWaves { get; private set; } = new List<EnemyWave>();
		public string ftueChapterKey { get; private set; }
		public string ftueStageKey { get; private set; }
		public AdminBRO.Battle battleData { get; private set; }
		public int mana { get; private set; }
		public int hp { get; private set; }
		public float manaMagnitude { get; private set; }
		public float hpMagnitude { get; private set; }
		public BattleFlow battleFlow { get; private set; } = BattleFlow.Normal;

		public class EnemyWave
        {
			public List<AdminBRO.Character> enemyTeam { get; set; } = new List<AdminBRO.Character>();
		}

		public static BattleManagerInData InstFromFTUEStage(AdminBRO.FTUEStageItem stage)
        {
			var inst = InstFromBattleData(stage?.battleData);
			if (inst != null)
            {
				inst.ftueChapterKey = stage.ftueChapterData?.key;
				inst.ftueStageKey = stage?.key;

				switch (stage?.lerningKey)
				{
					case (FTUE.CHAPTER_1, FTUE.BATTLE_2):
						inst.battleFlow = GameData.ftue.chapter1_sex2.isComplete ?
							BattleFlow.Win : BattleFlow.Defeat;
						break;
				}

				return inst;
            }
			return null;
        }

		public static BattleManagerInData InstFromEventStage(AdminBRO.EventStageItem stage)
        {
			return InstFromBattleData(stage?.battleData);
		}

		public static BattleManagerInData InstFromBattleData(AdminBRO.Battle battleData)
        {
			if (battleData == null)
            {
				return null;
            }

			var inst = new BattleManagerInData();

			inst.battleData = battleData;

			//my team
			var overlordCh = GameData.characters.overlord;
			if (overlordCh != null)
            {
				inst.myTeam.Add(overlordCh);
            }

			inst.myTeam.AddRange(GameData.characters.myTeamCharacters);

			//enemy teams
			foreach (var phase in battleData.battlePhases)
            {
				var wave = new EnemyWave();
				wave.enemyTeam.AddRange(phase.enemyCharacters);
				inst.enemyWaves.Add(wave);
            }

			//poison
			inst.mana = GameData.player.info.potion.mana;
			inst.hp = GameData.player.info.potion.hp;
			inst.manaMagnitude = GameData.potions.manaInfo.magnitude;
			inst.hpMagnitude = GameData.potions.hpInfo.magnitude;

			return inst;
        }
    }

	public class BattleManagerOutData
    {
		public bool battleWin { get; set; } = false;
		public int manaSpent { get; set; } = 0;
		public int hpSpent { get; set; } = 0;
    }
}