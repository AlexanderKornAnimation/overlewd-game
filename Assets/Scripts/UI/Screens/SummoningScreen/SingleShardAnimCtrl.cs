using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSSummoningScreen
    {
        public class SingleShardAnimCtrl : MonoBehaviour
        {
            void Awake()
            {

            }

            public static SingleShardAnimCtrl GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<SingleShardAnimCtrl>(
                    "Prefabs/UI/Screens/SummoningScreen/Animations/SingleShardAnimation", parent);
            }
        }
    }
}
