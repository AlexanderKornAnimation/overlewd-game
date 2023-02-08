using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSDialogScreen
    {
        public class CharacterRight : CharacterBase
        {
            
            public static CharacterRight GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<CharacterRight>
                    ("Prefabs/UI/Screens/DialogScreen/CharacterRight", parent);
            }
        }
    }
}
