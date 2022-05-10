using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSEventMapScreen
    {
        public class MapButton : BaseButton
        {
            private TextMeshProUGUI description;
            private TextMeshProUGUI questMark;

            protected override void Awake()
            {
                base.Awake();
                description = button.transform.Find("Description").GetComponent<TextMeshProUGUI>();
                questMark = button.transform.Find("QuestMark").GetComponent<TextMeshProUGUI>();
            }

            protected override void ButtonClick()
            {
                base.ButtonClick();
                UIManager.ShowScreen<MapScreen>();
            }

            public static MapButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<MapButton>
                    ("Prefabs/UI/Screens/ChapterScreens/MapButton", parent);
            }
        }
    }
}
