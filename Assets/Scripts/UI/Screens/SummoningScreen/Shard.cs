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
            protected Image shardBackground;
            protected Image girl;

            protected virtual void Awake()
            {
                shardBackground = transform.Find("ShardBackground").GetComponent<Image>();
                girl = shardBackground.transform.Find("Girl").GetComponent<Image>();
            }

            protected virtual void Start()
            {
                Customize();
            }

            protected virtual void Customize()
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