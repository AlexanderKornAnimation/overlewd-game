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
            protected Transform notification;

            protected override void Awake()
            {
                base.Awake();

                notification = transform.Find("CollectCrystalsNotification");
            }

            protected override void ButtonClick()
            {
                
            }

            public static CastleButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<CastleButton>
                    ("Prefabs/UI/Screens/CastleScreen/CastleButton", parent);
            }
        }
    }
}