using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class MemoryScreen : BaseScreen
    {
        private Button portalButton;
        private Button forgeButton;
        private Button buyButton;
        private Button backButton;

        private TextMeshProUGUI commonShardsCount;
        private TextMeshProUGUI rareShardsCount;
        private TextMeshProUGUI legendaryShardsCount;
        
        private void Start()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/MemoryScreen/Memory"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            var canvas = screenRectTransform.Find("Canvas");

            backButton = canvas.Find("BackButton").GetComponent <Button>();
            portalButton = canvas.Find("PortalButton").GetComponent<Button>();
            forgeButton = canvas.Find("PortalButton").GetComponent<Button>();
            buyButton = canvas.Find("PortalButton").GetComponent<Button>();

            commonShardsCount = canvas.Find("Bag").Find("CommonShards").Find("Count").GetComponent<TextMeshProUGUI>();
            rareShardsCount = canvas.Find("Bag").Find("RareShards").Find("Count").GetComponent<TextMeshProUGUI>();
            legendaryShardsCount = canvas.Find("Bag").Find("LegendaryShards").Find("Count").GetComponent<TextMeshProUGUI>();
            
            backButton.onClick.AddListener(BackButtonClick);
            portalButton.onClick.AddListener(PortalButtonClick);
            forgeButton.onClick.AddListener(ForgeButtonClick);
            buyButton.onClick.AddListener(BuyButtonClick);
            
            Customize();
        }

        private void Customize()
        {
            
        }

        private void BackButtonClick()
        {
            UIManager.ShowScreen<CastleScreen>();
        }

        private void PortalButtonClick()
        {
            UIManager.ShowScreen<PortalScreen>();
        }

        private void ForgeButtonClick()
        {
            
        }

        private void BuyButtonClick()
        {
            
        }
    }
}
