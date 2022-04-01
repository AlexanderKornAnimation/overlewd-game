using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class MemoryScreen : BaseFullScreen
    {
        protected Button portalButton;
        protected Button forgeButton;
        protected Button buyButton;
        protected Button backButton;

        protected TextMeshProUGUI commonShardsCount;
        protected TextMeshProUGUI rareShardsCount;
        protected TextMeshProUGUI legendaryShardsCount;
        
        protected virtual void Awake()
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

        protected virtual void Start()
        {
            Customize();
        }

        protected virtual void Customize()
        {
            
        }

        protected virtual void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<CastleScreen>();
        }

        protected virtual void PortalButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<PortalScreen>();
        }

        protected virtual void ForgeButtonClick()
        {
            
        }

        protected virtual void BuyButtonClick()
        {
            
        }
    }
}