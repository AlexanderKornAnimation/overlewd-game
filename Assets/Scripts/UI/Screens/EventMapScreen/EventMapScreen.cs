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

        public override async Task BeforeShowAsync()
        {
            var eventData = GameGlobalStates.eventMapScreen_EventData;
            var eventChapterData = GameData.GetEventChapterById(eventData.chapters[0]);
            var mapData = GameData.GetChapterMapById(eventChapterData.chapterMapId.Value);
            chapterMap = ResourceManager.InstantiateRemoteAsset<GameObject>(mapData.chapterMapPath, mapData.assetBundleId, map);

            mapButton = NSEventMapScreen.MapButton.GetInstance(chapterMap.transform.Find("eventMap"));

            foreach (var stageId in eventChapterData.stages)
            {
                var stageData = GameData.GetEventStageById(stageId);

                if (stageData.status == AdminBRO.EventStageItem.Status_Closed)
                {
                    continue;
                }

                var node = chapterMap.transform.Find(stageData.mapNodeName ?? "");
                if (node != null)
                {
                    if (stageData.type == AdminBRO.EventStageItem.Type_Battle)
                    {
                        if (stageData.battleId.HasValue)
                        {
                            var battleData = GameData.GetBattleById(stageData.battleId.Value);
                            if (battleData.type == AdminBRO.Battle.Type_Battle)
                            {
                                var fightButton = NSEventMapScreen.FightButton.GetInstance(node);
                                fightButton.eventStageId = stageId;
                                fightButtons.Add(fightButton);
                            }
                            else if (battleData.type == AdminBRO.Battle.Type_Boss)
                            {
                                var bossFightButton = NSEventMapScreen.BossFightButton.GetInstance(node);
                                bossFightButton.eventStageId = stageId;
                                bossFightButtons.Add(bossFightButton);
                            }
                        }
                    }
                    else if (stageData.type == AdminBRO.EventStageItem.Type_Dialog)
                    {
                        if (stageData.dialogId.HasValue)
                        {
                            var dialogData = GameData.GetDialogById(stageData.dialogId.Value);
                            if (dialogData.type == AdminBRO.Dialog.Type_Dialog)
                            {
                                var dialogButton = NSEventMapScreen.DialogButton.GetInstance(node);
                                dialogButton.eventStageId = stageId;
                                dialogButtons.Add(dialogButton);
                            }
                            else if (dialogData.type == AdminBRO.Dialog.Type_Sex)
                            {
                                var sexButton = NSEventMapScreen.SexButton.GetInstance(node);
                                sexButton.eventStageId = stageId;
                                sexButtons.Add(sexButton);
                            }
                        }
                    }
                }
            }

            foreach (var eventMarketId in eventData.markets)
            {
                var eventMarketData = GameData.GetEventMarketById(eventMarketId);
                var node = chapterMap.transform.Find(eventMarketData.eventMapNodeName);
                if (node != null)
                {
                    var shopButton = NSEventMapScreen.EventShopButton.GetInstance(node);
                    shopButton.eventMarketId = eventMarketData.id;
                    shopButtons.Add(shopButton);
                }
            }

            await Task.CompletedTask;
        }
        
        private void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<CastleScreen>();
        }
    }
}