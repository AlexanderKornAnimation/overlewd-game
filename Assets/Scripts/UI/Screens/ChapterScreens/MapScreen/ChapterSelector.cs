using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSMapScreen
    {
        public class ChapterSelector : MonoBehaviour
        {
            private Transform grid;
            private Button closeButton;

            private void Awake()
            {
                closeButton = transform.Find("CloseButton").GetComponent<Button>();
                closeButton.onClick.AddListener(CloseButtonClick);
                grid = transform.Find("ButtonsGrid");
            }

            private void Start()
            {
                Customize();
            }

            private void Customize()
            {
                var chapters = GameData.ftue.info.chapters;

                foreach (var chapter in chapters)
                {
                    var button = ChapterButton.GetInstance(grid);
                    button.chapterId = chapter.id;
                }
            }

            private void CloseButtonClick()
            {
                Hide();
            }

            public void Show()
            {
                gameObject.SetActive(true);
            }

            public void Hide()
            {
                gameObject.SetActive(false);
            }

            public static ChapterSelector GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateScreenPrefab<ChapterSelector>(
                    "Prefabs/UI/Screens/ChapterScreens/ChapterSelector", parent);
            }
        }
    }
}
