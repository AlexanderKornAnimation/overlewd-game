using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace FTUE
    {
        public class CastleScreen : Overlewd.CastleScreen
        {
            private GameObject hint;
            private TextMeshProUGUI hintText;

            protected override void Awake()
            {
                var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/CastleScreen/CastleScreenFTUE", transform);

                var canvas = screenInst.transform.Find("Canvas");

                harem = canvas.Find("Harem");
                portal = canvas.Find("Portal");
                market = canvas.Find("Market");
                forge = canvas.Find("Forge");
                magicGuild = canvas.Find("MagicGuild");
                capitol = canvas.Find("Capitol");
                castleBuilding = canvas.Find("Castle");
                sanctuary = canvas.Find("Sanctuary");
                catacombs = canvas.Find("Catacombs");
                aerostat = canvas.Find("Aerostat");

                contenViewerButton = canvas.Find("ContentViewer").GetComponent<Button>();

                hint = canvas.Find("Hint").gameObject;
                hintText = hint.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            }

            protected override void Customize()
            {
                contenViewerButton.gameObject.SetActive(false);

                var hintMessage = "Hint";
                hintText.text = hintMessage;
                hint.SetActive(hintMessage != null);

                if (/*GameGlobalStates.ulviCaveBuilded*/true)
                {
                    NSCastleScreen.GirlBuildingButton.GetInstance(harem);
                }

                if (/*GameGlobalStates.portalBuilded*/true)
                {
                    NSCastleScreen.PortalButton.GetInstance(portal);
                }

                if (/*GameGlobalStates.ulviCaveCanBuilded || GameGlobalStates.portalCanBuilded ||
                    GameGlobalStates.ulviCaveBuilded || GameGlobalStates.portalBuilded*/true)
                {
                    NSCastleScreen.CastleBuildingButton.GetInstance(castleBuilding);
                }

                //EventsWidget.GetInstance(transform);
                //QuestsWidget.GetInstance(transform);
                //BuffWidget.GetInstance(transform);
                SidebarButtonWidget.GetInstance(transform);
            }
        }
    }
}
