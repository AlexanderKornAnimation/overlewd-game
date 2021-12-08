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

                public new static MagicGuildButton GetInstance(Transform parent)
                {
                    var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/CastleScreen/MagicGuildButton"), parent);
                    newItem.name = nameof(MagicGuildButton);

                    return newItem.AddComponent<MagicGuildButton>();
                }
            }
        }
    }
}
