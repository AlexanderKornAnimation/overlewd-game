using UnityEngine;

namespace Overlewd
{
    namespace QuestWidget
    {
         public class SideQuestButton1 : BaseQuestButton
        {
            public static SideQuestButton1 GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<SideQuestButton1>(
                    "Prefabs/UI/Widgets/QuestsWidget/SideQuest1", parent);
            }
        }
    }
}
