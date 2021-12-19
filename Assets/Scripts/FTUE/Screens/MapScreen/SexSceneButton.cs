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
                protected override void ButtonClick()
                {
                    
                }

                public new static SexSceneButton GetInstance(Transform parent)
                {
                    var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/MapScreen/SexSceneButton"), parent);
                    newItem.name = nameof(SexSceneButton);

                    return newItem.AddComponent<SexSceneButton>();
                }
            }
        }
    }
}