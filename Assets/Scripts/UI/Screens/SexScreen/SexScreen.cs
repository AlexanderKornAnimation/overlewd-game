using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class SexScreen : BaseScreen
    {
        private Button nextButton;
        private Text personageName;
        private Text text;

        private Button skipButton;

        private AdminBRO.Dialog dialogData;
        private int currentReplicaId;

        void Start()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/SexScreen/SexScreen"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            var canvas = screenRectTransform.Find("Canvas");
            var textContainer = canvas.Find("TextContainer");

            nextButton = canvas.Find("Next").GetComponent<Button>();
            nextButton.onClick.AddListener(NextButtonClick);

            personageName = textContainer.Find("Name").GetComponent<Text>();
            text = textContainer.Find("Text").GetComponent<Text>();

            skipButton = canvas.Find("SkipButton").GetComponent<Button>();
            skipButton.onClick.AddListener(SkipButtonClick);

            dialogData = GameData.GetDialogById(GameGlobalStates.sex_EventStageData.dialog.id);

            ShowCurrentReplica();
        }

        void Update()
        {

        }

        private void SkipButtonClick()
        {
            UIManager.ShowScreen<EventMapScreen>();
        }

        private void NextButtonClick()
        {
            currentReplicaId++;
            if (currentReplicaId < dialogData.replicas.Count)
            {
                ShowCurrentReplica();
            }
            else
            {
                UIManager.ShowScreen<EventMapScreen>();
            }
        }

        private void ShowCurrentReplica()
        {
            var replica = dialogData.replicas[currentReplicaId];
            personageName.text = replica.characterName;
            text.text = replica.message;
        }
    }

}
