using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
	public class TempBattleScreen : BaseFullScreen
	{
		private Button backButton;
		private Button skipButton;
		private BattleManager bm;

		private void Awake()
		{
			var screenInst = ResourceManager.InstantiateScreenPrefab(
				"Prefabs/UI/Screens/BattleScreens/TempBattleScreen/TempBattleScreen", transform);

			bm = screenInst.GetComponent<BattleManager>();

			var canvas = screenInst.transform.Find("Canvas");
			backButton = canvas.Find("BackButton").GetComponent<Button>();
			backButton.onClick.AddListener(BackButtonClick);

			skipButton = canvas.Find("SkipButton").GetComponent<Button>();
			skipButton.onClick.AddListener(SkipButtonClick);
			skipButton.gameObject.SetActive(false);

			WannaWin(true);
		}

		public override async Task BeforeShowAsync()
		{
			await Task.CompletedTask;
		}

        public override void StartShow()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_BattleScreenShow);
        }

        private void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<StartingScreen>();
        }
		protected void SkipButtonClick()
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
		public void WannaWin(bool win)
        {
			bm.wannaWin = win;
        }

		public void BattleWin()
        {
			Debug.Log("Win Battle");
        }
		public void BattleDefeat()
        {
			Debug.Log("Lose Battle");
		}
    }
}