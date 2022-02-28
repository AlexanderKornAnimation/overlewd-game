using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Resharper disable All

namespace Overlewd
{
    namespace FTUE
    {
        namespace NSMapScreen
        {
            public class SexSceneButton : Overlewd.NSMapScreen.SexSceneButton
            {
                private string[] sexNames = {
                    "empty",
                    "empty",
                    "Harder, Stronger",
                    "Lustful Memories"
                };

                public int stageId { get; set; }
                public int sexId { get; set; }

                private void Customize()
                {
                    gameObject.SetActive(stageId <= GameGlobalStates.currentStageId);

                    title.text = sexNames[sexId];
                    markers.SetActive(false);
                    sceneDone.gameObject.SetActive(GameGlobalStates.currentStageId > stageId);
                    button.interactable = GameGlobalStates.currentStageId == stageId;
                }

                private void Start()
                {
                    Customize();
                }

                protected override void ButtonClick()
                {
                    if (sexId == 2)
                    {

                    }
                    else if (sexId == 3)
                    {
                        SoundManager.PlayOneShoot(SoundPath.UI_GenericButtonClick);
                        UIManager.ShowScreen<CastleScreen>();
                    }
                }

                public new static SexSceneButton GetInstance(Transform parent)
                {
                    return ResourceManager.InstantiateWidgetPrefab<SexSceneButton>
                        ("Prefabs/UI/Screens/MapScreen/SexSceneButton", parent);
                }
            }
        }
    }
}