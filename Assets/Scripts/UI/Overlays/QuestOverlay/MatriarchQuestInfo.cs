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
                var newItem = MainQuestInfo.LoadPrefab(parent);
                newItem.name = nameof(MatriarchQuestInfo);
                return newItem.AddComponent<MatriarchQuestInfo>();
            }
        }
    }
}
