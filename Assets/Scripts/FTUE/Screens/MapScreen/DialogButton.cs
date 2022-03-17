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
                public string dialogKey { get; set; }

                private void Customize()
                {
                    gameObject.SetActive(true);

                    title.text = "name";
                    dialogueDone.gameObject.SetActive(true);
                    button.interactable = true;
                }

                private void Start()
                {
                    Customize();
                }

                protected override void ButtonClick()
                {
                    SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);

                    GameGlobalStates.dialogScreen_StageKey = dialogKey;
                    UIManager.ShowScreen<DialogScreen>();
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
