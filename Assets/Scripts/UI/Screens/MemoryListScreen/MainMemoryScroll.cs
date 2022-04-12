using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSMemoryListScreen
    {
         public class MainMemoryScroll : BaseScroll
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
                    MainMemoryClosed.GetInstance(content);
                    MainMemoryOpened.GetInstance(content);
                }
            }

            public static MainMemoryScroll GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<MainMemoryScroll>
                    ("Prefabs/UI/Screens/MemoryListScreen/MainMemoryScroll", parent);
            }
        }
    }
}