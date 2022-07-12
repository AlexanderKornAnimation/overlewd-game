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

        private GameObject chapterMap;

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

            var mapData = GameData.chapterMaps.GetById(eventChapterData.chapterMapId);
            if (mapData != null)
            {
                chapterMap = ResourceManager.InstantiateRemoteAsset<GameObject>(mapData.chapterMapPath, mapData.assetBundleId, map);
                background.gameObject.SetActive(false);
            }
            else
            {
                background.sprite = ResourceManager.LoadSprite(eventChapterData.mapImgUrl);
            }


            foreach (var stageData in eventChapterData.stagesData)
            {
                if (stageData.isClosed)
                {
                    //continue;
                }

                var mapNode = chapterMap?.transform.Find(stageData.mapNodeName ?? "") ?? map;

                if (stageData.battleId.HasValue)
                {
                    var battleData = stageData.battleData;
                    if (battleData.isTypeBattle)
                    {
                        var fightButton = NSEventMapScreen.FightButton.GetInstance(mapNode);
                        fightButton.stageId = stageData.id;
                        fightButton.transform.localPosition = (chapterMap == null) ?
                            stageData.mapPos.pos : Vector2.zero;

                        if (!stageData.isComplete)
                        {
                            newStages.Add(fightButton);
                            fightButton.gameObject.SetActive(false);
                        }
                    }
                    else if (battleData.isTypeBoss)
                    {
                        var bossFightButton = NSEventMapScreen.FightButton.GetInstance(mapNode);
                        bossFightButton.stageId = stageData.id;
                        bossFightButton.transform.localPosition = (chapterMap == null) ?
                            stageData.mapPos.pos : Vector2.zero;

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
                        var dialogButton = NSEventMapScreen.DialogButton.GetInstance(mapNode);
                        dialogButton.stageId = stageData.id;
                        dialogButton.transform.localPosition = (chapterMap == null) ?
                            stageData.mapPos.pos : Vector2.zero;

                        if (!stageData.isComplete)
                        {
                            newStages.Add(dialogButton);
                            dialogButton.gameObject.SetActive(false);
                        }
                    }
                    else if (dialogData.isTypeSex)
                    {
                        var sexButton = NSEventMapScreen.SexButton.GetInstance(mapNode);
                        sexButton.stageId = stageData.id;
                        sexButton.transform.localPosition = (chapterMap == null) ?
                            stageData.mapPos.pos : Vector2.zero;

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
                var mapNode = chapterMap?.transform.Find(eventMarketData.eventMapNodeName ?? "") ?? map;
                var shopButton = NSEventMapScreen.EventShopButton.GetInstance(mapNode);
                shopButton.eventMarketId = eventMarketData.id;
                shopButton.transform.localPosition = (chapterMap == null) ?
                    eventMarketData.mapPos.pos : Vector2.zero;
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