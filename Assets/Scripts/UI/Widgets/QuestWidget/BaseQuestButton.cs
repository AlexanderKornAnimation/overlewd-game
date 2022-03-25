using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace QuestWidget
    {
        public class BaseQuestButton : MonoBehaviour
        {
            protected TextMeshProUGUI title;
            protected Button button;
            protected static int questId = 1;

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
                var questData = GameData.GetEventQuestById(questId);
                questId += 1;
                title.text = questData.name;
            }
        }
    }
}