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
    public class SexScreen : BaseFullScreenParent<SexScreenInData>
    {
        private Coroutine autoplayCoroutine;

        private TextMeshProUGUI personageName;
        private TextMeshProUGUI text;
        private GameObject nameBackground;

        private Transform textContainer;
        private Button screenButton;
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

        private SpineScene mainAnimation;
        private SpineScene cutInAnimation;

        private FMODEvent backgroundMusic;
        private FMODEvent mainSound;
        private FMODEvent cutInSound;
        private FMODEvent replicaSound;

        private void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/SexScreen/SexScreen", transform);

            var canvas = screenInst.transform.Find("Canvas");

            textContainer = canvas.Find("TextContainer");            
            nameBackground = canvas.Find("SubstrateName").gameObject;
            personageName = nameBackground.transform.Find("PersonageName").GetComponent<TextMeshProUGUI>();
            text = textContainer.Find("Text").GetComponent<TextMeshProUGUI>();

            screenButton = canvas.Find("ScreenTap").GetComponent<Button>();
            screenButton.onClick.AddListener(ScreenTapButtonClick);

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

        public override async Task BeforeShowMakeAsync()
        {
            switch (inputData.ftueStageData?.lerningKey)
            {
                case (_, _):
                    skipButton.gameObject.SetActive(GameData.ftue.chapter1_battle3.isComplete);
                    break;
            }

            await Task.CompletedTask;
        }

        public override async Task BeforeShowAsync()
        {
            if (dialogData == null)
                return;

            SoundManager.StopBGMusic();

            Initialize();
            ShowCurrentReplica();
            AutoplayButtonCustomize();

            await Task.CompletedTask;
        }

        public override async Task AfterShowAsync()
        {
            await Task.CompletedTask;
        }

        public override async Task BeforeHideAsync()
        {
            SoundManager.StopAll();
            await Task.CompletedTask;
        }
        
        private void LeaveScreen()
        {
            switch (inputData.ftueStageData?.lerningKey)
            {
                case (FTUE.CHAPTER_1, FTUE.SEX_1):
                    UIManager.MakeScreen<DialogScreen>().
                        SetData(new DialogScreenInData
                        {
                            ftueStageId = GameData.ftue.chapter1_dialogue1.id
                        }).DoShow();
                    break;
                case (FTUE.CHAPTER_2, FTUE.SEX_2):
                    UIManager.ShowScreen<CastleScreen>();
                    break;
                default:
                    if (UIManager.currentState.prevState.ScreenTypeIs<BattleScreen>())
                    {
                        UIManager.ToPrevState(UIManager.currentState.prevState.prevScreenState);
                    }
                    else
                    {
                        UIManager.ToPrevScreen();
                    }
                    break;
            }
        }

        public override async Task BeforeShowDataAsync()
        {
            dialogData = inputData.dialogData;

            if (inputData.eventStageId.HasValue)
            {
                await GameData.events.StageStart(inputData.eventStageId.Value);
            }
            else if (inputData.ftueStageId.HasValue)
            {
                await GameData.ftue.StartStage(inputData.ftueStageId.Value);
            }
            else if (inputData.dialogId.HasValue)
            {
                await GameData.dialogs.Start(inputData.dialogId.Value);
            }
        }

        public override async Task BeforeHideDataAsync()
        {
            if (inputData.eventStageId.HasValue)
            {
                await GameData.events.StageEnd(inputData.eventStageId.Value);
            }
            else if (inputData.ftueStageId.HasValue)
            {
                await GameData.ftue.EndStage(inputData.ftueStageId.Value);
            }
            else if (inputData.dialogId.HasValue)
            {
                await GameData.dialogs.End(inputData.dialogId.Value);
            }

            if (dialogData.postAction == AdminBRO.Dialog.PostAction_Seduce)
            {
                await GameData.matriarchs.matriarchSeduce(dialogData.matriarchId);
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

        private void ScreenTapButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_DialogNextButtonClick);
            if (dialogData == null)
            {
                LeaveScreen();
                return;
            }
            
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
            
        }

        private void ShowCurrentReplica()
        {
            var replica = dialogReplicas[currentReplicaId];
            var prevReplica = currentReplicaId > 0 ? dialogReplicas[currentReplicaId - 1] : null;
            
            personageName.text = replica.characterName;
            text.text = replica.message;
            nameBackground.SetActive(replica.characterName != null);

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

                    var animation = GameData.animations.GetById(replica.mainAnimationId);
                    if (animation != null)
                    {
                        mainAnimation = SpineScene.GetInstance(animation, mainAnimPos);
                    }
                }
                mainAnimation?.TimeScale(replica.mainAnimationTimeScale);
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

                    var animation = GameData.animations.GetById(replica.cutInAnimationId);
                    if (animation != null)
                    {
                        cutInAnimation = SpineScene.GetInstance(animation, cutInAnimPos);
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
            //background music
            if (replica.backgroundMusicId.HasValue)
            {
                var backgroundMusicData = GameData.sounds.GetById(replica.backgroundMusicId);
                if (backgroundMusicData.eventPath != backgroundMusic?.path)
                {
                    backgroundMusic?.Stop();
                    backgroundMusic = SoundManager.GetEventInstance(backgroundMusicData.eventPath, backgroundMusicData.soundBankId);
                }
            }

            //main sound
            if (replica.mainSoundId.HasValue)
            {
                var mainSoundData = GameData.sounds.GetById(replica.mainSoundId);
                if (mainSoundData.eventPath != mainSound?.path)
                {
                    mainSound?.Stop();
                    mainSound = SoundManager.GetEventInstance(mainSoundData.eventPath, mainSoundData.soundBankId);
                }
                mainSound?.SetPitch(replica.mainAnimationTimeScale);
            }
            else
            {
                mainSound?.Stop();
                mainSound = null;
            }

            if (replica.cutInSoundId.HasValue)
            {
                var cutInSoundData = GameData.sounds.GetById(replica.cutInSoundId);
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
                var replicaSoundData = GameData.sounds.GetById(replica.replicaSoundId);
                if (replicaSoundData.eventPath != replicaSound?.path)
                {
                    replicaSound?.Stop();
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

    public class SexScreenInData : BaseFullScreenInData
    {

    }
}
