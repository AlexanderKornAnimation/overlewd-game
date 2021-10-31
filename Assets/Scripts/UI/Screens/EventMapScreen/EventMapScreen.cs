using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class EventMapScreen : BaseScreen
    { 
        private Transform map;

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
            map = canvas.Find("Map");

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);

            mapButton = NSEventMapScreen.MapButton.GetInstance(map.Find("eventMap"));

            var eventData = GameGlobalStates.eventMapScreen_EventData;
            var eventShopIds = eventData.markets;
            var battleStages = new List<AdminBRO.EventStageItem>();
            var bossFightStages = new List<AdminBRO.EventStageItem>();
            var dialogStages = new List<AdminBRO.EventStageItem>();
            var sexStages = new List<AdminBRO.EventStageItem>();
            foreach (var stage in eventData.stages)
            {
                switch (stage.type)
                {
                    case AdminBRO.EventStageType.Battle:
                        battleStages.Add(stage);
                        break;
                    case AdminBRO.EventStageType.Boss:
                        bossFightStages.Add(stage);
                        break;
                    case AdminBRO.EventStageType.Dialog:
                        dialogStages.Add(stage);
                        break;
                    case AdminBRO.EventStageType.SexDialog:
                        sexStages.Add(stage);
                        break;
                }

            }

            var eventShopId = 0;
            foreach (Transform node in GetSpawnNodes("eventShop_"))
            {
                if (eventShopId < eventShopIds.Count)
                {
                    var shopButton = NSEventMapScreen.EventShopButton.GetInstance(node);
                    shopButton.marketData = GameData.GetMarketById(eventShopIds[eventShopId]);
                    shopButtons.Add(shopButton);
                }
                eventShopId++;
            }

            var battleId = 0;
            foreach (Transform node in GetSpawnNodes("fight_"))
            {
                if (battleId < battleStages.Count)
                {
                    var fightButton = NSEventMapScreen.FightButton.GetInstance(node);
                    fightButton.eventStageData = battleStages[battleId];
                    fightButtons.Add(fightButton);
                }
                battleId++;
            }

            var bossFightId = 0;
            foreach (Transform node in GetSpawnNodes("bossFight_"))
            {
                if (bossFightId < bossFightStages.Count)
                {
                    var bossFightButton = NSEventMapScreen.BossFightButton.GetInstance(node);
                    bossFightButton.eventStageData = bossFightStages[bossFightId];
                    bossFightButtons.Add(bossFightButton);
                }
                bossFightId++;
            }

            var dialogId = 0;
            foreach (Transform node in GetSpawnNodes("dialogue_"))
            {
                if (dialogId < dialogStages.Count)
                {
                    var dialogButton = NSEventMapScreen.DialogButton.GetInstance(node);
                    dialogButton.eventStageData = dialogStages[dialogId];
                    dialogButtons.Add(dialogButton);
                }
                dialogId++;
            }

            var sexId = 0;
            foreach (Transform node in GetSpawnNodes("sexDialogue_"))
            {
                if (sexId < sexStages.Count)
                {
                    var sexButton = NSEventMapScreen.SexButton.GetInstance(node);
                    sexButton.eventStageData = sexStages[sexId];
                    sexButtons.Add(sexButton);
                }
                sexId++;
            }
        }

        private IEnumerable GetSpawnNodes(string nodeName)
        {
            var nodeId = 1;
            var node = map.Find(nodeName + nodeId.ToString());
            while (node != null)
            {
                yield return node;
                nodeId++;
                node = map.Find(nodeName + nodeId.ToString());
            }
            yield break;
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
