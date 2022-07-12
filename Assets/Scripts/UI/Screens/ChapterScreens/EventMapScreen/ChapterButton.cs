using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSEventMapScreen
    {
        public class ChapterButton : MonoBehaviour
        {
            private Button button;
            private TextMeshProUGUI title;
            private TextMeshProUGUI markers;
            public int? eventId { get; set; }
            public AdminBRO.EventChapter chapterData => GameData.events.GetChapterById(eventId);

            private void Awake()
            {
                button = GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);
                title = button.transform.Find("Title").GetComponent<TextMeshProUGUI>();
                markers = button.transform.Find("Markers").GetComponent<TextMeshProUGUI>();
            }

            private void Start()
            {
                Customize();
            }

            private void Customize()
            {
                title.text = chapterData?.name;
            }

            private void ButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            }

            public static ChapterButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<ChapterButton>(
                    "Prefabs/UI/Screens/ChapterScreens/ChapterButton", parent);
            }
        }
    }
}
