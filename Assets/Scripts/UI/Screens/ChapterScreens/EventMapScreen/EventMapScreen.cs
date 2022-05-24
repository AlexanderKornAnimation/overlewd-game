using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class EventMapScreen : BaseFullScreen
    {
        private List<NSEventMapScreen.BaseStageButton> newStages = new List<NSEventMapScreen.BaseStageButton>();

        private Transform map;
        private GameObject chapterMap;

        private Button sidebarButton;

        private NSEventMapScreen.MapButton mapButton;
        private BuffWidget buffPanel;

        private EventMapScreenInData inputData = new EventMapScreenInData();

        private void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/ChapterScreens/EventMapScreen/EventMap", transform);

            EventsWidget.GetInstance(transform);
            QuestsWidget.GetInstance(transform);
            buffPanel = BuffWidget.GetInstance(transform);
            buffPanel.inputData = inputData;

            var canvas = screenInst.transform.Find("Canvas");
            map = canvas.Find("Map");

            sidebarButton = canvas.Find("SidebarButton").GetComponent<Button>();
            sidebarButton.onClick.AddListener(SidebarButtonClick);
        }

        public EventMapScreen SetData(EventMapScreenInData data)
        {
            inputData = data;
            return this;
        }

        public override async Task BeforeShowMakeAsync()
        {
            var eventChapterData = GetActiveChapter(GameGlobalStates.eventData);
            if (eventChapterData == null)
            {
                return;
            }

            if (!eventChapterData.chapterMapId.HasValue)
            {
                return;
            }
            var mapData = GameData.GetChapterMapById(eventChapterData.chapterMapId.Value);
            chapterMap = ResourceManager.InstantiateRemoteAsset<GameObject>(mapData.chapterMapPath, mapData.assetBundleId, map);

            mapButton = NSEventMapScreen.MapButton.GetInstance(chapterMap.transform.Find("eventMap"));

            foreach (var stageId in eventChapterData.stages)
            {
                var stageData = GameData.GetEventStageById(stageId);

                if (stageData.isClosed)
                {
                    //continue;
                }

                var mapNode = chapterMap.transform.Find(stageData.mapNodeName ?? "");
                if (mapNode == null)
                {
                    continue;
                }

                if (stageData.battleId.HasValue)
                {
                    var battleData = stageData.battleData;
                    if (battleData.isTypeBattle)
                    {
                        var fightButton = NSEventMapScreen.FightButton.GetInstance(mapNode);
                        fightButton.screenInData = inputData;
                        fightButton.stageId = stageId;

                        if (!stageData.isComplete)
                        {
                            newStages.Add(fightButton);
                            fightButton.gameObject.SetActive(false);
                        }
                    }
                    else if (battleData.isTypeBoss)
                    {
                        var bossFightButton = NSEventMapScreen.FightButton.GetInstance(mapNode);
                        bossFightButton.stageId = stageId;
                        bossFightButton.screenInData = inputData;
                        
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
                        dialogButton.stageId = stageId;
                        
                        if (!stageData.isComplete)
                        {
                            newStages.Add(dialogButton);
                            dialogButton.gameObject.SetActive(false);
                        }
                    }
                    else if (dialogData.isTypeSex)
                    {
                        var sexButton = NSEventMapScreen.SexButton.GetInstance(mapNode);
                        sexButton.stageId = stageId;
                        
                        if (!stageData.isComplete)
                        {
                            newStages.Add(sexButton);
                            sexButton.gameObject.SetActive(false);
                        }
                    }
                }
            }

            foreach (var eventMarketId in GameGlobalStates.eventData.markets)
            {
                var eventMarketData = GameData.GetEventMarketById(eventMarketId);
                var mapNode = chapterMap.transform.Find(eventMarketData.eventMapNodeName);
                if (mapNode != null)
                {
                    var shopButton = NSEventMapScreen.EventShopButton.GetInstance(mapNode);
                    shopButton.eventMarketId = eventMarketData.id;
                }
            }

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

        private void SidebarButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowOverlay<SidebarMenuOverlay>();
        }

        private AdminBRO.EventChapter GetActiveChapter(AdminBRO.EventItem eventData)
        {
            AdminBRO.EventChapter firstChapter = null;
            foreach (var chapterId in eventData.chapters)
            {
                var findAsNextChapter = eventData.chapters.Exists(chId => 
                {
                    var chData = GameData.GetEventChapterById(chId);
                    return chData.nextChapterId.HasValue ?
                        chData.nextChapterId.Value == chapterId :
                        false;
                });
                firstChapter = findAsNextChapter ? firstChapter : GameData.GetEventChapterById(chapterId);
            }

            var curChapter = firstChapter;
            while (curChapter != null)
            {
                foreach (var stageId in curChapter.stages)
                {
                    var stageData = GameData.GetEventStageById(stageId);
                    if (!stageData.isComplete)
                    {
                        return curChapter;
                    }
                }

                if (curChapter.nextChapterId.HasValue)
                {
                    curChapter = GameData.GetEventChapterById(curChapter.nextChapterId.Value);
                    continue;
                }
                return curChapter;
            }
            return null;
        }
    }

    public class EventMapScreenInData : BaseScreenInData
    {

    }
}