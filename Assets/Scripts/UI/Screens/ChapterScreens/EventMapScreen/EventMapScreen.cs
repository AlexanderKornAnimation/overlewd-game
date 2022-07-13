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
            QuestsWidget.GetInstance(transform);
            buffPanel = BuffWidget.GetInstance(transform);

            var canvas = screenInst.transform.Find("Canvas");
            map = canvas.Find("Map");
            background = map.Find("Background").GetComponent<Image>();

            sidebarButton = canvas.Find("SidebarButton").GetComponent<Button>();
            sidebarButton.onClick.AddListener(SidebarButtonClick);

            chapterSelectorButton = canvas.Find("ChapterSelectorButton").GetComponent<Button>();
            chapterSelectorButton.onClick.AddListener(ChapterSelectorButtonClick);
            chapterSelectorButtonName = chapterSelectorButton.transform.Find("ChapterName").GetComponent<TextMeshProUGUI>();
            chapterSelectroMarkers = chapterSelectorButton.transform.Find("Markers").GetComponent<TextMeshProUGUI>();
        }

        public override async Task BeforeShowMakeAsync()
        {
            var eventChapterData = GameData.events.mapEventData.activeChapter;
            if (eventChapterData == null)
            {
                return;
            }

            background.sprite = ResourceManager.LoadSprite(eventChapterData.mapImgUrl);

            foreach (var stageData in eventChapterData.stagesData)
            {
                if (stageData.isClosed)
                {
                    //continue;
                }

                if (stageData.battleId.HasValue)
                {
                    var battleData = stageData.battleData;
                    if (battleData.isTypeBattle)
                    {
                        var fightButton = NSEventMapScreen.FightButton.GetInstance(map);
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

            foreach (var eventMarketData in GameData.events.mapEventData.marketsData)
            {
                var shopButton = NSEventMapScreen.EventShopButton.GetInstance(map);
                shopButton.eventMarketId = eventMarketData.id;
                shopButton.transform.localPosition = eventMarketData.mapPos.pos;
            }

            chapterSelector = NSEventMapScreen.ChapterSelector.GetInstance(transform);
            chapterSelector.Hide();
            chapterSelectorButtonName.text = eventChapterData.name;

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
                        }).RunShowPopupProcess();
                }
                else if (battleData.isTypeBoss)
                {
                    UIManager.MakePopup<PrepareBossFightPopup>().
                        SetData(new PrepareBossFightPopupInData
                        {
                            eventStageId = inputData.eventStageId
                        }).RunShowPopupProcess();
                }
            }

            foreach (var stage in newStages)
            {
                stage.gameObject.SetActive(true);
            }
            
            await Task.CompletedTask;
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