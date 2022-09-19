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
            public AdminBRO.Character characterData { get; set; }

            protected Transform canvas;

            protected Image icon;
            protected TextMeshProUGUI level;
            protected GameObject levelBack;
            protected TextMeshProUGUI characterClass;

            protected virtual void Awake()
            {
                canvas = transform.Find("Canvas");
                icon = canvas.Find("Icon").GetComponent<Image>();
                levelBack = canvas.Find("LevelBack").gameObject;
                level = levelBack.transform.Find("Level").GetComponent<TextMeshProUGUI>();
                characterClass = canvas.Find("Class").GetComponent<TextMeshProUGUI>();
            }

            protected virtual void Start()
            {
                Customize();
            }

            protected virtual void Customize()
            {
                if (characterData == null)
                    return;

                level.text = characterData.level.ToString();
                icon.sprite = ResourceManager.LoadSprite(characterData.iconUrl);
                characterClass.text = characterData.classMarker;

            }
        }
    }
}