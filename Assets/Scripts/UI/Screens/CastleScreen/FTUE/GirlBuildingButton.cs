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
                    SoundManager.PlayOneShot(FMODEventPath.UI_CastleScreenButtons);

                    UIManager.ShowScreen<MapScreen>();
                }

                protected override void Customize()
                {
                    base.Customize();

                    notificationsGrid.gameObject.SetActive(false);
                    markers.gameObject.SetActive(false);

                    if (/*GameGlobalStates.castle_CaveLock*/true)
                    {
                        UITools.DisableButton(button);
                    }
                }

                public new static GirlBuildingButton GetInstance(Transform parent)
                {
                    return ResourceManager.InstantiateWidgetPrefab<GirlBuildingButton>
                        ("Prefabs/UI/Screens/CastleScreen/GirlBuildingButton", parent);
                }
            }
        }
    }
}