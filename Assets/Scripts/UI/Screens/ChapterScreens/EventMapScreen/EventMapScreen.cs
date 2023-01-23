using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Overlewd
{
    public class EventMapScreen : BaseFullScreenParent<EventMapScreenInData>
    {
        private List<NSEventMapScreen.BaseStageButton> newStages = new List<NSEventMapScreen.BaseStageButton>();

        private Transform map;
        private Image background;
        private Image banner;
        private Button bannerButton;
        private TextMeshProUGUI timer;

        private Button sidebarButton;

        private BuffWidget buffPanel;
        private Button chapterSelectorButton;
        private TextMeshProUGUI chapterSelectorButtonName;
        private TextMeshProUGUI chapterSelectroMarkers;

        private NSEventMapScreen.ChapterSelector chapterSelector;

        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/ChapterScreens/EventMapScreen/EventMap", transform);

            EventsWidget.GetInstance(transform);
            var questPanel = QuestsWidget.GetInstance(transform);
            questPanel.SwitchToEventMode();
            buffPanel = BuffWidget.GetInstance(transform);

            var canvas = screenInst.transform.Find("Canvas");
            map = canvas.Find("Map");
            background = map.Find("Background").GetComponent<Image>();
            banner = canvas.Find("Banner").GetComponent<Image>();
            bannerButton = banner.GetComponent<Button>();
            bannerButton.onClick.AddListener(BannerButtonClick);
            timer = banner.transform.Find("TimerBack").Find("Timer").GetComponent<TextMeshProUGUI>();

            sidebarButton = canvas.Find("SidebarButton").GetComponent<Button>();
            sidebarButton.onClick.AddListener(SidebarButtonClick);

            chapterSelectorButton = canvas.Find("ChapterSelectorButton").GetComponent<Button>();
            chapterSelectorButton.onClick.AddListener(ChapterSelectorButtonClick);
            chapterSelectorButtonName = chapterSelectorButton.transform.Find("ChapterName").GetComponent<TextMeshProUGUI>();
            chapterSelectroMarkers = chapterSelectorButton.transform.Find("Markers").GetComponent<TextMeshProUGUI>();
        }

        public override async Task BeforeShowMakeAsync()
        {
            var eventData = GameData.events.mapEventData;

            if (GameData.events.mapChapter == null)
            {
                if (GameData.devMode)
                    eventData?.firstChapter.SetAsMapChapter();
                else
                    eventData?.activeChapter.SetAsMapChapter();
            }

            var eventChapterData = GameData.events.mapChapter;
            if (eventChapterData == null)
            {
                return;
            }

            background.sprite = ResourceManager.LoadSprite(eventChapterData.mapImgUrl);
            banner.sprite = ResourceManager.LoadSprite(eventData.mapBannerImage);

            foreach (var stageData in eventChapterData.stagesData)
            {
                var instantiateStageOnMap = GameData.devMode ? true : !stageData.isClosed;
                if (!instantiateStageOnMap)
                {
                    continue;
                }

                if (stageData.battleId.HasValue)
                {
                    var battleData = stageData.battleData;
                    if (battleData.isTypeBattle)
                    {
                        var fightButton = NSEventMapScreen.FightButton.GetInstance(map);
                        fightButton.eventId = eventData.id;
                        fightButton.stageId = stageData.id;
                        fightButton.transform.localPosition = stageData.mapPos.pos;

                        if (!stageData.isComplete)
                        {
                            newStages.Add(fightButton);
                            fightButton.gameObject.SetActive(false);
                        }
                    }
                    else if (battleData.isTypeBoss)
                    {
                        var bossFightButton = NSEventMapScreen.FightButton.GetInstance(map);
                        bossFightButton.eventId = eventData.id;
                        bossFightButton.stageId = stageData.id;
                        bossFightButton.transform.localPosition = stageData.mapPos.pos;

                        if (!stageData.isComplete)
                        {
                            newStages.Add(bossFightButton);
                            bossFightButton.gameObject.SetActive(false);
                        }
                    }
                }
                else if (stageData.dialogId.HasValue)
                {
                    var dialogData = stageData.dialogData;
                    if (dialogData.isTypeDialog)
                    {
                        var dialogButton = NSEventMapScreen.DialogButton.GetInstance(map);
                        dialogButton.eventId = eventData.id;
                        dialogButton.stageId = stageData.id;
                        dialogButton.transform.localPosition = stageData.mapPos.pos;

                        if (!stageData.isComplete)
                        {
                            newStages.Add(dialogButton);
                            dialogButton.gameObject.SetActive(false);
                        }
                    }
                    else if (dialogData.isTypeSex)
                    {
                        var sexButton = NSEventMapScreen.SexButton.GetInstance(map);
                        sexButton.eventId = eventData.id;
                        sexButton.stageId = stageData.id;
                        sexButton.transform.localPosition = stageData.mapPos.pos;

                        if (!stageData.isComplete)
                        {
                            newStages.Add(sexButton);
                            sexButton.gameObject.SetActive(false);
                        }
                    }
                }
            }

            foreach (var market in eventChapterData.markets)
            {
                var marketData = market.marketData;
                if (marketData != null)
                {
                    var shopButton = NSEventMapScreen.EventShopButton.GetInstance(map);
                    shopButton.eventId = eventData.id;
                    shopButton.marketId = market.marketId;
                    shopButton.transform.localPosition = market.mapPos.pos;
                }
            }
            
            if (GameData.events.mapChapter.isComplete && GameData.events.mapChapter.nextChapterId.HasValue)
            {
                var button = NSEventMapScreen.ButtonNextChapter.GetInstance(map);
                button.transform.localPosition = GameData.events.mapChapter.nextChapterMapPos.pos;
                button.chapterId = GameData.events.mapChapter.nextChapterId;
            }
            
            chapterSelector = NSEventMapScreen.ChapterSelector.GetInstance(transform);
            chapterSelector.Hide();
            chapterSelectorButtonName.text = eventChapterData.name;

            DevWidget.GetInstance(transform);

            await Task.CompletedTask;
        }

        public override async Task AfterShowAsync()
        {
            var battleData = inputData?.eventStageData?.battleData;
            if (battleData != null)
            {
                if (battleData.isTypeBattle)
                {
                    UIManager.MakePopup<PrepareBattlePopup>().
                        SetData(new PrepareBattlePopupInData
                        {
                            eventStageId = inputData.eventStageId
                        }).DoShow();
                }
                else if (battleData.isTypeBoss)
                {
                    UIManager.MakePopup<PrepareBossFightPopup>().
                        SetData(new PrepareBossFightPopupInData
                        {
                            eventStageId = inputData.eventStageId
                        }).DoShow();
                }
            }

            foreach (var stage in newStages)
            {
                stage.gameObject.SetActive(true);
                stage.transform.SetAsLastSibling();
            }
            
            await Task.CompletedTask;
        }

        private void BannerButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowOverlay<EventMarketOverlay>();
        }
        
        private void ChapterSelectorButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            chapterSelector.Show();
        }

        private void SidebarButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowOverlay<SidebarMenuOverlay>();
        }
    }

    public class EventMapScreenInData : BaseFullScreenInData
    {

    }
}