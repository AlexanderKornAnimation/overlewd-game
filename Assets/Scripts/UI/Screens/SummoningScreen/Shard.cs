using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSSummoningScreen
    {
        public class Shard : MonoBehaviour
        {
            private Image shardBackground;
            private Image girl;

            private void Awake()
            {
                shardBackground = transform.Find("ShardBackground").GetComponent<Image>();
                girl = shardBackground.transform.Find("Girl").GetComponent<Image>();
            }

            private void Start()
            {
                Customize();
            }

            private void Customize()
            {

            }

            public static Shard GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<Shard>
                    ("Prefabs/UI/Screens/SummoningScreen/Shard", parent);
            }
        }
    }
}