using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSPrepareBattlePopup
    {
        public class AllyCharacter : BaseCharacter
        {
            public static AllyCharacter GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<AllyCharacter>(
                    "Prefabs/UI/Popups/PrepareBattlePopups/AllyCharacter", parent);
            }
        }
    }
}