using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class DialogScreen : BaseScreen
    {
        protected Coroutine autoplayCoroutine;

        protected Transform charactersPos;
        protected Transform leftCharacterPos;
        protected Transform midCharacterPos;
        protected Transform rightCharacterPos;

        protected Button textContainer;
        protected TextMeshProUGUI personageName;
        protected Image personageHead;
        protected Transform emotionBack;
        protected Transform emotionPos;
        protected TextMeshProUGUI text;

        protected Button skipButton;
        protected Button autoplayButton;
        protected Image autoplayButtonPressed;
        protected TextMeshProUGUI autoplayStatus;

        protected Transform mainAnimPos;
        protected GameObject cutIn;
        protected Transform cutInAnimPos;

        protected AdminBRO.Dialog dialogData;
        protected int currentReplicaId;

        protected bool isAutoplayButtonPressed = false;

        private Dictionary<string, NSDialogScreen.DialogCharacter> characters = 
            new Dictionary<string, NSDialogScreen.DialogCharacter>();
        private Dictionary<string, Transform> slots = new Dictionary<string, Transform>();
        private Dictionary<string, string> slot_character = new Dictionary<string, string>();
        private Dictionary<string, string> character_slot = new Dictionary<string, string>();

        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/DialogScreen/DialogScreen", transform);

            var canvas = screenInst.transform.Find("Canvas");

            charactersPos = canvas.Find("CharactersPos");
            leftCharacterPos = charactersPos.Find("LeftCharacterPos");
            midCharacterPos = charactersPos.Find("MidCharacterPos");
            rightCharacterPos = charactersPos.Find("RightCharacterPos");

            textContainer = canvas.Find("TextContainer").GetComponent<Button>();

            
            textContainer.onClick.AddListener(TextContainerButtonClick);

            personageName = canvas.Find("SubstrateName").Find("PersonageName").GetComponent<TextMeshProUGUI>();
            personageHead = textContainer.transform.Find("PersonageHead").GetComponent<Image>();
            emotionBack = textContainer.transform.Find("EmotionBack");
            emotionPos = emotionBack.Find("EmotionPos");
            text = textContainer.transform.Find("Text").GetComponent<TextMeshProUGUI>();

            skipButton = canvas.Find("SkipButton").GetComponent<Button>();
            skipButton.onClick.AddListener(SkipButtonClick);

            autoplayButton = canvas.Find("AutoplayButton").GetComponent<Button>();
            autoplayButtonPressed = canvas.Find("AutoplayButton").Find("ButtonPressed").GetComponent<Image>();
            autoplayStatus = canvas.Find("AutoplayButton").Find("Status").GetComponent<TextMeshProUGUI>();
            autoplayButton.onClick.AddListener(AutoplayButtonClick);
            autoplayButtonPressed.enabled = false;

            mainAnimPos = canvas.Find("MainAnimPos");
            cutIn = canvas.Find("CutIn").gameObject;
            cutInAnimPos = cutIn.transform.Find("AnimPos");
            cutIn.SetActive(false);
        }

        public override async Task BeforeShowAsync()
        {
            await EnterScreen();

            Initialize();
            ShowCurrentReplica();
            AutoplayButtonCustomize();
        }

        protected virtual async Task EnterScreen()
        {
            dialogData = GameData.GetDialogById(GameGlobalStates.dialog_EventStageData.dialogId.Value);
            await GameData.EventStageStartAsync(GameGlobalStates.dialog_EventStageData);
        }

        protected virtual async void LeaveScreen()
        {
            await GameData.EventStageEndAsync(GameGlobalStates.dialog_EventStageData);

            UIManager.ShowScreen<EventMapScreen>();
        }

        private void HideCharacterByName(string keyName)
        {
            if (keyName == null)
                return;
            if (!characters.ContainsKey(keyName))
                return;

            Destroy(characters[keyName].gameObject);
            characters[keyName] = null;
            var keyPos = character_slot[keyName];
            character_slot[keyName] = null;
            if (keyPos != null)
            {
                slot_character[keyPos] = null;
            }
        }

        private void HideCharacterBySlot(string keyPos)
        {
            if (keyPos == null)
                return;
            if (!slots.ContainsKey(keyPos))
                return;

            var keyName = slot_character[keyPos];
            HideCharacterByName(keyName);
        }

        private void ShowCharacter(string keyName, string keyPos)
        {
            if (keyName == null || keyPos == null)
                return;
            if (!characters.ContainsKey(keyName))
                return;
            if (!slots.ContainsKey(keyPos))
                return;

            if (character_slot[keyName] == keyPos)
                return;

            if (slot_character[keyPos] != null)
            {
                HideCharacterBySlot(keyPos);
            }

            if (characters[keyName] == null)
            {
                var slot = slots[keyPos];
                var prefabPath = GameLocalResources.dialogCharacterPrefabPath[keyName];
                characters[keyName] = NSDialogScreen.DialogCharacter.GetInstance(prefabPath, slot);

                slot_character[keyPos] = keyName;
                character_slot[keyName] = keyPos;
                slot.SetAsLastSibling();
            }
            else
            {
                var curKeyPos = character_slot[keyName];
                slot_character[curKeyPos] = null;
                character_slot[keyName] = null;

                var slot = slots[keyPos];
                characters[keyName].transform.SetParent(slot, false);

                slot_character[keyPos] = keyName;
                character_slot[keyName] = keyPos;
                slot.SetAsLastSibling();
            }
        }

        private void CharacterSelect(string keyName)
        {
            if (keyName == null)
                return;
            if (!characters.ContainsKey(keyName))
                return;

            characters[keyName]?.Select();
        }

        private void CharacterDeselect(string keyName)
        {
            if (keyName == null)
                return;
            if (!characters.ContainsKey(keyName))
                return;

            characters[keyName]?.Deselect();
        }

        protected void Initialize()
        {
            slots[AdminBRO.DialogCharacterPosition.Left] = leftCharacterPos;
            slots[AdminBRO.DialogCharacterPosition.Right] = rightCharacterPos;
            slots[AdminBRO.DialogCharacterPosition.Middle] = midCharacterPos;
            slot_character[AdminBRO.DialogCharacterPosition.Left] = null;
            slot_character[AdminBRO.DialogCharacterPosition.Right] = null;
            slot_character[AdminBRO.DialogCharacterPosition.Middle] = null;

            foreach (var replica in dialogData.replicas)
            {
                var keyName = replica.characterKey;
                var keyPos = replica.characterPosition;

                bool addKeyName = false;
                if (!characters.ContainsKey(keyName))
                {
                    characters[keyName] = null;
                    character_slot[keyName] = null;
                    addKeyName = true;
                }

                if (characters[keyName] == null)
                {
                    if (keyPos != null && addKeyName) 
                    {
                        if (slot_character[keyPos] == null)
                        {
                            ShowCharacter(keyName, keyPos);
                        }
                    }
                }
            }
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
            SoundManager.PlayOneShoot(SoundManager.SoundPath.UI.ButtonClick);
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

        private void SkipButtonClick()
        {
            SoundManager.PlayOneShoot(SoundManager.SoundPath.UI.ButtonClick);
            LeaveScreen();
        }

        private void TextContainerButtonClick()
        {
            SoundManager.PlayOneShoot(SoundManager.SoundPath.UI.ButtonClick);
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
            var prevReplica = (currentReplicaId > 0) ? dialogData.replicas[currentReplicaId - 1] : null;
            if (prevReplica != null)
            {
                var keyName = prevReplica.characterKey;
                var keyPos = prevReplica.characterPosition;

                if (keyPos == null)
                {
                    HideCharacterByName(keyName);
                }
                else
                {
                    CharacterDeselect(keyName);
                }
            }

        
            var replica = dialogData.replicas[currentReplicaId];
            if (replica != null)
            {
                personageName.text = replica.characterName;
                text.text = replica.message;

                var keyName = replica.characterKey;
                var keyPos = replica.characterPosition;

                ShowCharacter(keyName, keyPos);
                CharacterSelect(keyName);
            }
        }
    }
}