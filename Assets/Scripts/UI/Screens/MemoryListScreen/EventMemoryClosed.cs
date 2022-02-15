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
            private Image art;
            private TextMeshProUGUI title;
            private Button button;
            private TextMeshProUGUI legendaryShardCount;
            private TextMeshProUGUI rareShardCount;
            private TextMeshProUGUI commonShardCount;

            private void Awake()
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

            private void ButtonClick()
            {
                SoundManager.PlayOneShoot(SoundPath.UI.GenericButtonClick);
                UIManager.ShowScreen<MemoryScreen>();
            }
            
            private void Start()
            {
                Customize();
            }

            private void Customize()
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
