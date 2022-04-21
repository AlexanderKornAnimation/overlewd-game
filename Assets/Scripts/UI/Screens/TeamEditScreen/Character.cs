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

            void Awake()
            {
                var canvas = transform.Find("Canvas");
                girlIcon = canvas.Find("Girl").GetComponent<Image>();
                level = canvas.Find("LevelBack").Find("Level").GetComponent<TextMeshProUGUI>();
                girlClass = canvas.Find("Class").GetComponent<TextMeshProUGUI>();
                equipStatus = canvas.Find("Notification");
            }

            public static Character GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<Character>(
                    "Prefabs/UI/Screens/TeamEditScreen/Character", parent);
            }
        }
    }
}
