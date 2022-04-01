using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSMemoryListScreen
    {
         public class OpenedEventMemoryScroll : BaseScroll
        {
            protected override void Start()
            {
                Customize();
            }

            protected override void Customize()
            {
                var random = new System.Random();
                var rand = random.Next(1, 9);
                
                for (int i = 0; i < rand; i++)
                {
                    EventMemoryOpened.GetInstance(content);
                }
            }

            public static OpenedEventMemoryScroll GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<OpenedEventMemoryScroll>
                    ("Prefabs/UI/Screens/MemoryListScreen/OpenedEventMemoryScroll", parent);
            }
        }
    }
}