using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class HaremScreen : BaseScreen
    {
        private Button backButton;

        private Button ulviButton;
        private Image ulviGirl;
        private Image ulviBuff;
        private Text ulviDescription;
        private Text ulviName;

        private Button adrielButton;
        private Image adrielGirl;
        private Image adrielBuff;
        private Text adrielDescription;
        private Text adrielName;
        private Transform adrielNotActive;

        private Button ingieButton;
        private Text ingieName;
        private Transform ingieNotActive;

        private Button fayeButton;
        private Text fayeName;
        private Transform fayeNotActive;

        private Button liliButton;
        private Text liliName;
        private Transform liliNotActive;

        private Button battleGirlsButton;
        private Image battleGirlsGirl;
        private Text battleGirlsTitle;

        void Start()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/HaremScreen/Harem"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            var canvas = screenRectTransform.Find("Canvas");

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);

            ulviButton = canvas.Find("UlviButton").GetComponent<Button>();
            ulviButton.onClick.AddListener(UlviButtonClick);
            ulviGirl = ulviButton.transform.Find("Girl").GetComponent<Image>();
            ulviBuff = ulviButton.transform.Find("Buff").GetComponent<Image>();
            ulviDescription = ulviButton.transform.Find("Description").GetComponent<Text>();
            ulviName = ulviButton.transform.Find("Name").GetComponent<Text>();

            adrielButton = canvas.Find("AdrielButton").GetComponent<Button>();
            adrielButton.onClick.AddListener(AdrielButtonClick);
            adrielGirl = adrielButton.transform.Find("Girl").GetComponent<Image>();
            adrielBuff = adrielButton.transform.Find("Buff").GetComponent<Image>();
            adrielDescription = adrielButton.transform.Find("Description").GetComponent<Text>();
            adrielName = adrielButton.transform.Find("Name").GetComponent<Text>();
            adrielNotActive = adrielButton.transform.Find("NotActive");

            ingieButton = canvas.Find("IngieButton").GetComponent<Button>();
            ingieButton.onClick.AddListener(IngieButtonClick);
            ingieName = ingieButton.transform.Find("Name").GetComponent<Text>();
            ingieNotActive = ingieButton.transform.Find("NotActive");

            fayeButton = canvas.Find("FayeButton").GetComponent<Button>();
            fayeButton.onClick.AddListener(FayeButtonClick);
            fayeName = fayeButton.transform.Find("Name").GetComponent<Text>();
            fayeNotActive = fayeButton.transform.Find("NotActive");

            liliButton = canvas.Find("LiliButton").GetComponent<Button>();
            liliButton.onClick.AddListener(LiliButtonClick);
            liliName = liliButton.transform.Find("Name").GetComponent<Text>();
            liliNotActive = liliButton.transform.Find("NotActive");

            battleGirlsButton = canvas.Find("BattleGirlsButton").GetComponent<Button>();
            battleGirlsButton.onClick.AddListener(BattleGirlsButtonClick);
            battleGirlsGirl = battleGirlsButton.transform.Find("Girl").GetComponent<Image>();
            battleGirlsTitle = battleGirlsButton.transform.Find("Title").GetComponent<Text>();

            Customize();
        }

        private void Customize()
        {

        }

        private void BackButtonClick()
        {
            UIManager.ShowScreen<CastleScreen>();
        }

        private void UlviButtonClick()
        {
            UIManager.ShowScreen<GirlScreen>();
        }

        private void AdrielButtonClick()
        {
            UIManager.ShowScreen<GirlScreen>();
        }

        private void IngieButtonClick()
        {

        }

        private void FayeButtonClick()
        {

        }

        private void LiliButtonClick()
        {

        }

        private void BattleGirlsButtonClick()
        {

        }
    }
}
