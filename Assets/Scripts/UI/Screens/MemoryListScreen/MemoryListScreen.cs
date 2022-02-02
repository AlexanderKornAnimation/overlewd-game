using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class MemoryListScreen : BaseScreen
    {
        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/MemoryListScreen/MemoryList", transform);

            var canvas = screenInst.transform.Find("Canvas");

            
        }

        void Start()
        {
            Customize();
        }

        private void Customize()
        {
            
        }
    }
}
