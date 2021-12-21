using UnityEngine;

namespace Overlewd
{
    namespace FTUE
    {
        namespace NSCastleScreen
        {
            public class GirlBuildingButton : Overlewd.NSCastleScreen.GirlBuildingButton
            {
                protected override void ButtonClick()
                {
                    
                }

                protected override void Customize()
                {
                    base.Customize();

                    notificationsGrid.gameObject.SetActive(false);
                    markers.gameObject.SetActive(false);
                }

                public new static GirlBuildingButton GetInstance(Transform parent)
                {
                    var newItem = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/CastleScreen/GirlBuildingButton"), parent);
                    newItem.name = nameof(GirlBuildingButton);

                    return newItem.AddComponent<GirlBuildingButton>();
                }
            }
        }
    }
}