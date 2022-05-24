using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSMemoryListScreen
    {
         public class EventMemoryClosed : MonoBehaviour
        {
            protected Image art;
            protected TextMeshProUGUI title;
            protected Button button;
            protected TextMeshProUGUI legendaryShardCount;
            protected TextMeshProUGUI rareShardCount;
            protected TextMeshProUGUI commonShardCount;

            public MemoryListScreenInData screenInData;

            protected virtual void Awake()
            {
                var canvas = transform.Find("Canvas");
                
                button = canvas.Find("Button").GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);
                
                art = button.transform.Find("Art").GetComponent<Image>();
                title = button.transform.Find("Title").GetComponent<TextMeshProUGUI>();

                var shards = button.transform.Find("Shards");
                
                legendaryShardCount = shards.transform.Find("LegendaryShard").Find("Collected").GetComponent<TextMeshProUGUI>();
                rareShardCount = shards.transform.Find("RareShard").Find("Collected").GetComponent<TextMeshProUGUI>();
                commonShardCount = shards.transform.Find("CommonShard").Find("Collected").GetComponent<TextMeshProUGUI>();
            }

            protected virtual void ButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);

                if (screenInData == null)
                {
                    UIManager.ShowScreen<MemoryScreen>();
                }
                else
                {
                    UIManager.MakeScreen<MemoryScreen>().
                        SetData(new MemoryScreenInData
                        {
                            prevScreenInData = screenInData,
                            ftueStageId = screenInData.ftueStageId,
                            eventStageId = screenInData.eventStageId
                        }).RunShowScreenProcess();
                }
            }
            
            protected virtual void Start()
            {
                Customize();
            }

            protected virtual void Customize()
            {

            }

            public static EventMemoryClosed GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<EventMemoryClosed>(
                    "Prefabs/UI/Screens/MemoryListScreen/EventMemoryClosed", parent);
            }
        }
    }
}
