using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
	public class TempBattleScreen : BaseScreen
	{
		private Button backButton;
		//private GameObject battleManager;

		private void Awake()
		{
			var screenInst = ResourceManager.InstantiateScreenPrefab(
				"Prefabs/UI/Screens/BattleScreens/TempBattleScreen/TempBattleScreen", transform);

			var canvas = screenInst.transform.Find("Canvas");
			backButton = canvas.Find("BackButton").GetComponent<Button>();
			backButton.onClick.AddListener(BackButtonClick);
		}

		void Start()
		{
			/*battleManager = Resources.Load("Prefabs/Battle/BattleManager") as GameObject;
			if (battleManager != null)
				Instantiate(battleManager, transform);
			else
				Debug.LogError("BattleManager Loading Error");*/
		}

		public override async Task BeforeShowAsync()
		{
			await Task.CompletedTask;
		}

        public override void StartShow()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_BattleScreenShow);
        }

		public override void AfterShow()
		{

		}

        private void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<StartingScreen>();
        }
    }
}