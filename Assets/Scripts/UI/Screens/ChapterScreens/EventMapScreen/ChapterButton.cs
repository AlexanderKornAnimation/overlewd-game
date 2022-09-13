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
            public int? chapterId { get; set; }
            public AdminBRO.EventChapter chapterData => GameData.events.GetChapterById(chapterId);

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

                if (chapterData.isOpen)
                {
                    UITools.DisableButton(button, GameData.events.mapChapter.id == chapterId);
                }
                else
                {
                    UITools.DisableButton(button);
                }
            }

            private void ButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                chapterData.SetAsMapChapter();
                UIManager.ShowScreen<EventMapScreen>();
            }

            public static ChapterButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<ChapterButton>(
                    "Prefabs/UI/Screens/ChapterScreens/ChapterButton", parent);
            }
        }
    }
}
