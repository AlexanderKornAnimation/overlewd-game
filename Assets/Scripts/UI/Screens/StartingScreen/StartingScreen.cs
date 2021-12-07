using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class StartingScreen : BaseScreen
    {
        void Awake()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/StartingScreen/StartingScreen"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            screenRectTransform.Find("Canvas").Find("FTUE").GetComponent<Button>().onClick.AddListener(() =>
            {
                FTUE.GameData.Initialization();

                FTUE.GameGlobalStates.sexScreen_DialogId = 2;
                UIManager.ShowScreen<FTUE.SexScreen>();
            });

            screenRectTransform.Find("Canvas").Find("Castle").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<CastleScreen>();
            });
        }
    }
}
