using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSQuestOverlay
    {
        public class QuestButton : MonoBehaviour
        {
            private QuestContentScrollView contentScrollView;
            public Transform questContentPos { get; set; }
            public event Action<QuestButton> buttonPressed;

            private Button button;
            private GameObject pressedButton;
            private TextMeshProUGUI title;
            private GameObject notification;

            public int? questId { get; set; }
            public AdminBRO.QuestItem questData => GameData.quests.GetById(questId);
            
            private void Awake()
            {
                var canvas = transform.Find("Canvas");

                button = canvas.Find("Button").GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);
                pressedButton = canvas.Find("Pressed").gameObject;
                pressedButton.SetActive(false);
                title = canvas.Find("Title").GetComponent<TextMeshProUGUI>();

                notification = canvas.Find("Notification").gameObject;
            }

            public void Customize()
            {
                title.text = questData?.name;
                notification.gameObject.SetActive(questData.isClaimed);
                contentScrollView = QuestContentScrollView.GetInstance(questContentPos);
                contentScrollView.questId = questId;
            }
            
            private void ButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                buttonPressed?.Invoke(this);
            }

            public void Select()
            {
                pressedButton.SetActive(true);
                contentScrollView?.Show();
            }

            public void Deselect()
            {
                pressedButton.SetActive(false);
                contentScrollView?.Hide();
            }

            public static QuestButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<QuestButton>(
                    "Prefabs/UI/Overlays/QuestOverlay/QuestButton", parent);
            }
        }
    }
}
