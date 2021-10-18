using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSMarketScreen
    {
        public class BundleTypeD : MonoBehaviour
        {
            void Start()
            {

            }

            void Update()
            {

            }

            public static BundleTypeD GetInstance(Transform parent)
            {
                var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/MarketScreen/Bundle4"), parent);
                newItem.name = nameof(BundleTypeD);
                return newItem.AddComponent<BundleTypeD>();
            }
        }
    }
}
