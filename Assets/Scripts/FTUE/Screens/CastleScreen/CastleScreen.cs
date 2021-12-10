using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace FTUE
    {
        public class CastleScreen : Overlewd.CastleScreen
        {
            protected override void Customize()
            {
                contenViewerButton.gameObject.SetActive(false);

                NSCastleScreen.GirlBuildingButton.GetInstance(cave);
                NSCastleScreen.GirlBuildingButton.GetInstance(stable);
                NSCastleScreen.GirlBuildingButton.GetInstance(crunch);
                NSCastleScreen.GirlBuildingButton.GetInstance(tower);
                NSCastleScreen.GirlBuildingButton.GetInstance(source);
                NSCastleScreen.MarketButton.GetInstance(market);
                NSCastleScreen.ForgeButton.GetInstance(forge);
                NSCastleScreen.MagicGuildButton.GetInstance(magicGuild);
                NSCastleScreen.PortalButton.GetInstance(portal);
                NSCastleScreen.CapitolButton.GetInstance(capitol);
                NSCastleScreen.CastleBuildingButton.GetInstance(castleBuilding);

                EventsWidget.GetInstance(transform);
                QuestsWidget.GetInstance(transform);
                BuffWidget.GetInstance(transform);
                SidebarButtonWidget.GetInstance(transform);
            }
        }
    }
}
