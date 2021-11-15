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
            foreach (var stage in eventData.stages)
            {
                var node = stages.Find(stage.eventMapNodeName);
                if (node != null)
                {
                    switch (stage.type)
                    {
                        case AdminBRO.EventStageType.Battle:
                            var fightButton = NSEventMapScreen.FightButton.GetInstance(node);
                            fightButton.eventStageData = stage;
                            fightButtons.Add(fightButton);
                            break;
                        case AdminBRO.EventStageType.Boss:
                            var bossFightButton = NSEventMapScreen.BossFightButton.GetInstance(node);
                            bossFightButton.eventStageData = stage;
                            bossFightButtons.Add(bossFightButton);
                            break;
                        case AdminBRO.EventStageType.Dialog:
                            var dialogButton = NSEventMapScreen.DialogButton.GetInstance(node);
                            dialogButton.eventStageData = stage;
                            dialogButtons.Add(dialogButton);
                            break;
                        case AdminBRO.EventStageType.SexDialog:
                            var sexButton = NSEventMapScreen.SexButton.GetInstance(node);
                            sexButton.eventStageData = stage;
                            sexButtons.Add(sexButton);
                            break;
                    }
                }
            }

            foreach (var marketId in eventData.markets)
            {
                var marketData = GameData.GetMarketById(marketId);
                var node = stages.Find(marketData.eventMapNodeName);
                if (node != null)
                {
                    var shopButton = NSEventMapScreen.EventShopButton.GetInstance(node);
                    shopButton.marketData = marketData;
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
