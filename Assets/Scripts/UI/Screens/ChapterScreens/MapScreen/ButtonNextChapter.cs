using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSMapScreen
    {
        public class ButtonNextChapter : BaseButton
        {
            public int chapterId { get; set; }

            public AdminBRO.FTUEChapter chapterData => GameData.ftue.info.GetChapterById(chapterId);

            private void Start()
            {
                Customize();
            }

            private void Customize()
            {
                title.text = chapterData.name;
            }

            protected override void ButtonClick()
            {
                base.ButtonClick();
                chapterData.SetAsMapChapter();
                UIManager.ShowScreen<MapScreen>();
            }

            public static ButtonNextChapter GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<ButtonNextChapter>(
                    "Prefabs/UI/Screens/ChapterScreens/ButtonNextChapter", parent);
            }
        }
    }
}
