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

            protected Image art;
            protected TextMeshProUGUI level;
            protected TextMeshProUGUI characterClass;

            protected virtual void Awake()
            {
                canvas = transform.Find("Canvas");
                art = canvas.Find("Art").GetComponent<Image>();
                level = canvas.Find("LevelBack").Find("Level").GetComponent<TextMeshProUGUI>();
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
                art.sprite = ResourceManager.LoadSprite(characterData.teamEditPersIcon);
                SetClassIcon();
            }

            protected virtual void SetClassIcon()
            {
                characterClass.text = characterData.classMarker;
            }
        }
    }
}