using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSMemoryListScreen
    {
        public class MemoryWithShards : BaseMemory
        {
            private Transform closed;
            
            private GameObject heroicShard;
            private TextMeshProUGUI heroicShardAmount;
            
            private GameObject epicShard;
            private TextMeshProUGUI epicShardAmount;
            
            private GameObject advancedShard;
            private TextMeshProUGUI advancedShardAmount;
            
            private GameObject basicShard;
            private TextMeshProUGUI basicShardAmount;

            private Button memoryScreenButton;
            
            public string girlKey { get; set; }
            public AdminBRO.MatriarchItem girlData => GameData.matriarchs.GetMatriarchByKey(girlKey);

            protected override void Awake()
            {
                base.Awake();
                closed = canvas.Find("Closed");
                heroicShard = closed.Find("ShardHeroic").gameObject;
                heroicShardAmount = heroicShard.transform.Find("Count").GetComponent<TextMeshProUGUI>();
                
                epicShard = closed.Find("ShardEpic").gameObject;
                epicShardAmount = epicShard.transform.Find("Count").GetComponent<TextMeshProUGUI>();
                
                advancedShard = closed.Find("ShardAdvanced").gameObject;
                advancedShardAmount = advancedShard.transform.Find("Count").GetComponent<TextMeshProUGUI>();
                
                basicShard = closed.Find("ShardBasic").gameObject;
                basicShardAmount = basicShard.transform.Find("Count").GetComponent<TextMeshProUGUI>();

                memoryScreenButton = closed.Find("MemoryScreenButton").GetComponent<Button>();
                memoryScreenButton.onClick.AddListener(MemoryScreenButtonClick);
            }

            protected override void Customize()
            {
                base.Customize();
                if (memoryData != null)
                {
                    heroicShardAmount.text = $"{girlData?.heroicShard?.amount}/{memoryData?.heroicShard?.amount}";
                    epicShardAmount.text = $"{girlData?.epicShard?.amount}/{memoryData?.epicShard?.amount}";
                    advancedShardAmount.text = $"{girlData?.advancedShard?.amount}/{memoryData?.advancedShard?.amount}";
                    basicShardAmount.text = $"{girlData?.basicShard?.amount}/{memoryData?.basicShard?.amount}";
                    closed.gameObject.SetActive(!memoryData.isOpen);
                }
            }
            
            private void MemoryScreenButtonClick()
            {
                UIManager.MakeScreen<MemoryScreen>().
                    SetData(new MemoryScreenInData
                {
                    girlKey = girlKey,
                    prevScreenInData = UIManager.prevScreenInData
                }).RunShowScreenProcess();
            }
            
            public static MemoryWithShards GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<MemoryWithShards>(
                    "Prefabs/UI/Screens/MemoryListScreen/MemoryWithShards", parent);
            }
        }
    }
}