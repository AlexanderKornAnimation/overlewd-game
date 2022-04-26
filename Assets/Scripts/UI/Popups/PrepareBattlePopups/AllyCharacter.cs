using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSPrepareBattlePopup
    {
        public class AllyCharacter : BaseCharacter
        {
            protected override void SetClassIcon()
            {
                characterClass.text = AdminBRO.Character.GetMyClassMarker(characterData.characterClass);
            }

            public static AllyCharacter GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<AllyCharacter>(
                    "Prefabs/UI/Popups/PrepareBattlePopups/AllyCharacter", parent);
            }
        }
    }
}