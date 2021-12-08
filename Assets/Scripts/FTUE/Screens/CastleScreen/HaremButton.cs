using UnityEngine;

namespace Overlewd
{
    namespace FTUE
    {
        namespace NSCastleScreen
        {
            public class HaremButton : Overlewd.NSCastleScreen.HaremButton
            {

                public new static HaremButton GetInstance(Transform parent)
                {
                    var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/CastleScreen/HaremButton"), parent);
                    newItem.name = nameof(HaremButton);

                    return newItem.AddComponent<HaremButton>();
                }
            }
        }
    }
}