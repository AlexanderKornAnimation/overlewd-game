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
                public int dialogId { get; set; }
                protected override void ButtonClick()
                {
                    if (dialogId == 3)
                    {
                        UIManager.ShowScreen<CastleScreen>();
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
