using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Overlewd
{
    namespace NSMemoryListScreen
    {
         public class MemoryList : MonoBehaviour
         {
             private List<BaseMemory> memories = new List<BaseMemory>();

             private Transform content;
             private TextMeshProUGUI unlockedLockedCount;
             private TextMeshProUGUI title;
             
             private void Awake()
             {
                 var canvas = transform.Find("Canvas");
                 var bannerBack = canvas.Find("BannerBack");
                 var titleBack = canvas.Find("TitleBack");
                 
                 unlockedLockedCount = bannerBack.Find("UnlockedLockedCount").GetComponent<TextMeshProUGUI>();
                 title = titleBack.Find("Title").GetComponent<TextMeshProUGUI>();
                 content = canvas.Find("MemoryScroll").Find("Viewport").Find("Content");

                 var rect = GetComponent<RectTransform>();
                 var parentRect = transform.parent.GetComponent<RectTransform>();
                 rect.sizeDelta = new Vector2(parentRect.rect.width, 496);
             }

             private void Start()
             {
                 Customize();
             }

             private void Customize()
             {
                 for (int i = 0; i < 5; i++)
                 {
                     Memory.GetInstance(content);
                     MemoryWithShards.GetInstance(content);
                 }
             }
             

             public static MemoryList GetInstance(Transform parent)
             {
                 return ResourceManager.InstantiateWidgetPrefab<MemoryList>(
                     "Prefabs/UI/Screens/MemoryListScreen/MemoryList", parent);
             }
         }
    }
}
