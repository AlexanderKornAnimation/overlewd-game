using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    namespace FTUE
    {
        namespace NSCastleScreen
        {
            public class PortalButton : Overlewd.NSCastleScreen.PortalButton
            {
                protected override void ButtonClick()
                {
                    SoundManager.PlayOneShot(FMODEventPath.UI_CastleScreenButtons);
                    UIManager.ShowScreen<PortalScreen>();
                }

                protected override void Customize()
                {
                    base.Customize();

                    freeSummonNotification.gameObject.SetActive(false);
                    markers.gameObject.SetActive(false);

                    if (/*GameGlobalStates.castle_PortalLock*/true)
                    {
                        UITools.DisableButton(button);
                    }
                }

                public new static PortalButton GetInstance(Transform parent)
                {
                    return ResourceManager.InstantiateWidgetPrefab<PortalButton>
                        ("Prefabs/UI/Screens/CastleScreen/PortalButton", parent);
                }
            }
        }
    }
}
