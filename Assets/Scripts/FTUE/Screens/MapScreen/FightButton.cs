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
                public string battleKey { get; set; }


                private void Customize()
                {
                    gameObject.SetActive(true);

                    title.text = "name";
                    markers.SetActive(false);
                    fightDone.gameObject.SetActive(true);
                    button.interactable = true;


                    //icon.SetActive(false);
                    //bossIcon.SetActive(true);
                }

                private void Start()
                {
                    Customize();
                }

                protected override void ButtonClick()
                {
                    SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                    if (battleKey == "key1")
                    {
                        GameGlobalStates.battleScreen_StageKey = battleKey;
                        UIManager.ShowPopup<PrepareBossFightPopup>();
                    }
                    else
                    {
                        GameGlobalStates.battleScreen_StageKey = battleKey;
                        UIManager.ShowScreen<BattleScreen>();
                    }
                }

                public new static FightButton GetInstance(Transform parent)
                {
                    return ResourceManager.InstantiateWidgetPrefab<FightButton>
                        ("Prefabs/UI/Screens/MapScreen/FightButton", parent);
                }
            }
        }
    }
}

