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
        private Image ulviBuffIcon;
        private TextMeshProUGUI ulviBuffDescription;
        private GameObject ulviBuffActive;
        private TextMeshProUGUI ulviName;

        private Button adrielButton;
        private Image adrielBuffIcon;
        private Transform adrielActive;
        private TextMeshProUGUI adrielName;
        private TextMeshProUGUI adrielBuffDescription;
        private GameObject adrielBuffActive;
        private GameObject adrielNotActive;

        private Button ingieButton;
        private Image ingieBuffIcon;
        private Transform ingieActive;
        private TextMeshProUGUI ingieName;
        private TextMeshProUGUI ingieBuffDescription;
        private GameObject ingieBuffActive;
        private GameObject ingieNotActive;

        private Button fayeButton;
        private Image fayeBuffIcon;
        private Transform fayeActive;
        private TextMeshProUGUI fayeBuffDescription;
        private GameObject fayeBuffActive;
        private TextMeshProUGUI fayeName;
        private GameObject fayeNotActive;

        private Button liliButton;
        private Image liliBuffIcon;
        private Transform liliActive;
        private TextMeshProUGUI liliBuffDescription;
        private GameObject liliBuffActive;
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
            ulviBuffIcon = ulviButton.transform.Find("Buff").GetComponent<Image>();
            ulviBuffDescription = ulviButton.transform.Find("Description").GetComponent<TextMeshProUGUI>();
            ulviBuffActive = ulviButton.transform.Find("BuffActive").gameObject;
            ulviName = ulviButton.transform.Find("Name").GetComponent<TextMeshProUGUI>();

            adrielButton = canvas.Find("AdrielButton").GetComponent<Button>();
            adrielButton.onClick.AddListener(AdrielButtonClick);
            adrielActive = adrielButton.transform.Find("Active");
            adrielBuffIcon = adrielActive.Find("Buff").GetComponent<Image>();
            adrielBuffDescription = adrielActive.Find("Description").GetComponent<TextMeshProUGUI>();
            adrielBuffActive = adrielActive.Find("BuffActive").gameObject;
            adrielName = adrielButton.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            adrielNotActive = adrielButton.transform.Find("NotActive").gameObject;

            ingieButton = canvas.Find("IngieButton").GetComponent<Button>();
            ingieButton.onClick.AddListener(IngieButtonClick);
            ingieActive = ingieButton.transform.Find("Active");
            ingieBuffIcon = ingieActive.Find("Buff").GetComponent<Image>();
            ingieBuffDescription = ingieActive.Find("Description").GetComponent<TextMeshProUGUI>();
            ingieBuffActive = ingieActive.Find("BuffActive").gameObject;
            ingieName = ingieButton.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            ingieNotActive = ingieButton.transform.Find("NotActive").gameObject;

            fayeButton = canvas.Find("FayeButton").GetComponent<Button>();
            fayeButton.onClick.AddListener(FayeButtonClick);
            fayeActive = fayeButton.transform.Find("Active");
            fayeBuffIcon = fayeActive.Find("Buff").GetComponent<Image>();
            fayeBuffDescription = fayeActive.Find("Description").GetComponent<TextMeshProUGUI>();
            fayeBuffActive = fayeActive.Find("BuffActive").gameObject;
            fayeName = fayeButton.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            fayeNotActive = fayeButton.transform.Find("NotActive").gameObject;

            liliButton = canvas.Find("LiliButton").GetComponent<Button>();
            liliButton.onClick.AddListener(LiliButtonClick);
            liliActive = liliButton.transform.Find("Active");
            liliBuffIcon = liliActive.Find("Buff").GetComponent<Image>();
            liliBuffDescription = liliActive.Find("Description").GetComponent<TextMeshProUGUI>();
            liliBuffActive = liliActive.Find("BuffActive").gameObject;
            liliName = liliButton.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            liliNotActive = liliButton.transform.Find("NotActive").gameObject;

            battleGirlsButton = canvas.Find("BattleGirlsButton").GetComponent<Button>();
            battleGirlsButton.onClick.AddListener(BattleGirlsButtonClick);
            battleGirlsGirl = battleGirlsButton.transform.Find("Girl").GetComponent<Image>();
            battleGirlsTitle = battleGirlsButton.transform.Find("Title").GetComponent<TextMeshProUGUI>();            
        }

        public override async Task BeforeShowMakeAsync()
        {
            ulviBuffIcon.sprite = ResourceManager.LoadSprite(GameData.matriarchs.Ulvi.buff?.icon);
            ulviBuffActive.SetActive(GameData.matriarchs.Ulvi.buff?.active ?? false);
            ulviBuffDescription.text = 
                UITools.ChangeTextSize(GameData.matriarchs.Ulvi.buff?.description, ulviBuffDescription.fontSize);

            adrielActive.gameObject.SetActive(GameData.matriarchs.Adriel.isOpen);
            adrielNotActive.gameObject.SetActive(!GameData.matriarchs.Adriel.isOpen);
            adrielButton.interactable = GameData.matriarchs.Adriel.isOpen;
            adrielBuffIcon.sprite = ResourceManager.LoadSprite(GameData.matriarchs.Adriel.buff?.icon);
            adrielBuffActive.SetActive(GameData.matriarchs.Adriel.buff?.active ?? false);
            adrielBuffDescription.text = 
                UITools.ChangeTextSize(GameData.matriarchs.Adriel.buff?.description, adrielBuffDescription.fontSize);

            ingieActive.gameObject.SetActive(GameData.matriarchs.Ingie.isOpen);
            ingieNotActive.gameObject.SetActive(!GameData.matriarchs.Ingie.isOpen);
            ingieButton.interactable = GameData.matriarchs.Ingie.isOpen;
            ingieBuffIcon.sprite = ResourceManager.LoadSprite(GameData.matriarchs.Ingie.buff?.icon);
            ingieBuffActive.SetActive(GameData.matriarchs.Ingie.buff?.active ?? false);
            ingieBuffDescription.text = 
                UITools.ChangeTextSize(GameData.matriarchs.Ingie.buff?.description, ingieBuffDescription.fontSize);

            fayeActive.gameObject.SetActive(GameData.matriarchs.Faye.isOpen);
            fayeNotActive.gameObject.SetActive(!GameData.matriarchs.Faye.isOpen);
            fayeButton.interactable = GameData.matriarchs.Faye.isOpen;
            fayeBuffIcon.sprite = ResourceManager.LoadSprite(GameData.matriarchs.Faye.buff?.icon);
            fayeBuffActive.SetActive(GameData.matriarchs.Faye.buff?.active ?? false);
            fayeBuffDescription.text =
                UITools.ChangeTextSize(GameData.matriarchs.Faye.buff?.description, fayeBuffDescription.fontSize);

            liliActive.gameObject.SetActive(GameData.matriarchs.Lili.isOpen);
            liliNotActive.gameObject.SetActive(!GameData.matriarchs.Lili.isOpen);
            liliButton.interactable = GameData.matriarchs.Lili.isOpen;
            liliBuffIcon.sprite = ResourceManager.LoadSprite(GameData.matriarchs.Lili.buff?.icon);
            liliBuffActive.SetActive(GameData.matriarchs.Lili.buff?.active ?? false);
            liliBuffDescription.text =
                UITools.ChangeTextSize(GameData.matriarchs.Lili.buff?.description, liliBuffDescription.fontSize);

            if (inputData?.prevScreenInData != null)
            {
                if (inputData.prevScreenInData.IsType<MapScreenInData>() || inputData.prevScreenInData.IsType<EventMapScreenInData>())
                {
                    backButtonText.text = "Back to\nthe Map";
                }
            }
            
            await Task.CompletedTask;
        }

        public override async Task AfterShowAsync()
        {
            GameData.ftue.DoLern(
                GameData.ftue.stats.lastEndedStageData,
                new FTUELernActions
                {
                    ch1_any = () => SoundManager.PlayOneShot(FMODEventPath.VO_Ulvi_Reactions_harem),
                    ch2_any = () => SoundManager.PlayOneShot(FMODEventPath.VO_Adriel_Reactions_harem),
                    ch3_any = () => SoundManager.PlayOneShot(FMODEventPath.VO_Ingie_Reactions_harem)
                });

            await Task.CompletedTask;
        }

        public override async Task BeforeShowAsync()
        {
            SoundManager.GetEventInstance(FMODEventPath.Castle_Screen_BGM_Attn);
            await Task.CompletedTask;
        }

        public override async Task BeforeHideAsync()
        {
            SoundManager.StopAll();
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
                    }).DoShow();
                }
                else if (inputData.prevScreenInData.IsType<EventMapScreenInData>())
                {
                    UIManager.MakeScreen<EventMapScreen>().
                        SetData(new EventMapScreenInData
                    {
                        eventStageId = inputData.eventStageId
                    }).DoShow();
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

        private void GirlButtonClick(string girlKey)
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
                        girlKey = girlKey,
                        prevScreenInData = inputData,
                        ftueStageId = inputData.ftueStageId,
                        eventStageId = inputData.eventStageId
                    }).DoShow();
            }
        }

        private void UlviButtonClick()
        {
            GirlButtonClick(AdminBRO.MatriarchItem.Key_Ulvi);
        }

        private void AdrielButtonClick()
        {
            GirlButtonClick(AdminBRO.MatriarchItem.Key_Adriel);
        }

        private void IngieButtonClick()
        {
            GirlButtonClick(AdminBRO.MatriarchItem.Key_Ingie);
        }

        private void FayeButtonClick()
        {
            GirlButtonClick(AdminBRO.MatriarchItem.Key_Faye);
        }

        private void LiliButtonClick()
        {
            GirlButtonClick(AdminBRO.MatriarchItem.Key_Lili);
        }

        private void BattleGirlsButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            if (inputData == null)
            {
                UIManager.ShowScreen<BattleGirlListScreen>();
            }
            else
            {
                UIManager.MakeScreen<BattleGirlListScreen>().
                    SetData(new BattleGirlListScreenInData
                    {
                        prevScreenInData = inputData,
                        ftueStageId = inputData?.ftueStageId,
                        eventStageId = inputData?.eventStageId
                    }).DoShow();
            }
        }
    }

    public class HaremScreenInData : BaseFullScreenInData
    {

    }
}