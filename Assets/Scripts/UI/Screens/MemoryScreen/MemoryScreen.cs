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
        
        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/MemoryScreen/Memory", transform);

            var canvas = screenInst.transform.Find("Canvas");

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
        }

        void Start()
        {
            Customize();
        }

        private void Customize()
        {
            
        }

        private void BackButtonClick()
        {
            SoundManager.PlayOneShoot(SoundManager.SoundPath.UI.ButtonClick);
            UIManager.ShowScreen<CastleScreen>();
        }

        private void PortalButtonClick()
        {
            SoundManager.PlayOneShoot(SoundManager.SoundPath.UI.ButtonClick);
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
