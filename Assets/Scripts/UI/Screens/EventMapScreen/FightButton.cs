using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public class FightButton : MonoBehaviour
    {
        void Start()
        {

        }

        void Update()
        {

        }

        public static FightButton GetInstance(Transform parent)
        {
            var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/EventMapScreen/FightButton"), parent);
            newItem.name = nameof(FightButton);
            return newItem.AddComponent<FightButton>();
        }
    }
}
