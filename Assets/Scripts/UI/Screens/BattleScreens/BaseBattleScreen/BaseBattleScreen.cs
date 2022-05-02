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
				"Prefabs/UI/Screens/BattleScreens/BaseBattleScreen/BaseBattleScreen", transform);

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

		public void StartBattle()
        {
			backButton.gameObject.SetActive(false);
			skipButton.gameObject.SetActive(true);
		}

		protected void WannaWin(bool win)
        {
			bm.wannaWin = win;
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

		public virtual string GetFTUEChapterKey()
        {
			return "";
        }

		public virtual string GetFTUEStageKey()
        {
			return "";
        }

		public virtual BattleManagerInData GetBattleData()
        {
			return null;
        }
    }

	public class BattleManagerInData
    {
		public List<AdminBRO.Character> myTeam { get; private set; }
		public List<EnemyWave> enemyWaves { get; private set; }

		public class EnemyWave
        {
			public List<AdminBRO.Character> enemyTeam { get; set; }
		}

		public static BattleManagerInData InstFromBattleData(AdminBRO.Battle battleData)
        {
			if (battleData == null)
            {
				return null;
            }

			var inst = new BattleManagerInData();

			//my team
			var overlordCh = GameData.GetCharacterByClass(AdminBRO.Character.Class_Overlord);
			if (overlordCh != null)
            {
				inst.myTeam.Add(overlordCh);
            }

			foreach (var myCh in GameData.characters)
            {
				if (myCh.teamPosition != AdminBRO.Character.TeamPosition_None)
                {
					inst.myTeam.Add(myCh);
                }
            }

			//enemy teams
			foreach (var phase in battleData.battlePhases)
            {
				var wave = new EnemyWave { enemyTeam = new List<AdminBRO.Character>(phase.enemyCharacters) };
				inst.enemyWaves.Add(wave);
            }

			return inst;
        }
    }
}