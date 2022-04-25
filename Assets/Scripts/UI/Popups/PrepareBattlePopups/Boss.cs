using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSPrepareBattlePopup
    {
        public class Boss : BaseCharacter
        {
            protected override void Start()
            {

            }

            protected override void Customize()
            {
                
            }

            public static Boss GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<Boss>("Prefabs/UI/Popups/PrepareBattlePopups/Boss",
                    parent);
            }
        }
    }
}
