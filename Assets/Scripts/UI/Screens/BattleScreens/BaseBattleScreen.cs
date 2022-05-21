using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
	public class BaseBattleScreen : BaseFullScreen
	{
		protected Button backButton;
		protected Button skipButton;
		protected BattleManager bm;

		protected virtual void Awake()
		{
			var screenInst = ResourceManager.InstantiateScreenPrefab(
				"Prefabs/UI/Screens/BattleScreens/BaseBattleScreen", transform);

			bm = screenInst.GetComponent<BattleManager>();

			var canvas = screenInst.transform.Find("Canvas");
			backButton = canvas.Find("BackButton").GetComponent<Button>();
			backButton.onClick.AddListener(BackButtonClick);

			skipButton = canvas.Find("SkipButton").GetComponent<Button>();
			skipButton.onClick.AddListener(SkipButtonClick);
			skipButton.gameObject.SetActive(false);
		}

        public override void StartShow()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_BattleScreenShow);
        }

        protected virtual void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<StartingScreen>();
        }

		private void SkipButtonClick()
		{
			SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
			if (bm.wannaWin)
				BattleWin();
			else
				BattleDefeat();
		}

		public virtual void StartBattle()
        {
			backButton.gameObject.SetActive(false);
			skipButton.gameObject.SetActive(true);
		}

		public virtual void BattleWin()
        {
			Debug.Log("Win Battle");
			backButton.gameObject.SetActive(true);
			skipButton.gameObject.SetActive(false);
		}

		public virtual void BattleDefeat()
        {
			Debug.Log("Lose Battle");
			backButton.gameObject.SetActive(true);
			skipButton.gameObject.SetActive(false);
		}

		public virtual void OnBattleEvent(BattleEvent battleEvent)
        {
			
        }

		public virtual void OnBattleNotification(string stageKey, string chapterKey, string notifKey)
		{
			GameData.ftue.info.GetChapterByKey(chapterKey)?.ShowNotifByKey(notifKey);
		}

		public virtual BattleManagerInData GetBattleData()
        {
			return null;
        }
    }

	public class BattleManagerInData
    {
		public List<AdminBRO.Character> myTeam { get; private set; } = new List<AdminBRO.Character>();
		public List<EnemyWave> enemyWaves { get; private set; } = new List<EnemyWave>();
		public string ftueChapterKey { get; private set; }
		public string ftueStageKey { get; private set; }
		public AdminBRO.Battle battleData { get; private set; }
		

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
				return inst;
            }
			return null;
        }

		public static BattleManagerInData InstFromEventStage(AdminBRO.EventStageItem stage)
        {
			return InstFromBattleData(stage?.battleData);
		}

		private static BattleManagerInData InstFromBattleData(AdminBRO.Battle battleData)
        {
			if (battleData == null)
            {
				return null;
            }

			var inst = new BattleManagerInData();

			inst.battleData = battleData;

			//my team
			var overlordCh = GameData.GetCharacterByClass(AdminBRO.Character.Class_Overlord);
			if (overlordCh != null)
            {
				inst.myTeam.Add(overlordCh);
            }

			if (battleData.isTypeBattle)
			{
				foreach (var myCh in GameData.characters)
				{
					if (myCh.teamPosition != AdminBRO.Character.TeamPosition_None)
					{
						inst.myTeam.Add(myCh);
					}
				}
			}

			//enemy teams
			foreach (var phase in battleData.battlePhases)
            {
				var wave = new EnemyWave();
				wave.enemyTeam.AddRange(phase.enemyCharacters);
				inst.enemyWaves.Add(wave);
            }

			return inst;
        }
    }
}