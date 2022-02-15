using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSMemoryListScreen
    {
        public class ClosedEventMemoryScroll : BaseScroll
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
                    AddMemory();
                }
            }

            private void AddMemory()
            {
                EventMemoryClosed.GetInstance(content);
            }

            public static ClosedEventMemoryScroll GetInstance(Transform parent)
            {
                var prefab = ResourceManager.InstantiateWidgetPrefab<ClosedEventMemoryScroll>("Prefabs/UI/Screens/MemoryListScreen/ClosedEventMemoryScroll", parent);
                return prefab;
            }
        }
    }
}