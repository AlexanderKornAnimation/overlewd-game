using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class EventMapScreen : BaseFullScreen
    { 
        private Transform map;
        private GameObject chapterMap;

        private Button backButton;

        private NSEventMapScreen.MapButton mapButton;
        private List<NSEventMapScreen.EventShopButton> shopButtons = new List<NSEventMapScreen.EventShopButton>();
        private List<NSEventMapScreen.FightButton> fightButtons = new List<NSEventMapScreen.FightButton>();
        private List<NSEventMapScreen.BossFightButton> bossFightButtons = new List<NSEventMapScreen.BossFightButton>();
        private List<NSEventMapScreen.DialogButton> dialogButtons = new List<NSEventMapScreen.DialogButton>();
        private List<NSEventMapScreen.SexButton> sexButtons = new List<NSEventMapScreen.SexButton>();

        private EventMapScreenInData inputData;

        private void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/EventMapScreen/EventMap", transform);

            EventsWidget.GetInstance(transform);
            QuestsWidget.GetInstance(transform);
            BuffWidget.GetInstance(transform);

            var canvas = screenInst.transform.Find("Canvas");
            map = canvas.Find("Map");

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);
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

                if (stageData.status == AdminBRO.EventStageItem.Status_Closed)
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
                    var battleData = GameData.GetBattleById(stageData.battleId.Value);
                    if (battleData.type == AdminBRO.Battle.Type_Battle)
                    {
                        var fightButton = NSEventMapScreen.FightButton.GetInstance(mapNode);
                        fightButton.stageId = stageId;
                        fightButtons.Add(fightButton);
                    }
                    else if (battleData.type == AdminBRO.Battle.Type_Boss)
                    {
                        var bossFightButton = NSEventMapScreen.BossFightButton.GetInstance(mapNode);
                        bossFightButton.stageId = stageId;
                        bossFightButtons.Add(bossFightButton);
                    }
                }
                else if (stageData.dialogId.HasValue)
                {
                    var dialogData = GameData.GetDialogById(stageData.dialogId.Value);
                    if (dialogData.type == AdminBRO.Dialog.Type_Dialog)
                    {
                        var dialogButton = NSEventMapScreen.DialogButton.GetInstance(mapNode);
                        dialogButton.stageId = stageId;
                        dialogButtons.Add(dialogButton);
                    }
                    else if (dialogData.type == AdminBRO.Dialog.Type_Sex)
                    {
                        var sexButton = NSEventMapScreen.SexButton.GetInstance(mapNode);
                        sexButton.stageId = stageId;
                        sexButtons.Add(sexButton);
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
                    shopButtons.Add(shopButton);
                }
            }

            await Task.CompletedTask;
        }

        public override async Task AfterShowAsync()
        {
            if (inputData != null)
            {
                var teamEditStageId = inputData.teamEditStageId;
                if (teamEditStageId.HasValue)
                {
                    var stageData = GameData.GetEventStageById(teamEditStageId.Value);
                    if (stageData != null)
                    {
                        if (stageData.battleId.HasValue)
                        {
                            var battleData = GameData.GetBattleById(stageData.battleId.Value);
                            if (battleData.type == AdminBRO.Battle.Type_Battle)
                            {
                                UIManager.MakePopup<PrepareBattlePopup>().
                                    SetData(new PrepareBattlePopupInData
                                    {
                                        eventStageId = teamEditStageId.Value
                                    }).RunShowPopupProcess();
                            }
                            else if (battleData.type == AdminBRO.Battle.Type_Boss)
                            {
                                UIManager.MakePopup<PrepareBossFightPopup>().
                                    SetData(new PrepareBossFightPopupInData
                                    {
                                        eventStageId = teamEditStageId.Value
                                    }).RunShowPopupProcess();
                            }
                        }
                    }
                }
            }

            await Task.CompletedTask;
        }
        
        private void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<CastleScreen>();
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
                    if (stageData.status != AdminBRO.EventStageItem.Status_Complete)
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

    public class EventMapScreenInData
    {
        public int? teamEditStageId;
    }
}