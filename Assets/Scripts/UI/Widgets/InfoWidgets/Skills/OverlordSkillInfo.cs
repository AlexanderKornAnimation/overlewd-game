using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class OverlordSkillInfo : BaseSkillInfo
    {
        public static OverlordSkillInfo GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateWidgetPrefab<OverlordSkillInfo>(
                "Prefabs/UI/Widgets/InfoWidgets/OverlordSkillInfo", parent);
        }
    }
}
