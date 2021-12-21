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
                    "Overlord’s Inception",
                    "Hot&Unbothered"
                };

                public int stageId { get; set; }
                public int dialogId { get; set; }

                private void Customize()
                {
                    title.text = dialogNames[dialogId];
                    markers.SetActive(false);
                    dialogueDone.gameObject.SetActive(false);
                }

                private void Start()
                {
                    Customize();
                }

                protected override void ButtonClick()
                {
                    if (dialogId == 1)
                    {

                    }
                    else if (dialogId == 2)
                    {

                    }
                    else if (dialogId == 3)
                    {
                        
                    }
                }

                public new static DialogButton GetInstance(Transform parent)
                {
                    var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/MapScreen/DialogButton"), parent);
                    newItem.name = nameof(DialogButton);

                    return newItem.AddComponent<DialogButton>();
                }
            }
        }
    }
}
