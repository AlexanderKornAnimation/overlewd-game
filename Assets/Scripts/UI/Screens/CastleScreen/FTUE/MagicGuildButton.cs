using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Overlewd
{
    namespace FTUE
    {
        namespace NSCastleScreen
        {
            public class MagicGuildButton : Overlewd.NSCastleScreen.MagicGuildButton
            {
                protected override void ButtonClick()
                {

                }

                protected override void Customize()
                {
                    base.Customize();

                }

                public new static MagicGuildButton GetInstance(Transform parent)
                {
                    return ResourceManager.InstantiateWidgetPrefab<MagicGuildButton>
                        ("Prefabs/UI/Screens/CastleScreen/MagicGuildButton", parent);
                }
            }
        }
    }
}
