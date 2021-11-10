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
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Popups/BuildingPopups/BuildingPopup"));
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
            for (var i = 0; i < 4; i++)
            {
                resource[i] = resourcesGrid.Find("Recource" + (i + 1).ToString());

                notEnough[i] = resource[i].Find("NotEnough").gameObject;
                count[i] = resource[i].Find("Count").GetComponent<Text>();
                recourceIcon[i] = resource[i].Find("RecourceIcon").GetComponent<Image>();
            }

            backButton = canvas.Find("").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);
            backButtonText = backButton.transform.Find("Text").GetComponent<Text>();

            freeBuildButton = canvas.Find("").GetComponent<Button>();
            freeBuildButton.onClick.AddListener(FreeBuildButtonClick);
            freeBuildButtonText = freeBuildButton.transform.Find("Text").GetComponent<Text>();

            paidBuildingButton = canvas.Find("").GetComponent<Button>();
            paidBuildingButton.onClick.AddListener(PaidBuildingButtonClick);
            paidBuildingButtonText = paidBuildingButton.transform.Find("Text").GetComponent<Text>();
            paidBuildingButtonIcon = paidBuildingButton.transform.Find("Icon").GetComponent<Image>();
        }

        private void BackButtonClick()
        {

        }

        protected virtual void FreeBuildButtonClick()
        {

        }

        protected virtual void PaidBuildingButtonClick()
        {

        }
    }

}