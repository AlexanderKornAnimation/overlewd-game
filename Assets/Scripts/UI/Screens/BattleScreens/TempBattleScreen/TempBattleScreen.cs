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

		private void Awake()
		{
			var screenInst = ResourceManager.InstantiateScreenPrefab(
				"Prefabs/UI/Screens/BattleScreens/TempBattleScreen/TempBattleScreen", transform);

			var canvas = screenInst.transform.Find("Canvas");
			backButton = canvas.Find("BackButton").GetComponent<Button>();
			backButton.onClick.AddListener(BackButtonClick);

			skipButton = canvas.Find("SkipButton").GetComponent<Button>();
			skipButton.onClick.AddListener(SkipButtonClick);
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
			//skip action
		}

		public void BattleWin()
        {

        }
		public void BattleDefeat()
        {

        }
    }
}