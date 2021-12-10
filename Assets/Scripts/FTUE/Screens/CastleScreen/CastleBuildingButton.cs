using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace FTUE
    {
        namespace NSCastleScreen
        {
            public class CastleBuildingButton : Overlewd.NSCastleScreen.CastleBuildingButton
            {
                protected override void ButtonClick()
                {
                    UIManager.ShowScreen<BuildingScreen>();
                }

                protected override void Customize()
                {
                    base.Customize();

                }

                public new static CastleBuildingButton GetInstance(Transform parent)
                {
                    var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/CastleScreen/CastleBuildingButton"), parent);
                    newItem.name = nameof(CastleBuildingButton);

                    return newItem.AddComponent<CastleBuildingButton>();
                }
            }
        }
    }
}