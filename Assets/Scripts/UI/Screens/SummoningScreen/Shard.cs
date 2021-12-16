using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
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
            var shard = (GameObject) Instantiate(Resources.Load("Prefabs/UI/Screens/SummoningScreen/Shard"), parent);
            shard.name = nameof(Shard);
            return shard.AddComponent<Shard>();
        }
    }
}