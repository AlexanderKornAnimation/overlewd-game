using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Overlewd
{
    namespace FTUE
    {
        namespace NSMapScreen
        {
            public class FightButton : Overlewd.NSMapScreen.FightButton
            {
                private string[] battleNames = { 
                    "empty",
                    "Unchained",
                    "Blast 'em",
                    "Operation Domination",
                    "Player and Slayer",
                    "Dog's Day",
                    "Lord of Ashes",
                    "Knucklesandwiches",
                    "Rule the World",
                    "Hack and Slash",
                    "Power is Power",
                    "Spoils of War",
                };

                public int stageId { get; set; }
                public int battleId { get; set; }

                private void Customize()
                {
                    title.text = battleNames[battleId];
                    markers.SetActive(false);
                    fightDone.gameObject.SetActive(false);

                    if (battleId == 11)
                    {
                        icon.SetActive(false);
                        bossIcon.SetActive(true);
                    }
                    else
                    {
                        icon.SetActive(true);
                        bossIcon.SetActive(false);
                    }
                }

                private void Start()
                {
                    Customize();
                }

                protected override void ButtonClick()
                {
                    if (battleId == 11)
                    {

                    }
                    else
                    {

                    }
                }

                public new static FightButton GetInstance(Transform parent)
                {
                    var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/MapScreen/FightButton"), parent);
                    newItem.name = nameof(FightButton);

                    return newItem.AddComponent<FightButton>();
                }
            }
        }
    }
}

