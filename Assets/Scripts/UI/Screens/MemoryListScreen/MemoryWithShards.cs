using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Overlewd
{
    namespace NSMemoryListScreen
    {
        public class MemoryWithShards : BaseMemory
        {
            private Transform closed;
            
            private GameObject goldShard;
            private TextMeshProUGUI goldShardsCount;
            
            private GameObject purpuleShard;
            private TextMeshProUGUI purpuleShardsCount;
            
            private GameObject greenShard;
            private TextMeshProUGUI greenShardsCount;
            
            private GameObject grayShard;
            private TextMeshProUGUI grayShardsCount;

            protected override void Awake()
            {
                base.Awake();
                closed = canvas.Find("Closed");
                goldShard = closed.Find("ShardGold").gameObject;
                goldShardsCount = goldShard.transform.Find("Count").GetComponent<TextMeshProUGUI>();
                
                purpuleShard = closed.Find("ShardPurpule").gameObject;
                purpuleShardsCount = purpuleShard.transform.Find("Count").GetComponent<TextMeshProUGUI>();
                
                greenShard = closed.Find("ShardGreen").gameObject;
                greenShardsCount = greenShard.transform.Find("Count").GetComponent<TextMeshProUGUI>();
                
                grayShard = closed.Find("ShardGray").gameObject;
                grayShardsCount = grayShard.transform.Find("Count").GetComponent<TextMeshProUGUI>();
            }

            public static MemoryWithShards GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<MemoryWithShards>(
                    "Prefabs/UI/Screens/MemoryListScreen/MemoryWithShards", parent);
            }
        }
    }
}