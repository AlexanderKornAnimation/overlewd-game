using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSEventMapScreen
    {
        public class MapButton : MonoBehaviour
        {
            private Button button;
            private TextMeshProUGUI title;
            private TextMeshProUGUI description;

            void Awake()
            {
                var canvas = transform.Find("Canvas");

                button = canvas.Find("Button").GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);

                title = button.transform.Find("Title").GetComponent<TextMeshProUGUI>();
                description = button.transform.Find("Description").GetComponent<TextMeshProUGUI>();
            }

            private void ButtonClick()
            {
                SoundManager.PlayOneShoot(SoundManager.SoundPath.UI.ButtonClick);
                UIManager.ShowScreen<MapScreen>();
            }

            public static MapButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<MapButton>
                    ("Prefabs/UI/Screens/EventMapScreen/MapButton", parent);
            }
        }
    }
}
