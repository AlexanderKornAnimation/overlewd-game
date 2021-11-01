using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class DialogScreen : BaseScreen
    {
        private Button nextButton;
        private Text personageName;
        private Text text;
        private Transform girlEmotion;
        private Text girlName;

        private Button skipButton;

        private AdminBRO.Dialog dialogData;
        private int currentReplicaId;

        void Start()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/DialogScreen/DialogScreen"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            var canvas = screenRectTransform.Find("Canvas");
            var textContainer = canvas.Find("TextContainer");

            nextButton = textContainer.Find("NextButton").GetComponent<Button>();
            nextButton.onClick.AddListener(NextButtonClick);

            personageName = textContainer.Find("PersonageName").GetComponent<Text>();
            text = textContainer.Find("Text").GetComponent<Text>();
            girlEmotion = textContainer.Find("GirlEmotion");
            girlName = textContainer.Find("GirlName").GetComponent<Text>();

            skipButton = canvas.Find("SkipButton").GetComponent<Button>();
            skipButton.onClick.AddListener(SkipButtonClick);

            dialogData = GameData.GetDialogById(GameGlobalStates.dialog_EventStageData.dialogId.Value);

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
            if (replica.characterName == "Ulvi")
            {
                personageName.gameObject.SetActive(false);
                girlEmotion.gameObject.SetActive(true);
                girlName.gameObject.SetActive(true);
                girlName.text = replica.characterName;
            }
            else
            {
                personageName.gameObject.SetActive(true);
                girlEmotion.gameObject.SetActive(false);
                girlName.gameObject.SetActive(false);
                personageName.text = replica.characterName;
            }
            text.text = replica.message;
        }
    }
}
