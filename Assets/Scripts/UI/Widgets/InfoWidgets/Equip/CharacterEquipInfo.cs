using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class CharacterEquipInfo : BaseEquipInfo
    {
        public static CharacterEquipInfo GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateWidgetPrefab<CharacterEquipInfo>(
                "Prefabs/UI/Widgets/InfoWidgets/CharacterEquipInfo", parent);
        }
    }
}
