using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class BuildingPopup : BasePopup
    {
        protected Transform background;
        protected Transform imageSpawnPoint;

        protected TextMeshProUGUI fullPotentialDescription;
        protected TextMeshProUGUI buildingName;
        protected TextMeshProUGUI description;

        protected Transform resourcesGrid;
        protected Transform[] resource = new Transform[4];
        protected GameObject[] notEnough = new GameObject[4];
        protected TextMeshProUGUI[] count = new TextMeshProUGUI[4];
        protected Image[] recourceIcon = new Image[4];

        protected Button backButton;

        protected Button freeBuildButton;
        protected TextMeshProUGUI freeBuildButtonText;

        protected Button paidBuildingButton;
        protected TextMeshProUGUI paidBuildingButtonText;
        protected Image paidBuildingButtonIcon;

        protected virtual void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Popups/BuildingPopups/BuildingPopup", transform);

            var canvas = screenInst.transform.Find("Canvas");

            background = canvas.Find("Background");
            imageSpawnPoint = background.Find("ImageSpawnPoint");

            fullPotentialDescription = canvas.Find("FullPotentialDescription").GetComponent<TextMeshProUGUI>();
            buildingName = canvas.Find("BuildingName").GetComponent<TextMeshProUGUI>();
            description = canvas.Find("Description").GetComponent<TextMeshProUGUI>();

            resourcesGrid = canvas.Find("Grid");
            for (var i = 0; i < resource.Length; i++)
            {
                resource[i] = resourcesGrid.Find($"Recource{i + 1}");

                notEnough[i] = resource[i].Find("NotEnough").gameObject;
                count[i] = resource[i].Find("Count").GetComponent<TextMeshProUGUI>();
                recourceIcon[i] = resource[i].Find("RecourceIcon").GetComponent<Image>();
            }

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);

            freeBuildButton = canvas.Find("FreeBuildButton").GetComponent<Button>();
            freeBuildButton.onClick.AddListener(FreeBuildButtonClick);
            freeBuildButtonText = freeBuildButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();

            paidBuildingButton = canvas.Find("PaidBuildingButton").GetComponent<Button>();
            paidBuildingButton.onClick.AddListener(PaidBuildingButtonClick);
            paidBuildingButtonText = paidBuildingButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            paidBuildingButtonIcon = paidBuildingButton.transform.Find("Icon").GetComponent<Image>();
        }

        private void Start()
        {
            Customize();
        }

        protected virtual void Customize()
        {
            recourceIcon[0].sprite = ResourceManager.InstantiateAsset<Sprite>("Common/Images/Gold");
            recourceIcon[1].sprite = ResourceManager.InstantiateAsset<Sprite>("Common/Images/Stone");
            recourceIcon[2].sprite = ResourceManager.InstantiateAsset<Sprite>("Common/Images/Wood");
            recourceIcon[3].sprite = ResourceManager.InstantiateAsset<Sprite>("Common/Images/Gem");
        }

        protected virtual void BackButtonClick()
        {
            SoundManager.PlayOneShoot(SoundPath.UI.GenericButtonClick);
            UIManager.HidePopup();
        }

        protected virtual void FreeBuildButtonClick()
        {
            SoundManager.PlayOneShoot(SoundPath.UI.FreeBuildButton);
            UIManager.HidePopup();
        }

        protected virtual void PaidBuildingButtonClick()
        {
            SoundManager.PlayOneShoot(SoundPath.UI.GenericButtonClick);
            UIManager.HidePopup();
        }

        public override ScreenShow Show()
        {
            return gameObject.AddComponent<ScreenLeftShow>();
        }

        public override ScreenHide Hide()
        {
            return gameObject.AddComponent<ScreenLeftHide>();
        }
    }
}