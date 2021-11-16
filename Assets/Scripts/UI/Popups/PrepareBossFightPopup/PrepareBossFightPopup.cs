using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class PrepareBossFightPopup : BasePopup
    {
        private Button backButton;
        private Button battleButton;
        private Button prepareButton;
        
        private Image firstTimeReward;
        private Image reward1;
        private Image reward2;
        private Image reward3;
        
       private void Start()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Popups/PrepareBossFightPopup/PrepareBossFightPopup"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            var canvas = screenRectTransform.Find("Canvas");

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);

            battleButton = canvas.Find("BattleButton").GetComponent<Button>();
            battleButton.onClick.AddListener(BattleButtonClick);

            prepareButton = canvas.Find("PrepareBattleButton").GetComponent<Button>();
            prepareButton.onClick.AddListener(PrepareButtonClick);
            
            firstTimeReward = canvas.Find("FirstTimeReward").Find("Resource").GetComponent<Image>();
            reward1 = canvas.Find("Reward1").Find("Resource").GetComponent<Image>();
            reward2 = canvas.Find("Reward2").Find("Resource").GetComponent<Image>();
            reward3 = canvas.Find("Reward3").Find("Resource").GetComponent<Image>();
            
            firstTimeReward.sprite = Resources.Load<Sprite>("Prefabs/UI/Common/Images/Recources/Crystal");
            reward1.sprite = Resources.Load<Sprite>("Prefabs/UI/Common/Images/Recources/Crystal");
            reward2.sprite = Resources.Load<Sprite>("Prefabs/UI/Common/Images/Recources/Gem");
            reward3.sprite = Resources.Load<Sprite>("Prefabs/UI/Common/Images/Recources/Gold");
        }
        
        private void BackButtonClick()
        {
            UIManager.HidePopup();
        }

        private void BattleButtonClick()
        {
            UIManager.ShowScreen<BossFightScreen>();
        }

        private void PrepareButtonClick()
        {
            UIManager.ShowSubPopup<BottlesSubPopup>();
        }
    }
}
