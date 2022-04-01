using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSQuestWidget
    {
        public abstract class BaseQuestButton : MonoBehaviour
        {
            protected TextMeshProUGUI title;
            protected Button button;
            public AdminBRO.QuestItem questData { get; set; }

            private void Awake()
            {
                var quest = transform.Find("Quest");
                button = quest.GetComponent<Button>();
                title = quest.Find("Text").GetComponent<TextMeshProUGUI>();
                button.onClick.AddListener(ButtonClick);
            }

            protected virtual void ButtonClick()
            {
                UIManager.ShowOverlay<QuestOverlay>();
            }

            private void Start()
            {
                Customize();
            }

            protected virtual void Customize()
            {
                title.text = questData?.name;
            }
        }
    }
}