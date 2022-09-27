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
            protected override void Customize()
            {
                base.Customize();
                levelBack.SetActive(characterData.characterClass != AdminBRO.Character.Class_Overlord);
            }

            public static AllyCharacter GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<AllyCharacter>(
                    "Prefabs/UI/Popups/PrepareBattlePopups/AllyCharacter", parent);
            }
        }
    }
}