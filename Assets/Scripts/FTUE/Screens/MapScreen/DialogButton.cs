using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
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
            public class DialogButton : Overlewd.NSMapScreen.DialogButton
            {
                private string[] dialogNames = {
                    "empty",
                    "Evil's Little Helper",
                    "Overlord's Inception",
                    "Hot&Unbothered"
                };

                public int stageId { get; set; }
                public int dialogId { get; set; }

                private void Customize()
                {
                    gameObject.SetActive(stageId <= GameGlobalStates.currentStageId);

                    title.text = dialogNames[dialogId];
                    markers.SetActive(false);
                    dialogueDone.gameObject.SetActive(GameGlobalStates.currentStageId > stageId);
                    button.interactable = GameGlobalStates.currentStageId == stageId;
                }

                private void Start()
                {
                    Customize();
                }

                protected override void ButtonClick()
                {
                    SoundManager.PlayOneShoot(SoundPath.UI.GenericButtonClick);
                    if (dialogId == 1)
                    {
                        GameGlobalStates.dialogScreen_StageId = stageId;
                        GameGlobalStates.dialogScreen_DialogId = dialogId;
                        UIManager.ShowScreen<DialogScreen>();
                    }
                    else if (dialogId == 2)
                    {
                        GameGlobalStates.dialogScreen_StageId = stageId;
                        GameGlobalStates.dialogScreen_DialogId = dialogId;
                        UIManager.ShowScreen<DialogScreen>();
                    }
                    else if (dialogId == 3)
                    {
                        GameGlobalStates.dialogScreen_StageId = stageId;
                        GameGlobalStates.dialogScreen_DialogId = dialogId;
                        UIManager.ShowScreen<DialogScreen>();
                    }
                }

                public new static DialogButton GetInstance(Transform parent)
                {
                    return ResourceManager.InstantiateWidgetPrefab<DialogButton>
                        ("Prefabs/UI/Screens/MapScreen/DialogButton", parent);
                }
            }
        }
    }
}
