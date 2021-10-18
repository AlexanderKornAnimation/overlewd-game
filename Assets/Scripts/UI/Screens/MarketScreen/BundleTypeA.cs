using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSMarketScreen
    {
        public class BundleTypeA : MonoBehaviour
        {
            void Start()
            {

            }

            void Update()
            {

            }

            public static BundleTypeA GetInstance(Transform parent)
            {
                var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/MarketScreen/Bundle1"), parent);
                newItem.name = nameof(BundleTypeA);
                return newItem.AddComponent<BundleTypeA>();
            }
        }
    }
}
