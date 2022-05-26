using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class MemoryScreen : BaseFullScreenParent<MemoryScreenInData>
    {
        private Button portalButton;
        private Button forgeButton;
        private Button buyButton;
        private Button backButton;

        private TextMeshProUGUI commonShardsCount;
        private TextMeshProUGUI rareShardsCount;
        private TextMeshProUGUI legendaryShardsCount;
        
        private void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/MemoryScreen/Memory", transform);

            var canvas = screenInst.transform.Find("Canvas");

            backButton = canvas.Find("BackButton").GetComponent <Button>();
            portalButton = canvas.Find("PortalButton").GetComponent<Button>();
            forgeButton = canvas.Find("ForgeButton").GetComponent<Button>();
            buyButton = canvas.Find("BuyButton").GetComponent<Button>();

            commonShardsCount = canvas.Find("Bag").Find("CommonShards").Find("Count").GetComponent<TextMeshProUGUI>();
            rareShardsCount = canvas.Find("Bag").Find("RareShards").Find("Count").GetComponent<TextMeshProUGUI>();
            legendaryShardsCount = canvas.Find("Bag").Find("LegendaryShards").Find("Count").GetComponent<TextMeshProUGUI>();
            
            backButton.onClick.AddListener(BackButtonClick);
            portalButton.onClick.AddListener(PortalButtonClick);
            forgeButton.onClick.AddListener(ForgeButtonClick);
            buyButton.onClick.AddListener(BuyButtonClick);
        }

        private void Start()
        {
            Customize();
        }

        private void Customize()
        {
            
        }
        
        private void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            if (inputData == null)
            {
                UIManager.ShowScreen<MemoryListScreen>();
            }
            else
            {
                UIManager.MakeScreen<MemoryListScreen>().
                    SetData(inputData.prevScreenInData as MemoryListScreenInData)
                    .RunShowScreenProcess();
            }
        }

        private void PortalButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakeScreen<PortalScreen>().
                SetData(new PortalScreenInData
            {
                activeButtonId = PortalScreenInData.shardsButtonId
            }).RunShowScreenProcess();
        }

        private void ForgeButtonClick()
        {
            
        }

        private void BuyButtonClick()
        {
            UIManager.ShowPopup<ChestPopup>();
        }
    }

    public class MemoryScreenInData : BaseFullScreenInData
    {
        public string girlName;
    }
}