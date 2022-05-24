using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Overlewd
{
    namespace NSCastleScreen
    {
        public class CastleButton : BaseButton
        {
            protected override void Awake()
            {
                base.Awake();
            }

            protected override void ButtonClick()
            {
                UIManager.ShowScreen<MapScreen>();
            }

            public static CastleButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<CastleButton>
                    ("Prefabs/UI/Screens/CastleScreen/CastleButton", parent);
            }
        }
    }
}