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
                equipButton = girl.GetComponent<Button>();
                level = canvas.Find("LevelBack").Find("Level").GetComponent<TextMeshProUGUI>();
                girlClass = canvas.Find("Class").GetComponent<TextMeshProUGUI>();
                equipStatus = canvas.Find("EquipStatus");
                girlScreenButton = canvas.Find("GirlScreenButton").GetComponent<Button>();
                notificationNew = canvas.Find("NotificationNew").GetComponent<GameObject>();
            }

            public static Character GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<Character>(
                    "Prefabs/UI/Screens/TeamEditScreen/Character", parent);
            }
        }
    }
}
