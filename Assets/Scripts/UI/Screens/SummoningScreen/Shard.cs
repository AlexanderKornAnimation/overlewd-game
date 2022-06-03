using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSSummoningScreen
    {
        public class Shard : Item
        {
            public static Shard GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<Shard>(
                    "Prefabs/UI/Screens/SummoningScreen/Animations/Shards/ShardAnim", parent);
            }
        }
    }
}
