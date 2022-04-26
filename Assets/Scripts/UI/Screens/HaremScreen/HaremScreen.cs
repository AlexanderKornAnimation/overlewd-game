using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class HaremScreen : BaseFullScreen
    {
        protected Button backButton;

        protected Button ulviButton;
        protected Image ulviGirl;
        protected Image ulviBuff;
        protected TextMeshProUGUI ulviDescription;
        protected TextMeshProUGUI ulviName;

        protected Button adrielButton;
        protected Image adrielGirl;
        protected Image adrielBuff;
        protected TextMeshProUGUI adrielDescription;
        protected TextMeshProUGUI adrielName;
        protected Transform adrielNotActive;

        protected Button ingieButton;
        protected TextMeshProUGUI ingieName;
        protected Transform ingieNotActive;

        protected Button fayeButton;
        protected TextMeshProUGUI fayeName;
        protected Transform fayeNotActive;

        protected Button liliButton;
        protected TextMeshProUGUI liliName;
        protected Transform liliNotActive;

        protected Button battleGirlsButton;
        protected Image battleGirlsGirl;
        protected TextMeshProUGUI battleGirlsTitle;

        protected virtual void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/HaremScreen/Harem", transform);

            var canvas = screenInst.transform.Find("Canvas");

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);

            ulviButton = canvas.Find("UlviButton").GetComponent<Button>();
            ulviButton.onClick.AddListener(UlviButtonClick);
            ulviGirl = ulviButton.transform.Find("Girl").GetComponent<Image>();
            ulviBuff = ulviButton.transform.Find("Buff").GetComponent<Image>();
            ulviDescription = ulviButton.transform.Find("Description").GetComponent<TextMeshProUGUI>();
            ulviName = ulviButton.transform.Find("Name").GetComponent<TextMeshProUGUI>();

            adrielButton = canvas.Find("AdrielButton").GetComponent<Button>();
            adrielButton.onClick.AddListener(AdrielButtonClick);
            adrielGirl = adrielButton.transform.Find("Girl").GetComponent<Image>();
            adrielBuff = adrielButton.transform.Find("Buff").GetComponent<Image>();
            adrielDescription = adrielButton.transform.Find("Description").GetComponent<TextMeshProUGUI>();
            adrielName = adrielButton.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            adrielNotActive = adrielButton.transform.Find("NotActive");

            ingieButton = canvas.Find("IngieButton").GetComponent<Button>();
            ingieButton.onClick.AddListener(IngieButtonClick);
            ingieName = ingieButton.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            ingieNotActive = ingieButton.transform.Find("NotActive");

            fayeButton = canvas.Find("FayeButton").GetComponent<Button>();
            fayeButton.onClick.AddListener(FayeButtonClick);
            fayeName = fayeButton.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            fayeNotActive = fayeButton.transform.Find("NotActive");

            liliButton = canvas.Find("LiliButton").GetComponent<Button>();
            liliButton.onClick.AddListener(LiliButtonClick);
            liliName = liliButton.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            liliNotActive = liliButton.transform.Find("NotActive");

            battleGirlsButton = canvas.Find("BattleGirlsButton").GetComponent<Button>();
            battleGirlsButton.onClick.AddListener(BattleGirlsButtonClick);
            battleGirlsGirl = battleGirlsButton.transform.Find("Girl").GetComponent<Image>();
            battleGirlsTitle = battleGirlsButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();
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

        protected virtual void UlviButtonClick()
        {
            UIManager.ShowScreen<GirlScreen>();
            GameGlobalStates.haremGirlNameSelected = ulviName.text;
        }

        protected virtual void AdrielButtonClick()
        {
            // UIManager.ShowScreen<GirlScreen>();
        }

        protected virtual void IngieButtonClick()
        {

        }

        protected virtual void FayeButtonClick()
        {

        }

        protected virtual void LiliButtonClick()
        {

        }

        protected virtual void BattleGirlsButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<TeamEditScreen>();
        }
    }
}