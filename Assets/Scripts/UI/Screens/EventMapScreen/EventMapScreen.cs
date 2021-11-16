using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class EventMapScreen : BaseScreen
    { 
        private Transform stages;

        private Button backButton;

        private NSEventMapScreen.MapButton mapButton;
        private List<NSEventMapScreen.EventShopButton> shopButtons = new List<NSEventMapScreen.EventShopButton>();
        private List<NSEventMapScreen.FightButton> fightButtons = new List<NSEventMapScreen.FightButton>();
        private List<NSEventMapScreen.BossFightButton> bossFightButtons = new List<NSEventMapScreen.BossFightButton>();
        private List<NSEventMapScreen.DialogButton> dialogButtons = new List<NSEventMapScreen.DialogButton>();
        private List<NSEventMapScreen.SexButton> sexButtons = new List<NSEventMapScreen.SexButton>();

        void Start()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/EventMapScreen/EventMap"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            EventsWidget.CreateInstance(transform);
            QuestsWidget.CreateInstance(transform);
            SidebarButtonWidget.CreateInstance(transform);

            var canvas = screenRectTransform.Find("Canvas");
            stages = canvas.Find("Stages");

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);

            mapButton = NSEventMapScreen.MapButton.GetInstance(stages.Find("eventMap"));

            var eventData = GameGlobalStates.eventMapScreen_EventData;
            foreach (var stageId in eventData.stages)
            {
                var stageData = GameData.GetEventStageById(stageId);

                var node = stages.Find(stageData.eventMapNodeName ?? "");
                if (node != null)
                {
                    if (stageData.type == AdminBRO.EventStageType.Battle)
                    {
                        if (stageData.battleId.HasValue)
                        {
                            var battleData = GameData.GetBattleById(stageData.battleId.Value);
                            if (battleData.type == AdminBRO.BattleType.Battle)
                            {
                                var fightButton = NSEventMapScreen.FightButton.GetInstance(node);
                                fightButton.eventStageData = stageData;
                                fightButtons.Add(fightButton);
                            }
                            else if (battleData.type == AdminBRO.BattleType.Boss)
                            {
                                var bossFightButton = NSEventMapScreen.BossFightButton.GetInstance(node);
                                bossFightButton.eventStageData = stageData;
                                bossFightButtons.Add(bossFightButton);
                            }
                        }
                    }
                    else if (stageData.type == AdminBRO.EventStageType.Dialog)
                    {
                        if (stageData.dialogId.HasValue)
                        {
                            var dialogData = GameData.GetDialogById(stageData.dialogId.Value);
                            if (dialogData.type == AdminBRO.DialogType.Dialog)
                            {
                                var dialogButton = NSEventMapScreen.DialogButton.GetInstance(node);
                                dialogButton.eventStageData = stageData;
                                dialogButtons.Add(dialogButton);
                            }
                            else if (dialogData.type == AdminBRO.DialogType.Sex)
                            {
                                var sexButton = NSEventMapScreen.SexButton.GetInstance(node);
                                sexButton.eventStageData = stageData;
                                sexButtons.Add(sexButton);
                            }
                        }
                    }
                }
            }

            foreach (var eventMarketId in eventData.markets)
            {
                var eventMarketData = GameData.GetEventMarketById(eventMarketId);
                var node = stages.Find(eventMarketData.eventMapNodeName);
                if (node != null)
                {
                    var shopButton = NSEventMapScreen.EventShopButton.GetInstance(node);
                    shopButton.eventMarketData = eventMarketData;
                    shopButtons.Add(shopButton);
                }
            }            
        }
        
        private void BackButtonClick()
        {
            UIManager.ShowScreen<CastleScreen>();
        }

        void Update()
        {

        }
    }
}
