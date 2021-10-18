using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace NSMarketScreen
    {
        public class BundleTypeC : MonoBehaviour
        {
            void Start()
            {

            }

            void Update()
            {

            }

            public static BundleTypeC GetInstance(Transform parent)
            {
                var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/MarketScreen/Bundle3"), parent);
                newItem.name = nameof(BundleTypeC);
                return newItem.AddComponent<BundleTypeC>();
            }
        }
    }
}
