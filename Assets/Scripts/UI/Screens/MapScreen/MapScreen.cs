using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class MapScreen : BaseScreen
    {
        void Start()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/MapScreen/Map"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            screenRectTransform.Find("Canvas").Find("Castle").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<CastleScreen>();
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
                UIManager.ShowPopup<PrepareBattlePopup>();
            });

            screenRectTransform.Find("Canvas").Find("Event1").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<EventMapScreen>();
            });

            screenRectTransform.Find("Canvas").Find("Event2").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<EventMapScreen>();
            });

            EventsWidget.CreateInstance(transform);
            QuestsWidget.CreateInstance(transform);
            SidebarButtonWidget.CreateInstance(transform);
        }

        void Update()
        {

        }
    }
}
