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
            public class EventButton : Overlewd.NSMapScreen.EventButton
            {
           
                public new static EventButton GetInstance(Transform parent)
                {
                    var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/MapScreen/EventButton"), parent);
                    newItem.name = nameof(EventButton);

                    return newItem.AddComponent<EventButton>();
                }
            }
        }
    }
}