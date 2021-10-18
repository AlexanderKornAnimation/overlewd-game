using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSMarketScreen
    {
        public class BundleTypeB : MonoBehaviour
        {
            void Start()
            {

            }

            void Update()
            {

            }

            public static BundleTypeB GetInstance(Transform parent)
            {
                var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/MarketScreen/Bundle2"), parent);
                newItem.name = nameof(BundleTypeB);
                return newItem.AddComponent<BundleTypeB>();
            }
        }
    }
}
