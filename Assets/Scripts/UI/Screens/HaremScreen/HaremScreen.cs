using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class HaremScreen : BaseFullScreenParent<HaremScreenInData>
    {
        private Button backButton;
        private TextMeshProUGUI backButtonText;

        private Button ulviButton;
        private TextMeshProUGUI ulviBuffDescription;
        private TextMeshProUGUI ulviName;

        private Button adrielButton;
        private Transform adrielActive;
        private TextMeshProUGUI adrielName;
        private TextMeshProUGUI adrielBuffDescription;
        private GameObject adrielNotActive;

        private Button ingieButton;
        private Transform ingieActive;
        private TextMeshProUGUI ingieName;
        private TextMeshProUGUI ingieBuffDescription;
        private GameObject ingieNotActive;

        private Button fayeButton;
        private Transform fayeActive;
        private TextMeshProUGUI fayeBuffDescription;
        private TextMeshProUGUI fayeName;
        private GameObject fayeNotActive;

        private Button liliButton;
        private Transform liliActive;
        private TextMeshProUGUI liliBuffDescription;
        private TextMeshProUGUI liliName;
        private GameObject liliNotActive;

        private Button battleGirlsButton;
        private Image battleGirlsGirl;
        private TextMeshProUGUI battleGirlsTitle;

        private void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/HaremScreen/Harem", transform);

            var canvas = screenInst.transform.Find("Canvas");

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButtonText = backButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            backButton.onClick.AddListener(BackButtonClick);

            ulviButton = canvas.Find("UlviButton").GetComponent<Button>();
            ulviButton.onClick.AddListener(UlviButtonClick);
            ulviBuffDescription = ulviButton.transform.Find("Description").GetComponent<TextMeshProUGUI>();
            ulviName = ulviButton.transform.Find("Name").GetComponent<TextMeshProUGUI>();

            adrielButton = canvas.Find("AdrielButton").GetComponent<Button>();
            adrielButton.onClick.AddListener(AdrielButtonClick);
            adrielActive = adrielButton.transform.Find("Active");
            adrielBuffDescription = adrielActive.Find("Description").GetComponent<TextMeshProUGUI>();
            adrielName = adrielButton.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            adrielNotActive = adrielButton.transform.Find("NotActive").gameObject;

            ingieButton = canvas.Find("IngieButton").GetComponent<Button>();
            ingieButton.onClick.AddListener(IngieButtonClick);
            ingieActive = ingieButton.transform.Find("Active");
            ingieBuffDescription = ingieActive.Find("Description").GetComponent<TextMeshProUGUI>();
            ingieName = ingieButton.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            ingieNotActive = ingieButton.transform.Find("NotActive").gameObject;

            fayeButton = canvas.Find("FayeButton").GetComponent<Button>();
            fayeButton.onClick.AddListener(FayeButtonClick);
            fayeActive = fayeButton.transform.Find("Active");
            fayeBuffDescription = fayeActive.Find("Description").GetComponent<TextMeshProUGUI>();
            fayeName = fayeButton.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            fayeNotActive = fayeButton.transform.Find("NotActive").gameObject;

            liliButton = canvas.Find("LiliButton").GetComponent<Button>();
            liliButton.onClick.AddListener(LiliButtonClick);
            liliActive = liliButton.transform.Find("Active");
            liliBuffDescription = liliActive.Find("Description").GetComponent<TextMeshProUGUI>();
            liliName = liliButton.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            liliNotActive = liliButton.transform.Find("NotActive").gameObject;

            battleGirlsButton = canvas.Find("BattleGirlsButton").GetComponent<Button>();
            battleGirlsButton.onClick.AddListener(BattleGirlsButtonClick);
            battleGirlsGirl = battleGirlsButton.transform.Find("Girl").GetComponent<Image>();
            battleGirlsTitle = battleGirlsButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();

            //disable matriach
            adrielActive.gameObject.SetActive(false);
            ingieActive.gameObject.SetActive(false);
            fayeActive.gameObject.SetActive(false);
            liliActive.gameObject.SetActive(false);
            
        }

        public override async Task BeforeShowMakeAsync()
        {
            if (inputData?.prevScreenInData != null)
            {
                if (inputData.prevScreenInData.IsType<MapScreenInData>() || inputData.prevScreenInData.IsType<EventMapScreenInData>())
                {
                    backButtonText.text = "Back to\nthe Map";
                }
            }
            
            await Task.CompletedTask;
        }

        private void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);

            if (inputData?.prevScreenInData != null)
            {
                if (inputData.prevScreenInData.IsType<MapScreenInData>())
                {
                    UIManager.MakeScreen<MapScreen>().
                        SetData(new MapScreenInData
                    {
                        ftueStageId = inputData.ftueStageId
                    }).RunShowScreenProcess();
                }
                else if (inputData.prevScreenInData.IsType<EventMapScreenInData>())
                {
                    UIManager.MakeScreen<EventMapScreen>().
                        SetData(new EventMapScreenInData
                    {
                        eventStageId = inputData.eventStageId
                    }).RunShowScreenProcess();
                }
                else
                {
                    UIManager.ShowScreen<CastleScreen>();
                }
            }
            else
            {
                UIManager.ShowScreen<CastleScreen>();
            }
        }

        private void UlviButtonClick()
        {
            if (inputData == null)
            {
                UIManager.ShowScreen<GirlScreen>();
            }
            else
            {
                UIManager.MakeScreen<GirlScreen>().
                    SetData(new GirlScreenInData
                    {
                        girlName = ulviName.text,
                        prevScreenInData = inputData,
                        ftueStageId = inputData.ftueStageId,
                        eventStageId = inputData.eventStageId
                    }).RunShowScreenProcess();
            }
        }

        private void AdrielButtonClick()
        {
            // UIManager.ShowScreen<GirlScreen>();
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
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            if (inputData == null)
            {
                UIManager.ShowScreen<TeamEditScreen>();
            }
            else
            {
                UIManager.MakeScreen<TeamEditScreen>().
                    SetData(new TeamEditScreenInData
                    {
                        prevScreenInData = inputData,
                        ftueStageId = inputData?.ftueStageId,
                        eventStageId = inputData?.eventStageId
                    }).RunShowScreenProcess();
            }
        }
    }

    public class HaremScreenInData : BaseFullScreenInData
    {

    }
}