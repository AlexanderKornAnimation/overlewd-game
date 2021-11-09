using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//Resharper disable All

namespace Overlewd
{
    namespace NSMapScreen
    {
        public class FightButton : MonoBehaviour
        {
            private Button button;
            private Transform fightDone;
            private Text title;
            
            private void Start()
            {
                var canvas = transform.Find("Canvas");

                button = canvas.Find("Button").GetComponent<Button>();
                title = button.transform.Find("Title").GetComponent<Text>();
                fightDone = button.transform.Find("FightDone");

                button.onClick.AddListener(ButtonClick);
            }

            private void ButtonClick()
            {
                UIManager.ShowScreen<PrepareBattlePopup>();
            }

            public static FightButton GetInstance(Transform parent)
            {
                var newItem = (GameObject) Instantiate(Resources.Load("Prefabs/UI/Screens/MapScreen/FightButton"), parent);
                newItem.name = nameof(FightButton);
                
                return newItem.AddComponent<FightButton>();
            }
        }
    }
}

