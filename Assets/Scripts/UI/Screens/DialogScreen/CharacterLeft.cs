using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSDialogScreen
    {
        public class CharacterLeft : CharacterBase
        {
            
            public static CharacterLeft GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<CharacterLeft>
                    ("Prefabs/UI/Screens/DialogScreen/CharacterLeft", parent);
            }
        }
    }
}
