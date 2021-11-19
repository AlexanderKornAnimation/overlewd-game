using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class BattleScreen : BaseScreen
    {
        private Button startBattleButton;
        private Button backButton;

        async void Start()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/BattleScreen/BattleScreen"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            var canvas = screenRectTransform.Find("Canvas");

            startBattleButton = canvas.Find("StartBattleButton").GetComponent<Button>();
            startBattleButton.onClick.AddListener(StartBattleButtonClick);

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);

            await GameData.EventStageStartAsync(GameGlobalStates.battle_EventStageData);
        }

        private async void StartBattleButtonClick()
        {
            await GameData.EventStageEndAsync(GameGlobalStates.battle_EventStageData);

            UIManager.ShowPopup<VictoryPopup>();
        }

        private void BackButtonClick()
        {
            UIManager.ShowScreen<EventMapScreen>();
        }

        void Update()
        {

        }
    }
}
