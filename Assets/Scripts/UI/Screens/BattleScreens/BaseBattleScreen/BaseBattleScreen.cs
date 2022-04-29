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
    }
}