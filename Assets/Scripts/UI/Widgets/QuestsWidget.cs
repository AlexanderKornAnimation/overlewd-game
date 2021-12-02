using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class QuestsWidget : BaseWidget
    {
        private Button mainQuestButton;
        private Button sideQuestButton1;
        private Button sideQuestButton2;
        private Button sideQuestButton3;
    
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

        private void OnQuestButtonClick()
        {
            UIManager.ShowOverlay<QuestOverlay>();
        }
        
        public static QuestsWidget CreateInstance(Transform parent)
        {
            var prefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Widgets/QuestsWidget/QuestWidget"));
            prefab.name = nameof(QuestsWidget);
            var rectTransform = prefab.GetComponent<RectTransform>();
            rectTransform.SetParent(parent, false);
            UIManager.SetStretch(rectTransform);
            return prefab.AddComponent<QuestsWidget>();
        }
    }
}
