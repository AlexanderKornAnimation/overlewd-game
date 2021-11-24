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

        private Transform leftCharacterPos;
        private Transform midCharacterPos;
        private Transform rightCharacterPos;

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

        private Dictionary<string, string> characterPrefabPath = new Dictionary<string, string>
        {
            ["Overlord"] = "Prefabs/UI/Screens/DialogScreen/Overlord",
            ["Ulvi"] = "Prefabs/UI/Screens/DialogScreen/Ulvi",
            ["Faye"] = "Prefabs/UI/Screens/DialogScreen/Faye"
        };
        private Dictionary<string, NSDialogScreen.DialogCharacter> characters = 
            new Dictionary<string, NSDialogScreen.DialogCharacter>();
        private Dictionary<string, Transform> slots = new Dictionary<string, Transform>();
        private Dictionary<string, string> slot_character = new Dictionary<string, string>();
        private Dictionary<string, string> character_slot = new Dictionary<string, string>();

        async void Start()
        {
            var screenPrefab = (GameObject) Instantiate(Resources.Load("Prefabs/UI/Screens/DialogScreen/DialogScreen"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            var canvas = screenRectTransform.Find("Canvas");

            leftCharacterPos = canvas.Find("LeftCharacterPos");
            midCharacterPos = canvas.Find("MidCharacterPos");
            rightCharacterPos = canvas.Find("RightCharacterPos");

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

            Initialize();
            ShowCurrentReplica();

            await GameData.EventStageStartAsync(GameGlobalStates.dialog_EventStageData);
        }

        private void HideCharacterByName(string keyName)
        {
            if (keyName == null)
                return;
            if (!characters.ContainsKey(keyName))
                return;

            Destroy(characters[keyName]);
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
                var prefabPath = characterPrefabPath[keyName];
                characters[keyName] = NSDialogScreen.DialogCharacter.GetInstance(prefabPath, slot);

                slot_character[keyPos] = keyName;
                character_slot[keyName] = keyPos;
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

        private void Initialize()
        {
            slots[AdminBRO.DialogCharacterPosition.Left] = leftCharacterPos;
            slots[AdminBRO.DialogCharacterPosition.Right] = rightCharacterPos;
            slots[AdminBRO.DialogCharacterPosition.Middle] = midCharacterPos;
            slot_character[AdminBRO.DialogCharacterPosition.Left] = null;
            slot_character[AdminBRO.DialogCharacterPosition.Right] = null;
            slot_character[AdminBRO.DialogCharacterPosition.Middle] = null;

            foreach (var replica in dialogData.replicas)
            {
                var keyName = replica.characterName;
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

        private async void SkipButtonClick()
        {
            await GameData.EventStageEndAsync(GameGlobalStates.dialog_EventStageData);

            UIManager.ShowScreen<EventMapScreen>();
        }

        private async void NextButtonClick()
        {
            currentReplicaId++;
            if (currentReplicaId < dialogData.replicas.Count)
            {
                ShowCurrentReplica();
            }
            else
            {
                await GameData.EventStageEndAsync(GameGlobalStates.dialog_EventStageData);

                UIManager.ShowScreen<EventMapScreen>();
            }
        }

        private void ShowCurrentReplica()
        {
            var prevReplica = (currentReplicaId > 0) ? dialogData.replicas[currentReplicaId - 1] : null;
            if (prevReplica != null)
            {
                var keyName = prevReplica.characterName;
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

                var keyName = replica.characterName;
                var keyPos = replica.characterPosition;

                ShowCharacter(keyName, keyPos);
                CharacterSelect(keyName);
            }
        }
    }
}