using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class DialogScreen : BaseScreen
    {
        private Coroutine autoplayCoroutine;
        
        private Button nextButton;
        private Text personageName;
        private Image personageHead;
        private Text text;

        private Button skipButton;
        private Button autoplayButton;
        private Image autoplayButtonPressed;
        private Text autoplayStatus;

        private AdminBRO.Dialog dialogData;
        private int currentReplicaId;

        private bool isAutoplayButtonPressed = false;

        void Start()
        {
            var screenPrefab = (GameObject) Instantiate(Resources.Load("Prefabs/UI/Screens/DialogScreen/DialogScreen"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            var canvas = screenRectTransform.Find("Canvas");
            var textContainer = canvas.Find("TextContainer");

            nextButton = textContainer.Find("NextButton").GetComponent<Button>();
            nextButton.onClick.AddListener(NextButtonClick);

            personageName = textContainer.Find("PersonageName").GetComponent<Text>();
            personageHead = textContainer.Find("PersonageHead").GetComponent<Image>();
            text = textContainer.Find("Text").GetComponent<Text>();

            skipButton = canvas.Find("SkipButton").GetComponent<Button>();
            skipButton.onClick.AddListener(SkipButtonClick);

            autoplayButton = canvas.Find("AutoplayButton").GetComponent<Button>();
            autoplayButtonPressed = canvas.Find("AutoplayButton").Find("ButtonPressed").GetComponent<Image>();
            autoplayStatus = canvas.Find("AutoplayButton").Find("Status").GetComponent<Text>();
            autoplayButton.onClick.AddListener(AutoplayButtonClick);
            autoplayButtonPressed.enabled = false;
            
            dialogData = GameData.GetDialogById(GameGlobalStates.dialog_EventStageData.dialogId.Value);

            ShowCurrentReplica();
        }

        private void AutoplayButtonClick()
        {
            var defaultColor = Color.black;
            var redColor = Color.HSVToRGB(0.9989f, 1.00000f, 0.6118f);
            
            if (isAutoplayButtonPressed == false)
            {
                isAutoplayButtonPressed = true;
                autoplayButtonPressed.enabled = true;
                autoplayStatus.color = redColor;
                autoplayStatus.text = "ON";
                autoplayCoroutine = StartCoroutine(Autoplay());
            }
            else
            {
                isAutoplayButtonPressed = false;
                autoplayButtonPressed.enabled = false;
                autoplayStatus.color = defaultColor;
                autoplayStatus.text = "OFF";
                
                if (autoplayCoroutine != null)
                {
                    StopCoroutine(autoplayCoroutine);
                }
            }
        }

        private IEnumerator Autoplay()
        {
            for (int i = 0; i < dialogData.replicas.Count; i++)
            {
                currentReplicaId++;
                if (currentReplicaId < dialogData.replicas.Count)
                {
                    ShowCurrentReplica();
                    yield return new WaitForSeconds(1f);
                }
            }
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