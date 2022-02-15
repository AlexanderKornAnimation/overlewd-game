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
                    SoundManager.PlayOneShoot(SoundPath.UI.CastleScreenButtons);
                    UIManager.ShowScreen<PortalScreen>();
                }

                protected override void Customize()
                {
                    base.Customize();

                    freeSummonNotification.gameObject.SetActive(false);
                    markers.gameObject.SetActive(false);

                    if (GameGlobalStates.castle_PortalLock)
                    {
                        Lock();
                    }
                }

                public void Lock()
                {
                    button.interactable = false;
                    foreach (var cr in GetComponentsInChildren<CanvasRenderer>())
                    {
                        cr.SetColor(Color.gray);
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
