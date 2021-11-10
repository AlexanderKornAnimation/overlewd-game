using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Resharper disable All

namespace Overlewd
{
    public class SpellPopup : BasePopup
    {
        private List<Transform> recources = new List<Transform>(4);

        private Transform spawnPoint;

        private Text spellName;
        private Text description;
        private Text fullPotentialDescription;

        private Button paidBuildButton;
        private Button freeBuildButton;
        private Button backButton;

        private void Start()
        {
            var screenPrefab = (GameObject) Instantiate(Resources.Load("Prefabs/UI/Popups/SpellPopup/SpellPopup"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            var canvas = screenRectTransform.Find("Canvas");

            spawnPoint = canvas.Find("Background").Find("ImageSpawnPoint");
            SpawnImage(spawnPoint, "Prefabs/UI/Popups/SpellPopup/FireballImage");

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
            for (int i = 1; i <= recources.Capacity; i++)
                recources.Add(canvas.Find("Grid").Find($"Recource{i}").GetComponent<Transform>());
        }
        
        private void SpawnImage(Transform parent, string path)
        {
            Instantiate(Resources.Load(path), parent);
        }

        private void PaidBuildButtonClick()
        {
            UIManager.HidePopup();
        }

        private void FreeBuildButtonClick()
        {
            UIManager.HidePopup();
        }

        private void BackButtonClick()
        {
            UIManager.HidePopup();
        }
    }
}