using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Overlewd
{
    namespace NSTeamEditScreen
    {
        public class Character : MonoBehaviour
        {
            public int characterId { get; set; }
            public TeamEditScreen screen { private get; set; }
            public TeamEditScreenInData inputData { get; set; }

            private Image girlIcon;
            private TextMeshProUGUI level;
            private TextMeshProUGUI girlClass;
            private Transform equipStatus;
            private Button girlScreenButton;
            private Button equipButton;
            private GameObject notificationNew;

            void Awake()
            {
                var canvas = transform.Find("Canvas");
                var girl = canvas.Find("Girl");
                
                girlIcon = girl.GetComponent<Image>();
                equipButton = canvas.Find("EquipButton").GetComponent<Button>();
                equipButton.onClick.AddListener(EquipButtonClick);
                level = canvas.Find("LevelBack").Find("Level").GetComponent<TextMeshProUGUI>();
                girlClass = canvas.Find("Class").GetComponent<TextMeshProUGUI>();
                equipStatus = canvas.Find("EquipStatus");
                girlScreenButton = canvas.Find("GirlScreenButton").GetComponent<Button>();
                girlScreenButton.onClick.AddListener(GirlScreenButtonClick);
                notificationNew = canvas.Find("NotificationNew").GetComponent<GameObject>();
            }

            void Start()
            {
                Customize();
            }

            public void Customize()
            {
                var chData = GameData.characters.GetById(characterId);
                girlIcon.sprite = ResourceManager.LoadSprite(chData.teamEditPersIcon);
                level.text = chData.level.ToString();
                girlClass.text = AdminBRO.Character.GetMyClassMarker(chData.characterClass);
                if (chData.teamPosition != AdminBRO.Character.TeamPosition_None)
                {
                    girlIcon.color = Color.gray;
                    equipStatus.gameObject.SetActive(true);
                }
                else
                {
                    girlIcon.color = Color.white;
                    equipStatus.gameObject.SetActive(false);
                }
            }

            private void EquipButtonClick()
            {
                screen.TryEquipOrUnequip(characterId);
            }

            private void GirlScreenButtonClick()
            {
                if (inputData == null)
                {
                    UIManager.ShowScreen<BattleGirlScreen>();
                }
                else
                {
                    UIManager.MakeScreen<BattleGirlScreen>().
                        SetData(new BattleGirlScreenInData 
                        {
                            prevScreenInData = inputData,
                            ftueStageId = inputData.ftueStageId,
                            eventStageId = inputData.eventStageId,
                            characterId = characterId
                        }).RunShowScreenProcess();
                }
            }

            public static Character GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<Character>(
                    "Prefabs/UI/Screens/TeamEditScreen/Character", parent);
            }
        }
    }
}
