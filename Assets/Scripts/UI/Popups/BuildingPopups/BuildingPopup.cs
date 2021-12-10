using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class BuildingPopup : BasePopup
    {
        protected Transform background;
        protected Transform imageSpawnPoint;

        protected Text recourcesNeeded;
        protected Text fullPotentialTitle;
        protected Text fullPotentialDescription;
        protected Text buildingName;
        protected Text description;

        protected Transform resourcesGrid;
        protected Transform[] resource = new Transform[4];
        protected GameObject[] notEnough = new GameObject[4];
        protected Text[] count = new Text[4];
        protected Image[] recourceIcon = new Image[4];

        protected Button backButton;
        protected Text backButtonText;

        protected Button freeBuildButton;
        protected Text freeBuildButtonText;

        protected Button paidBuildingButton;
        protected Text paidBuildingButtonText;
        protected Image paidBuildingButtonIcon;

        protected virtual void Start()
        {
            var screenPrefab =
                (GameObject) Instantiate(Resources.Load("Prefabs/UI/Popups/BuildingPopups/BuildingPopup"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            var canvas = screenRectTransform.Find("Canvas");

            background = canvas.Find("Background");
            imageSpawnPoint = background.Find("ImageSpawnPoint");

            recourcesNeeded = canvas.Find("RecourcesNeeded").GetComponent<Text>();
            fullPotentialTitle = canvas.Find("FullPotentialTitle").GetComponent<Text>();
            fullPotentialDescription = canvas.Find("FullPotentialDescription").GetComponent<Text>();
            buildingName = canvas.Find("BuildingName").GetComponent<Text>();
            description = canvas.Find("Description").GetComponent<Text>();

            resourcesGrid = canvas.Find("Grid");
            for (var i = 0; i < resource.Length; i++)
            {
                resource[i] = resourcesGrid.Find($"Recource{i + 1}");

                notEnough[i] = resource[i].Find("NotEnough").gameObject;
                count[i] = resource[i].Find("Count").GetComponent<Text>();
                recourceIcon[i] = resource[i].Find("RecourceIcon").GetComponent<Image>();
            }

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);
            backButtonText = backButton.transform.Find("Text").GetComponent<Text>();

            freeBuildButton = canvas.Find("FreeBuildButton").GetComponent<Button>();
            freeBuildButton.onClick.AddListener(FreeBuildButtonClick);
            freeBuildButtonText = freeBuildButton.transform.Find("Text").GetComponent<Text>();

            paidBuildingButton = canvas.Find("PaidBuildingButton").GetComponent<Button>();
            paidBuildingButton.onClick.AddListener(PaidBuildingButtonClick);
            paidBuildingButtonText = paidBuildingButton.transform.Find("Text").GetComponent<Text>();
            paidBuildingButtonIcon = paidBuildingButton.transform.Find("Icon").GetComponent<Image>();

            Customize();
        }

        protected virtual void Customize()
        {
            recourceIcon[0].sprite = Resources.Load<Sprite>("Prefabs/UI/Common/Images/Recources/Gold");
            recourceIcon[1].sprite = Resources.Load<Sprite>("Prefabs/UI/Common/Images/Recources/Stone");
            recourceIcon[2].sprite = Resources.Load<Sprite>("Prefabs/UI/Common/Images/Recources/Wood");
            recourceIcon[3].sprite = Resources.Load<Sprite>("Prefabs/UI/Common/Images/Recources/Gem");
        }

        private void BackButtonClick()
        {
        }

        protected virtual void FreeBuildButtonClick()
        {
            UIManager.HidePopup();
        }

        protected virtual void PaidBuildingButtonClick()
        {
            UIManager.HidePopup();
        }
    }
}