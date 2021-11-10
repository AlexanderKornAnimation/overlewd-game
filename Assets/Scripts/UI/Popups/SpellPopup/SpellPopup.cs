using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Resharper disable All

namespace Overlewd
{
    public class SpellPopup : BasePopup
    {
        protected List<Transform> recources = new List<Transform>(4);
        protected List<GameObject> notEnough = new List<GameObject>(4);
        protected List<Text> count = new List<Text>(4);
        protected List<Image> recourceIcon = new List<Image>(4);

        protected Transform spawnPoint;

        protected Text spellName;
        protected Text description;
        protected Text fullPotentialDescription;

        protected Button paidBuildButton;
        protected Button freeBuildButton;
        protected Button backButton;

        protected virtual void Start()
        {
            var screenPrefab = (GameObject) Instantiate(Resources.Load("Prefabs/UI/Popups/SpellPopup/SpellPopup"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            var canvas = screenRectTransform.Find("Canvas");

            spawnPoint = canvas.Find("Background").Find("ImageSpawnPoint");

            spellName = canvas.Find("SpellName").GetComponent<Text>();
            description = canvas.Find("Description").GetComponent<Text>();
            fullPotentialDescription = canvas.Find("FullPotentialDescription").GetComponent<Text>();

            paidBuildButton = canvas.Find("PaidBuildButton").GetComponent<Button>();
            freeBuildButton = canvas.Find("FreeBuildButton").GetComponent<Button>();
            backButton = canvas.Find("BackButton").GetComponent<Button>();

            paidBuildButton.onClick.AddListener(PaidBuildButtonClick);
            freeBuildButton.onClick.AddListener(FreeBuildButtonClick);
            backButton.onClick.AddListener(BackButtonClick);
            
            TakeRecources(canvas);
        }

        private void TakeRecources(Transform canvas)
        {
            var grid = canvas.Find("Grid");
            for (int i = 1; i <= recources.Capacity; i++)
            {
                var resource = grid.Find($"Recource{i}");
                recources.Add(resource);
                notEnough.Add(resource.Find("NotEnough").gameObject);
                count.Add(resource.Find("Count").GetComponent<Text>());
                recourceIcon.Add(resource.Find("RecourceIcon").GetComponent<Image>());
            }
        }

        protected virtual void PaidBuildButtonClick()
        {
            UIManager.HidePopup();
        }

        protected virtual void FreeBuildButtonClick()
        {
            UIManager.HidePopup();
        }

        private void BackButtonClick()
        {
            UIManager.HidePopup();
        }
    }
}