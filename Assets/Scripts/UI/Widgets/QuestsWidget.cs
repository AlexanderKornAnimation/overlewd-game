using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class QuestsWidget : BaseWidget
    {
        protected Button mainQuestButton;
        protected Button sideQuestButton1;
        protected Button sideQuestButton2;
        protected Button sideQuestButton3;
    
        void Awake()
        {
            var canvas = transform.Find("Canvas");
            var quests = canvas.Find("Quests");

            mainQuestButton = quests.Find("MainQuest").GetComponent<Button>();
            sideQuestButton1 = quests.Find("SideQuest1").GetComponent<Button>();
            sideQuestButton2 = quests.Find("SideQuest2").GetComponent<Button>();
            sideQuestButton3 = quests.Find("SideQuest3").GetComponent<Button>();
            
            mainQuestButton.onClick.AddListener(OnQuestButtonClick);
            sideQuestButton1.onClick.AddListener(OnQuestButtonClick);
            sideQuestButton2.onClick.AddListener(OnQuestButtonClick);
            sideQuestButton3.onClick.AddListener(OnQuestButtonClick);
        }

        protected virtual void OnQuestButtonClick()
        {
            SoundManager.PlayOneShoot(SoundPath.UI_GenericButtonClick);
            UIManager.ShowOverlay<QuestOverlay>();
        }
        
        public static QuestsWidget GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateScreenPrefab<QuestsWidget>
                ("Prefabs/UI/Widgets/QuestsWidget/QuestWidget", parent);
        }
    }
}
