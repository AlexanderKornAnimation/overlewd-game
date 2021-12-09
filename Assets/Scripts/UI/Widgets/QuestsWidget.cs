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
    
        private void Awake()
        {
            var canvas = transform.Find("Canvas");

            mainQuestButton = canvas.Find("MainQuest").GetComponent<Button>();
            sideQuestButton1 = canvas.Find("SideQuest1").GetComponent<Button>();
            sideQuestButton2 = canvas.Find("SideQuest2").GetComponent<Button>();
            sideQuestButton3 = canvas.Find("SideQuest3").GetComponent<Button>();
            
            mainQuestButton.onClick.AddListener(OnQuestButtonClick);
            sideQuestButton1.onClick.AddListener(OnQuestButtonClick);
            sideQuestButton2.onClick.AddListener(OnQuestButtonClick);
            sideQuestButton3.onClick.AddListener(OnQuestButtonClick);
        }

        protected virtual void OnQuestButtonClick()
        {
            UIManager.ShowOverlay<QuestOverlay>();
        }
        
        public static QuestsWidget GetInstance(Transform parent)
        {
            var prefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Widgets/QuestsWidget/QuestWidget"), parent);
            prefab.name = nameof(QuestsWidget);
            var rectTransform = prefab.GetComponent<RectTransform>();
            UIManager.SetStretch(rectTransform);
            return prefab.AddComponent<QuestsWidget>();
        }
    }
}
