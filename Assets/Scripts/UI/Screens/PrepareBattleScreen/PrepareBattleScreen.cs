using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class PrepareBattleScreen : BaseScreen
    {
        void Start()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/PrepareBattleScreen/PrepareBattle"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            screenRectTransform.Find("Canvas").Find("BackButton").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<EventMapScreen>();
            });

            screenRectTransform.Find("Canvas").Find("BattleButton").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<BattleScreen>();
            });

            screenRectTransform.Find("Canvas").Find("PrepareBattle").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowOverlay<BottlesOverlay>();
            });
        }

        void Update()
        {

        }
    }

}
