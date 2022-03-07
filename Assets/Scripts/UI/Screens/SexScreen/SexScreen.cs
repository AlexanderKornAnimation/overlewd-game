using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class SexScreen : BaseScreen
    {
        protected Coroutine autoplayCoroutine;

        protected TextMeshProUGUI personageName;
        protected TextMeshProUGUI text;

        protected Button textContainer;
        protected Button skipButton;
        protected Button autoplayButton;
        protected TextMeshProUGUI autoplayStatus;
        protected Image autoplayButtonPressed;

        protected Transform mainAnimPos;
        protected GameObject cutIn;
        protected Transform cutInAnimPos;

        protected AdminBRO.Dialog dialogData;
        protected int currentReplicaId;

        protected bool isAutoplayButtonPressed = false;

        protected SpineWidgetGroup mainAnimation;
        protected SpineWidgetGroup cutInAnimation;
        protected FMODEvent mainSound;
        protected FMODEvent cutInSound;

        void Awake()
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

        public override async Task BeforeShowAsync()
        {
            await EnterScreen();

            ShowCurrentReplica();
            AutoplayButtonCustomize();
        }

        public override async Task BeforeHideAsync()
        {
            SoundManager.StopAll();

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
            if (currentReplicaId < dialogData.replicas.Count)
            {
                ShowCurrentReplica();
            }
            else
            {
                LeaveScreen();
            }
        }

        private void ShowCurrentReplica()
        {
            var replica = dialogData.replicas[currentReplicaId];
            var prevReplica = currentReplicaId > 0 ? dialogData.replicas[currentReplicaId - 1] : null;
            
            personageName.text = replica.characterName;
            text.text = replica.message;

            ShowMain(replica, prevReplica);
            ShowCutIn(replica, prevReplica);
            PlaySound(replica, prevReplica);
        }
        
        private IEnumerator Autoplay()
        {
            while (currentReplicaId < dialogData.replicas.Count)
            {
                ShowCurrentReplica();
                yield return new WaitForSeconds(2f);
                currentReplicaId++;
            }
            AutoplayButtonClick();
        }

        private void ShowMain(AdminBRO.DialogReplica replica, AdminBRO.DialogReplica prevReplica)
        {
            if (replica.mainAnimationId.HasValue)
            {
                if (replica.mainAnimationId.Value != mainAnimation?.animationData.id)
                {
                    Destroy(mainAnimation?.gameObject);
                    mainAnimation = null;

                    var animation = GetAnimationById(replica.mainAnimationId.Value);
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

                    var animation = GetAnimationById(replica.cutInAnimationId.Value);
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

        private void PlaySound(AdminBRO.DialogReplica replica, AdminBRO.DialogReplica prevReplica)
        {
            //main sound
            if (!String.IsNullOrEmpty(replica.mainSoundPath))
            {
                if (replica.mainSoundPath != mainSound?.path)
                {
                    mainSound?.Stop();
                    mainSound = SoundManager.GetEventInstance(replica.mainSoundPath);
                }
            }
            else
            {
                mainSound?.Stop();
                mainSound = null;
            }

            //cutIn sound
            if (!String.IsNullOrEmpty(replica.cutInSoundPath))
            {
                if (replica.cutInSoundPath != cutInSound?.path)
                {
                    cutInSound?.Stop();
                    cutInSound = SoundManager.GetEventInstance(replica.cutInSoundPath);
                }

                mainSound?.Pause();
            }
            else
            {
                cutInSound?.Stop();
                cutInSound = null;

                mainSound?.Play();
            }
        }

        protected virtual AdminBRO.Animation GetAnimationById(int id)
        {
            return GameData.GetAnimationById(id);
        }
    }
}
