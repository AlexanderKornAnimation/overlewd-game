using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class EventMapScreen : BaseScreen
    {
        void Start()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/EventMapScreen/EventMap"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            screenRectTransform.Find("Canvas").Find("GlobalMap").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<MapScreen>();
            });

            screenRectTransform.Find("Canvas").Find("Dialog").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<DialogScreen>();
            });

            screenRectTransform.Find("Canvas").Find("Sex").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<SexScreen>();
            });

            screenRectTransform.Find("Canvas").Find("PrepareBattle").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<PrepareBattleScreen>();
            });

            screenRectTransform.Find("Canvas").Find("PrepapreBossFight").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<PrepareBossFightScreen>();
            });


            gameObject.AddComponent<EventsWidget>();
            gameObject.AddComponent<QuestsWidget>();
            gameObject.AddComponent<SidebarButtonWidget>();
        }

        void Update()
        {

        }
    }
}
