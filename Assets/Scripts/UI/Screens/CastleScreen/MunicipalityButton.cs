using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Overlewd
{
    namespace NSCastleScreen
    {
        public class MunicipalityButton : BaseButton
        {
            protected Transform collectNotification;

            protected override void Awake()
            {
                base.Awake();

                collectNotification = transform.Find("CollectNotification");
            }

            protected override void ButtonClick()
            {
                base.ButtonClick();
                UIManager.ShowScreen<MunicipalityScreen>();
            }
            
            public static MunicipalityButton GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<MunicipalityButton>
                    ("Prefabs/UI/Screens/CastleScreen/MunicipalityButton", parent);
            }
        }
    }
}