using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class SexScreen : BaseFullScreen
    {
        private Coroutine autoplayCoroutine;

        private TextMeshProUGUI personageName;
        private TextMeshProUGUI text;

        private Button textContainer;
        private Button skipButton;
        private Button autoplayButton;
        private TextMeshProUGUI autoplayStatus;
        private Image autoplayButtonPressed;

        private Transform mainAnimPos;
        private GameObject cutIn;
        private Transform cutInAnimPos;

        private AdminBRO.Dialog dialogData;
        private List<AdminBRO.DialogReplica> dialogReplicas;
        private int currentReplicaId;

        private bool isAutoplayButtonPressed = false;

        private SpineWidgetGroup mainAnimation;
        private SpineWidgetGroup cutInAnimation;
        private FMODEvent mainSound;
        private FMODEvent cutInSound;
        private FMODEvent replicaSound;

        private SexScreenInData inputData;

        private void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/SexScreen/SexScreen", transform);

            var canvas = screenInst.transform.Find("Canvas");

            textContainer = canvas.Find("TextContainer").GetComponent<Button>();
            textContainer.onClick.AddListener(TextContainerButtonClick);

            personageName = canvas.Find("SubstrateName").Find("PersonageName").GetComponent<TextMeshProUGUI>();
            text = textContainer.transform.Find("Text").GetComponent<TextMeshProUGUI>();

            skipButton = canvas.Find("SkipButton").GetComponent<Button>();
            skipButton.onClick.AddListener(SkipButtonClick);

            autoplayButton = canvas.Find("AutoplayButton").GetComponent<Button>();
            autoplayStatus = canvas.Find("AutoplayButton").Find("Status").GetComponent<TextMeshProUGUI>();
            autoplayButtonPressed = canvas.Find("AutoplayButton").Find("ButtonPressed").GetComponent<Image>();
            autoplayButton.onClick.AddListener(AutoplayButtonClick);

            mainAnimPos = canvas.Find("MainAnimPos");
            cutIn = canvas.Find("CutIn").gameObject;
            cutInAnimPos = cutIn.transform.Find("AnimPos");
            cutIn.SetActive(false);
        }

        public SexScreen SetData(SexScreenInData data)
        {
            inputData = data;
            return this;
        }

        public override async Task BeforeShowAsync()
        {
            Initialize();
            ShowCurrentReplica();
            AutoplayButtonCustomize();

            await Task.CompletedTask;
        }

        public override async Task AfterShowAsync()
        {
            ShowStartNotifications();
            await Task.CompletedTask;
        }

        public override async Task BeforeHideAsync()
        {
            SoundManager.StopAll();
            await Task.CompletedTask;
        }
        
        private void LeaveScreen()
        {
            switch (inputData.ftueStageData.ftueState)
            {
                case ("sex1", "chapter1"):
                    UIManager.MakeScreen<DialogScreen>().
                        SetData(new DialogScreenInData
                        {
                            ftueStageId = GameData.ftue.mapChapter.GetStageByKey("dialogue1")?.id
                        }).RunShowScreenProcess();
                    break;

                default:
                    if (inputData.ftueStageId.HasValue)
                    {
                        UIManager.ShowScreen<MapScreen>();
                    }
                    else
                    {
                        UIManager.ShowScreen<EventMapScreen>();
                    }
                    break;
            }
        }

        public override async Task BeforeShowDataAsync()
        {
            dialogData = inputData.eventStageData?.dialogData ?? inputData.ftueStageData?.dialogData;
            
            if (inputData.eventStageId.HasValue)
            {
                await GameData.EventStageStartAsync(inputData.eventStageId.Value);
            }
            else
            {
                await GameData.FTUEStartStage(inputData.ftueStageId.Value);
            }
        }

        public override async Task BeforeHideDataAsync()
        {
            if (inputData.eventStageId.HasValue)
            {
                await GameData.EventStageStartAsync(inputData.eventStageId.Value);
            }
            else
            {
                await GameData.FTUEStartStage(inputData.ftueStageId.Value);
            }
        }

        private async void ShowStartNotifications()
        {
            switch (inputData.ftueStageData.ftueState)
            {
                case ("sex4", "chapter1"):
                    GameData.ftue.mapChapter.ShowNotifByKey("memorytutor2");
                    break;
            }

            await Task.CompletedTask;
        }
        
        private async void ShowEndNotifictaions()
        {
            switch (inputData.ftueStageData.ftueState)
            {
                case ("sex2", "chapter1"):
                    UIManager.AddUserInputLocker(new UserInputLocker(this));
                    await UniTask.Delay(1000);
                    UIManager.RemoveUserInputLocker(new UserInputLocker(this));

                    GameData.ftue.mapChapter.ShowNotifByKey("bufftutor2");
                    await UIManager.WaitHideNotifications();
                    GameData.ftue.mapChapter.ShowNotifByKey("ulviscreentutor");
                    break;
            }
        }
        
        private void AutoplayButtonCustomize()
        {
            if (isAutoplayButtonPressed)
            {
                isAutoplayButtonPressed = true;
                autoplayButtonPressed.enabled = true;
                autoplayStatus.text = "ON";
            }
            else
            {
                isAutoplayButtonPressed = false;
                autoplayButtonPressed.enabled = false;
                autoplayStatus.text = "OFF";
            }
        }

        private void AutoplayButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
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
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            LeaveScreen();
        }

        private void TextContainerButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_DialogNextButtonClick);
            currentReplicaId++;
            if (currentReplicaId < dialogReplicas.Count)
            {
                ShowCurrentReplica();
            }
            else
            {
                LeaveScreen();
            }
        }

        private void ShowLastReplica()
        {
            ShowEndNotifictaions();
        }

        private void ShowCurrentReplica()
        {
            var replica = dialogReplicas[currentReplicaId];
            var prevReplica = currentReplicaId > 0 ? dialogReplicas[currentReplicaId - 1] : null;
            
            personageName.text = replica.characterName;
            text.text = replica.message;

            ShowMain(replica, prevReplica);
            ShowCutIn(replica, prevReplica);
            PlaySound(replica);

            if (!(currentReplicaId + 1 < dialogReplicas.Count))
            {
                ShowLastReplica();
            }
        }
        
        private IEnumerator Autoplay()
        {
            while (currentReplicaId < dialogReplicas.Count)
            {
                ShowCurrentReplica();
                yield return new WaitForSeconds(2f);
                currentReplicaId++;
            }
            AutoplayButtonClick();
        }

        private void Initialize()
        {
            dialogReplicas = dialogData.replicas.OrderBy(r => r.sort).ToList();
            if (!dialogReplicas.Any())
            {
                dialogReplicas.Add(
                    new AdminBRO.DialogReplica
                    {
                        message = "EMPTY DIALOG"
                    });
            }
        }

        private void ShowMain(AdminBRO.DialogReplica replica, AdminBRO.DialogReplica prevReplica)
        {
            if (replica.mainAnimationId.HasValue)
            {
                if (replica.mainAnimationId.Value != mainAnimation?.animationData.id)
                {
                    Destroy(mainAnimation?.gameObject);
                    mainAnimation = null;

                    var animation = GameData.GetAnimationById(replica.mainAnimationId.Value);
                    if (animation != null)
                    {
                        mainAnimation = SpineWidgetGroup.GetInstance(mainAnimPos);
                        mainAnimation.Initialize(animation);
                    }
                }
            }
            else
            {
                Destroy(mainAnimation?.gameObject);
                mainAnimation = null;
            }
        }

        private void ShowCutIn(AdminBRO.DialogReplica replica, AdminBRO.DialogReplica prevReplica)
        {
            if (replica.cutInAnimationId.HasValue)
            {
                if (replica.cutInAnimationId != cutInAnimation?.animationData.id)
                {
                    Destroy(cutInAnimation?.gameObject);
                    cutInAnimation = null;

                    var animation = GameData.GetAnimationById(replica.cutInAnimationId.Value);
                    if (animation != null)
                    {
                        cutInAnimation = SpineWidgetGroup.GetInstance(cutInAnimPos);
                        cutInAnimation.Initialize(animation);
                    }
                }
            }
            else
            {
                Destroy(cutInAnimation?.gameObject);
                cutInAnimation = null;
            }

            if (cutInAnimation != null)
            {
                cutIn.SetActive(true);
                mainAnimation?.Pause();
            }
            else
            {
                cutIn.SetActive(false);
                mainAnimation?.Play();
            }
        }

        private void PlaySound(AdminBRO.DialogReplica replica)
        {
            //main sound
            if (replica.mainSoundId.HasValue)
            {
                var mainSoundData = GameData.GetSoundById(replica.mainSoundId.Value);
                if (mainSoundData.eventPath != mainSound?.path)
                {
                    mainSound?.Stop();
                    mainSound = SoundManager.GetEventInstance(mainSoundData.eventPath, mainSoundData.soundBankId);
                }
            }
            else
            {
                mainSound?.Stop();
                mainSound = null;
            }

            if (replica.cutInSoundId.HasValue)
            {
                var cutInSoundData = GameData.GetSoundById(replica.cutInSoundId.Value);
                if (cutInSoundData.eventPath != cutInSound?.path)
                {
                    cutInSound?.Stop();
                    cutInSound = SoundManager.GetEventInstance(cutInSoundData.eventPath, cutInSoundData.soundBankId);
                }

                mainSound?.Pause();
            }
            else
            {
                cutInSound?.Stop();
                cutInSound = null;

                mainSound?.Play();
            }
            
            //replica sound
            if (replica.replicaSoundId.HasValue)
            {
                var replicaSoundData = GameData.GetSoundById(replica.replicaSoundId.Value);
                if (replicaSoundData.eventPath != replicaSound?.path)
                {
                    replicaSound = SoundManager.GetEventInstance(replicaSoundData.eventPath, replicaSoundData.soundBankId);
                }
            }
            else
            {
                replicaSound?.Stop();
                replicaSound = null;
            }
        }
    }

    public class SexScreenInData : BaseScreenInData
    {
        
    }
}
