using System.Collections;
using System.Collections.Generic;
using Overlewd;
using UnityEngine;

namespace Overlewd
{
    namespace FTUE
    {
        namespace NSCastleScreen
        {
            public class ForgeButton : Overlewd.NSCastleScreen.ForgeButton
            {
                protected override void ButtonClick()
                {
                    
                }

                protected override void Customize()
                {
                    base.Customize();

                }

                public new static ForgeButton GetInstance(Transform parent)
                {
                    var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/CastleScreen/ForgeButton"), parent);
                    newItem.name = nameof(ForgeButton);

                    return newItem.AddComponent<ForgeButton>();
                }
            }
        }
    }
}
