using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSBattleGirlListScreen
    {
        public class Character : MonoBehaviour
        {
            private Image icon;
            private GameObject notificationSexSceneClose;
            private GameObject notificationSexSceneOpen;
            private TextMeshProUGUI level;
            private TextMeshProUGUI characterClass;
            private Button button;
            public BattleGirlListScreenInData inputData;

            public int? characterId { get; set; }
            public AdminBRO.Character characterData => GameData.characters.GetById(characterId);

            private void Awake()
            {
                var canvas = transform.Find("Canvas");
                icon = canvas.Find("Icon").GetComponent<Image>();
                notificationSexSceneClose = canvas.Find("NotificationSexSceneClose").gameObject;
                notificationSexSceneOpen= canvas.Find("NotificationSexSceneOpen").gameObject;
                level = canvas.Find("LevelBack/Level").GetComponent<TextMeshProUGUI>();
                characterClass = canvas.Find("Class").GetComponent<TextMeshProUGUI>();
                button = canvas.Find("Button").GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);
            }

            private void Start()
            {
                Customize();
            }

            private void Customize()
            {
                icon.sprite = ResourceManager.LoadSprite(characterData.iconUrl);
                characterClass.text = characterData.classMarker;
                notificationSexSceneClose.SetActive(!characterData.isSexSceneOpen);
                notificationSexSceneOpen.SetActive(characterData.isSexSceneOpen);
                level.text = characterData.level.ToString();
            }

            private void ButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                UIManager.MakeScreen<BattleGirlScreen>().
                    SetData(new BattleGirlScreenInData
                    {
                        characterId = characterId,
                    }).DoShow();
            }
            
            public static Character GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<Character>(
                    "Prefabs/UI/Screens/BattleGirlListScreen/BattleGirl", parent);
            }
        }
    }    
}
