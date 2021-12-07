using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class SexScreen : BaseScreen
    {
        protected Coroutine autoplayCoroutine;

        protected Button nextButton;
        protected Text personageName;
        protected Text text;

        protected Button skipButton;
        protected Button autoplayButton;
        protected Text autoplayStatus;
        protected Image autoplayButtonPressed;

        protected Transform mainAnimPos;
        protected GameObject cutIn;
        protected Transform cutInAnimPos;

        protected AdminBRO.Dialog dialogData;
        protected int currentReplicaId;

        protected bool isAutoplayButtonPressed = false;

        void Awake()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/SexScreen/SexScreen"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            var canvas = screenRectTransform.Find("Canvas");
            var textContainer = canvas.Find("TextContainer");

            nextButton = textContainer.Find("NextButton").GetComponent<Button>();
            nextButton.onClick.AddListener(NextButtonClick);

            personageName = textContainer.Find("PersonageName").GetComponent<Text>();
            text = textContainer.Find("Text").GetComponent<Text>();

            skipButton = canvas.Find("SkipButton").GetComponent<Button>();
            skipButton.onClick.AddListener(SkipButtonClick);

            autoplayButton = canvas.Find("AutoplayButton").GetComponent<Button>();
            autoplayStatus = canvas.Find("AutoplayButton").Find("Status").GetComponent<Text>();
            autoplayButtonPressed = canvas.Find("AutoplayButton").Find("ButtonPressed").GetComponent<Image>();
            autoplayButton.onClick.AddListener(AutoplayButtonClick);

            mainAnimPos = canvas.Find("MainAnimPos");
            cutIn = canvas.Find("CutIn").gameObject;
            cutInAnimPos = cutIn.transform.Find("AnimPos");
            cutIn.SetActive(false);
        }

        void Start()
        {
            
        }

        protected override async Task PrepareShowOperationsAsync()
        {
            await EnterScreen();

            ShowCurrentReplica();
            AutoplayButtonCustomize();
        }

        protected override async Task PrepareHideOperationsAsync()
        {
            await Task.CompletedTask;
        }

        protected virtual async Task EnterScreen()
        {
            dialogData = GameData.GetDialogById(GameGlobalStates.sex_EventStageData.dialogId.Value);
            await GameData.EventStageStartAsync(GameGlobalStates.sex_EventStageData);
        }

        protected virtual async void LeaveScreen()
        {
            await GameData.EventStageEndAsync(GameGlobalStates.sex_EventStageData);

            UIManager.ShowScreen<EventMapScreen>();
        }

        protected void AutoplayButtonCustomize()
        {
            var defaultColor = Color.black;
            var redColor = Color.HSVToRGB(0.9989f, 1.00000f, 0.6118f);

            if (isAutoplayButtonPressed)
            {
                autoplayButtonPressed.enabled = true;
                autoplayStatus.color = redColor;
                autoplayStatus.text = "ON";
            }
            else
            {
                autoplayButtonPressed.enabled = false;
                autoplayStatus.color = defaultColor;
                autoplayStatus.text = "OFF";
            }
        }

        private void AutoplayButtonClick()
        {
            if (isAutoplayButtonPressed == false)
            {
                isAutoplayButtonPressed = true;
                autoplayCoroutine = StartCoroutine(Autoplay());
            }
            else
            {
                isAutoplayButtonPressed = false;
                if (autoplayCoroutine != null)
                {
                    StopCoroutine(autoplayCoroutine);
                }
            }

            AutoplayButtonCustomize();
        }
        
        private void SkipButtonClick()
        {
            LeaveScreen();
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
                LeaveScreen();
            }
        }

        protected virtual void ShowCurrentReplica()
        {
            var replica = dialogData.replicas[currentReplicaId];
            personageName.text = replica.characterName;
            text.text = replica.message;
        }
        
        private IEnumerator Autoplay()
        {
            while (currentReplicaId < dialogData.replicas.Count)
            {
                ShowCurrentReplica();
                yield return new WaitForSeconds(1f);
                currentReplicaId++;
            }
            AutoplayButtonClick();
        }
    }
}
