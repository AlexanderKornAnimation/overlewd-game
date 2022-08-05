using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSSummoningScreen
    {
        public class Crystal : BaseShard
        {
            public static Crystal GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<Crystal>(
                    "Prefabs/UI/Screens/SummoningScreen/Animations/Crystal/Crystal", parent);
            }
        }
    }
}
