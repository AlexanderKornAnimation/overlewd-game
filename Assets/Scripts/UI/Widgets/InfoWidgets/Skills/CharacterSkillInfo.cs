using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class CharacterSkillInfo : BaseSkillInfo
    {
        public static CharacterSkillInfo GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateWidgetPrefab<CharacterSkillInfo>(
                "Prefabs/UI/Widgets/InfoWidgets/CharacterSkillInfo", parent);
        }
    }
}