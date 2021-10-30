using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class EventMapScreen : BaseScreen
    { 
        private Transform map;

        void Start()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/EventMapScreen/EventMap"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            var canvas = screenRectTransform.Find("Canvas");
            map = canvas.Find("Map");

            canvas.Find("BackButton").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<CastleScreen>();
            });

            EventsWidget.CreateInstance(transform);
            QuestsWidget.CreateInstance(transform);
            SidebarButtonWidget.CreateInstance(transform);

            NSEventMapScreen.MapButton.GetInstance(map.Find("eventMap"));

            foreach (Transform node in GetSpawnNodes("eventShop_"))
            {
                NSEventMapScreen.EventShopButton.GetInstance(node);
            }

            foreach (Transform node in GetSpawnNodes("fight_"))
            {
                NSEventMapScreen.FightButton.GetInstance(node);
            }

            foreach (Transform node in GetSpawnNodes("bossFight_"))
            {
                NSEventMapScreen.BossFightButton.GetInstance(node);
            }
            
            foreach (Transform node in GetSpawnNodes("dialogue_"))
            {
                NSEventMapScreen.DialogeButton.GetInstance(node);
            }

            foreach (Transform node in GetSpawnNodes("sexDialogue_"))
            {
                NSEventMapScreen.SexButton.GetInstance(node);
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

        void Update()
        {

        }
    }
}
