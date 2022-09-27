using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSQuestWidget
    {
        public abstract class BaseQuestButton : MonoBehaviour
        {
            public int questId { get; set; }
            public AdminBRO.QuestItem questData =>
                GameData.quests.GetById(questId);

            protected TextMeshProUGUI title;
            protected Button button;

            private void Awake()
            {
                var quest = transform.Find("Quest");
                button = quest.GetComponent<Button>();
                title = quest.Find("Text").GetComponent<TextMeshProUGUI>();
                button.onClick.AddListener(ButtonClick);
            }

            protected virtual void ButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                UIManager.MakeOverlay<QuestOverlay>().
                    SetData(new QuestOverlayInData 
                    {
                        questId = questId
                    }).RunShowOverlayProcess();
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