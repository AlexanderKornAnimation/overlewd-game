using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class PrepareBattlePopup : BasePopup
    {
        private Button backButton;
        private Button battleButton;
        private Button prepareButton;

        void Start()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Popups/PrepareBattlePopup/PrepareBattlePopup"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            var canvas = screenRectTransform.Find("Canvas");

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);

            battleButton = canvas.Find("BattleButton").GetComponent<Button>();
            battleButton.onClick.AddListener(BattleButtonClick);

            prepareButton = canvas.Find("PrepareBattle").GetComponent<Button>();
            prepareButton.onClick.AddListener(PrepareButtonClick);

        }

        private void BackButtonClick()
        {
            UIManager.HidePopup();
        } 

        private void BattleButtonClick()
        {
            UIManager.ShowScreen<BattleScreen>();
        }

        private void PrepareButtonClick()
        {
            UIManager.ShowSubPopup<BottlesSubPopup>();
        }

        void Update()
        {

        }
    }

}
