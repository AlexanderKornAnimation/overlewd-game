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

