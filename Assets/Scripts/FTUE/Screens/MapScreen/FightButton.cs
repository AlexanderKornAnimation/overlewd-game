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
                    gameObject.SetActive(stageId <= GameGlobalStates.currentStageId);

                    title.text = battleNames[battleId];
                    markers.SetActive(false);
                    fightDone.gameObject.SetActive(GameGlobalStates.currentStageId > stageId);
                    button.interactable = GameGlobalStates.currentStageId == stageId;

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
                    SoundManager.PlayOneShoot(SoundManager.SoundPath.UI.ButtonClick);
                    if (battleId == 11)
                    {
                        GameGlobalStates.battleScreen_StageId = stageId;
                        GameGlobalStates.battleScreen_BattleId = battleId;
                        UIManager.ShowPopup<PrepareBossFightPopup>();
                    }
                    else
                    {
                        GameGlobalStates.battleScreen_StageId = stageId;
                        GameGlobalStates.battleScreen_BattleId = battleId;
                        if (GameGlobalStates.battleScreen_BattleId < 6)
                        {
                            UIManager.ShowScreen<BattleScreen>();
                        }
                        else
                        {
                            UIManager.ShowPopup<PrepareBattlePopup>();
                        }
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

