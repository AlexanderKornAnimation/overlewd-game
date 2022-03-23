using UnityEngine;

namespace Overlewd
{
    public class SideQuestButton2 : BaseQuestButton
    {
        public static SideQuestButton2 GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateWidgetPrefab<SideQuestButton2>("Prefabs/UI/Widgets/QuestsWidget/SideQuest2", parent);
        }
    }
}
