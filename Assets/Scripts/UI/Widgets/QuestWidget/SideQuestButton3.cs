using UnityEngine;

namespace Overlewd
{
    public class SideQuestButton3 : BaseQuestButton
    {
        public static SideQuestButton3 GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateWidgetPrefab<SideQuestButton3>("Prefabs/UI/Widgets/QuestsWidget/SideQuest3", parent);
        }
    }
}
