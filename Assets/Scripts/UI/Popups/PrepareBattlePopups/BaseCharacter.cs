using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSPrepareBattlePopup
    {
        public abstract class BaseCharacter : MonoBehaviour
        {
            public int? characterId { get; set; }
            public AdminBRO.Character characterData => GameData.characters.GetById(characterId);

            protected Transform canvas;
            public Transform widgetPos { get; set; }

            protected Image icon;
            protected TextMeshProUGUI level;
            protected GameObject levelBack;
            protected TextMeshProUGUI characterClass;
            protected Button button;

            protected virtual void Awake()
            {
                canvas = transform.Find("Canvas");
                icon = canvas.Find("Icon").GetComponent<Image>();
                levelBack = canvas.Find("LevelBack").gameObject;
                level = levelBack.transform.Find("Level").GetComponent<TextMeshProUGUI>();
                characterClass = canvas.Find("Class").GetComponent<TextMeshProUGUI>();
                button = canvas.Find("Button")?.GetComponent<Button>();
                button?.onClick.AddListener(ButtonClick);
            }

            protected virtual void Start()
            {
                Customize();
            }

            protected virtual void Customize()
            {
                if (characterData == null)
                    return;
                
                button.gameObject.SetActive(characterId != GameData.characters.overlord.id);
                level.text = characterData.level.ToString();
                icon.sprite = ResourceManager.LoadSprite(characterData.iconUrl);
                characterClass.text = characterData.classMarker;
            }

            protected void ButtonClick()
            {
                var charInfo = CharacterInfo.GetInstance(widgetPos);
                charInfo.chId = characterId;
            }
        }
    }
}