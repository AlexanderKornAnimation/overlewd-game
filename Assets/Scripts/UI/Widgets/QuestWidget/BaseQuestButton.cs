using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;

namespace Overlewd
{
    namespace NSQuestWidget
    {
        public abstract class BaseQuestButton : MonoBehaviour
        {
            public int questId { get; set; }
            public AdminBRO.QuestItem questData =>
                GameData.quests.GetById(questId);

            protected RectTransform root;
            protected RectTransform questBack;
            protected TextMeshProUGUI title;
            protected TextMeshProUGUI mark;
            protected Button button;

            private Vector2 baseSizeDelta;
            private float basePosX;

            private void Awake()
            {
                root = transform as RectTransform;

                questBack = transform.Find("Quest") as RectTransform;
                button = questBack.GetComponent<Button>();
                title = questBack.Find("Text").GetComponent<TextMeshProUGUI>();
                mark = questBack.Find("QuestMark").GetComponent<TextMeshProUGUI>();
                button.onClick.AddListener(ButtonClick);

                basePosX = questBack.anchoredPosition.x;
                baseSizeDelta = root.sizeDelta;
            }

            protected virtual void ButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                UIManager.MakeOverlay<QuestOverlay>().
                    SetData(new QuestOverlayInData 
                    {
                        questId = questId
                    }).DoShow();
            }

            private void Start()
            {
                Customize();
            }

            protected virtual void Customize()
            {
                title.text = questData?.name;
            }

            public void SetHide()
            {
                root.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0.0f);
                questBack.anchoredPosition += new Vector2(500.0f, 0.0f);
                title.gameObject.SetActive(false);
                mark.gameObject.SetActive(false);
            }

            public async Task WaitShowAsNew()
            {
                var scrollContent_vlg = transform.parent.GetComponent<VerticalLayoutGroup>();

                var seq = DOTween.Sequence();
                seq.Append(root.DOSizeDelta(baseSizeDelta, 0.2f));
                seq.Append(questBack.DOAnchorPosX(basePosX, 0.3f));
                seq.onUpdate = () => 
                {
                    scrollContent_vlg.enabled = false;
                    scrollContent_vlg.enabled = true;
                };
                seq.AppendCallback(() => 
                {
                    var effect = SpineWidget.GetInstanceDisposable(GameData.animations["uifx_quest_book01"], questBack);
                    effect.transform.localPosition += new Vector3(-210.0f, 0.0f, 0.0f);
                });
                seq.AppendInterval(0.2f);
                seq.AppendCallback(() => 
                {
                    title.gameObject.SetActive(true);
                    mark.gameObject.SetActive(true);
                });
                seq.AppendInterval(0.3f);
                seq.onComplete = () =>
                {
                    questData.isNew = false;
                };
                seq.Play();
                await seq.AsyncWaitForCompletion();
            }
        }
    }
}