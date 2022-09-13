using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSSummoningScreen
    {
        public class GroupShardsAnimCtrl : BaseShardsAnimCtrl
        {
            protected override void Awake()
            {
                base.Awake();
            }

            public static GroupShardsAnimCtrl GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<GroupShardsAnimCtrl>(
                    "Prefabs/UI/Screens/SummoningScreen/Animations/GroupShardsAnimation", parent);
            }
        }
    }
}