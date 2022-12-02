using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSQuestOverlay
    {
        public class QuestButton : MonoBehaviour
        {
            public QuestOverlay questOverlay { get; set; }
            public Transform questContentPos { get; set; }

            private QuestContentScrollView contentScrollView;

            private Button button;
            private GameObject pressedButton;
            private TextMeshProUGUI title;
            private GameObject notification;

            public int? questId { get; set; }
            public AdminBRO.QuestItem questData => GameData.quests.GetById(questId);

            public bool isSelected { get; private set; } = false;
            
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
                contentScrollView.questButton = this;
            }
            
            private void ButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                Select();
            }

            public void Select()
            {
                if (!isSelected)
                {
                    questOverlay.selectedQuest?.Deselect();

                    isSelected = true;
                    pressedButton.SetActive(true);
                    contentScrollView?.Show();
                    questOverlay.SelectQuest(this);
                }
            }

            public void Deselect()
            {
                isSelected = false;
                pressedButton.SetActive(false);
                contentScrollView?.Hide();
            }

            public void Remove()
            {
                var grid_vlg = transform.parent.GetComponent<VerticalLayoutGroup>();
                var buttonCG = gameObject.GetComponent<CanvasGroup>();
                var buttonRT = transform as RectTransform;
                var buttonRT_sizeDeltaEnd = buttonRT.sizeDelta;
                buttonRT_sizeDeltaEnd.y = 0.0f;
                var contentCG = contentScrollView.GetComponent<CanvasGroup>();

                var seq = DOTween.Sequence();
                seq.AppendCallback(() =>
                {
                    UIfx.Inst(UIfx.UIFX_QUEST_BOOK01, buttonRT, new Vector2(-190.0f, 0.0f));
                });
                seq.AppendInterval(0.2f);
                seq.Append(buttonCG.DOFade(0.0f, 0.3f));
                seq.Join(contentCG.DOFade(0.0f, 0.3f));
                seq.Append(buttonRT.DOSizeDelta(buttonRT_sizeDeltaEnd, 0.4f));
                seq.onUpdate = () =>
                {
                    grid_vlg.enabled = false;
                    grid_vlg.enabled = true;
                };
                seq.onComplete = () =>
                {
                    DestroyImmediate(contentScrollView.gameObject);
                    DestroyImmediate(gameObject);

                    if (questOverlay.selectedQuest == null)
                    {
                        questOverlay.questButtons.FirstOrDefault()?.Select();
                    }
                };
                seq.Play();
                seq.SetLink(gameObject);
            }

            public static QuestButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<QuestButton>(
                    "Prefabs/UI/Overlays/QuestOverlay/QuestButton", parent);
            }
        }
    }
}
