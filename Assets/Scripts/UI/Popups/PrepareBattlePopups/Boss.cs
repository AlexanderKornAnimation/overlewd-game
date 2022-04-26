using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSPrepareBossFightPopup
    {
        public class Boss : NSPrepareBattlePopup.BaseCharacter
        {
            public static Boss GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<Boss>("Prefabs/UI/Popups/PrepareBattlePopups/Boss",
                    parent);
            }
        }
    }
}
