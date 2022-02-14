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
                    SoundManager.PlayOneShoot(SoundPath.UI.CastleScreenButtons);
                    UIManager.ShowScreen<BuildingScreen>();
                }

                protected override void Customize()
                {
                    base.Customize();

                    notificationGrid.gameObject.SetActive(false);
                    markers.gameObject.SetActive(false);

                    if (GameGlobalStates.castle_BuildingButtonLock)
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

                public new static CastleBuildingButton GetInstance(Transform parent)
                {
                    return ResourceManager.InstantiateWidgetPrefab<CastleBuildingButton>
                        ("Prefabs/UI/Screens/CastleScreen/CastleBuildingButton", parent);
                }
            }
        }
    }
}