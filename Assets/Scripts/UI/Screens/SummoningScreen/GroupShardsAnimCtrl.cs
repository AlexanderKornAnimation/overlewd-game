using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSSummoningScreen
    {
        public class GroupShardsAnimCtrl : MonoBehaviour
        {
            void Awake()
            {

            }

            public static GroupShardsAnimCtrl GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<GroupShardsAnimCtrl>(
                    "Prefabs/UI/Screens/SummoningScreen/Animations/GroupShardsAnimation", parent);
            }
        }
    }
}