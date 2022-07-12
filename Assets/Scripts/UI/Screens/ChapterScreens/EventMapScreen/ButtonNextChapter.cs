using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSEventMapScreen
    {
        public class ButtonNextChapter : BaseButton
        {
            public int eventId { get; set; }

            public AdminBRO.EventItem eventData => GameData.events.GetEventById(eventId);

            private void Start()
            {
                Customize();
            }

            private void Customize()
            {

            }

            protected override void ButtonClick()
            {
                base.ButtonClick();
            }

            public static ButtonNextChapter GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<ButtonNextChapter>(
                    "Prefabs/UI/Screens/ChapterScreens/ButtonNextChapter", parent);
            }
        }
    }
}
