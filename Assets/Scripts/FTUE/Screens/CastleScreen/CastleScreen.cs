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
            protected override void Awake()
            {
                var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/CastleScreen/CastleScreenFTUE"));
                var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
                screenRectTransform.SetParent(transform, false);
                UIManager.SetStretch(screenRectTransform);

                var canvas = screenRectTransform.Find("Canvas");

                cave = canvas.Find("Cave");
                portal = canvas.Find("Portal");
                stable = canvas.Find("Stable");
                crunch = canvas.Find("Crunch");
                tower = canvas.Find("Tower");
                source = canvas.Find("Source");
                market = canvas.Find("Market");
                forge = canvas.Find("Forge");
                magicGuild = canvas.Find("MagicGuild");
                capitol = canvas.Find("Capitol");
                castleBuilding = canvas.Find("Castle");

                contenViewerButton = canvas.Find("ContentViewer").GetComponent<Button>();
            }

            protected override void Customize()
            {
                contenViewerButton.gameObject.SetActive(false);

                NSCastleScreen.GirlBuildingButton.GetInstance(cave);
                //NSCastleScreen.GirlBuildingButton.GetInstance(stable);
                //NSCastleScreen.GirlBuildingButton.GetInstance(crunch);
                //NSCastleScreen.GirlBuildingButton.GetInstance(tower);
                //NSCastleScreen.GirlBuildingButton.GetInstance(source);
                //NSCastleScreen.MarketButton.GetInstance(market);
                //NSCastleScreen.ForgeButton.GetInstance(forge);
                //NSCastleScreen.MagicGuildButton.GetInstance(magicGuild);
                NSCastleScreen.PortalButton.GetInstance(portal);
                //NSCastleScreen.CapitolButton.GetInstance(capitol);
                NSCastleScreen.CastleBuildingButton.GetInstance(castleBuilding);

                EventsWidget.GetInstance(transform);
                QuestsWidget.GetInstance(transform);
                BuffWidget.GetInstance(transform);
                SidebarButtonWidget.GetInstance(transform);
            }
        }
    }
}
