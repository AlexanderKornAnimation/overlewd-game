using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class EventMapScreen : BaseScreen
    {
        private GameObject screenPrefab;

        void Start()
        {
            screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/EventMapScreen/EventMap"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            screenRectTransform.Find("Canvas").Find("BackButton").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<CastleScreen>();
            });

            EventsWidget.CreateInstance(transform);
            QuestsWidget.CreateInstance(transform);
            SidebarButtonWidget.CreateInstance(transform);

            NSEventMapScreen.MapButton.GetInstance(screenRectTransform.Find("Canvas").Find("eventMap"));

            foreach (Transform node in GetAllNodes("eventShop_"))
            {
                NSEventMapScreen.EventShopButton.GetInstance(node);
            }

            foreach (Transform node in GetAllNodes("fight_"))
            {
                NSEventMapScreen.FightButton.GetInstance(node);
            }

            foreach (Transform node in GetAllNodes("bossFight_"))
            {
                NSEventMapScreen.BossFightButton.GetInstance(node);
            }

            foreach (Transform node in GetAllNodes("dialogue_"))
            {
                NSEventMapScreen.DialogeButton.GetInstance(node);
            }

            foreach (Transform node in GetAllNodes("sexDialogue_"))
            {
                NSEventMapScreen.SexButton.GetInstance(node);
            }
        }

        private IEnumerable GetAllNodes(string nodeName)
        {
            var nodeId = 1;
            var node = screenPrefab.transform.Find("Canvas").Find(nodeName + nodeId.ToString());
            while (node != null)
            {
                yield return node;
                nodeId++;
                node = screenPrefab.transform.Find("Canvas").Find(nodeName + nodeId.ToString());
            }
            yield break;
        }

        void Update()
        {

        }
    }
}
