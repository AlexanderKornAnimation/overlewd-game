using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSEventMapScreen
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
