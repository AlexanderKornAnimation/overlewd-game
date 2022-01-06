using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSQuestOverlay
    {
        public class MatriarchQuestInfo : MainQuestInfo
        {
            protected override void Awake()
            {
                base.Awake();
            }

            public static new MatriarchQuestInfo GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<MatriarchQuestInfo>
                    ("Prefabs/UI/Overlays/QuestOverlay/MainQuestInfo", parent);
            }
        }
    }
}
