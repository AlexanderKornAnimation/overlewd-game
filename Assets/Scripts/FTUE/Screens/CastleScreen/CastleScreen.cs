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

                hint = canvas.Find("Hint").gameObject;
                hintText = hint.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            }

            protected override void Customize()
            {
                contenViewerButton.gameObject.SetActive(false);

                var hintMessage = GameGlobalStates.castle_HintMessage;
                hintText.text = hintMessage;
                hint.SetActive(hintMessage != null);

                if (GameGlobalStates.ulviCaveBuilded)
                {
                    NSCastleScreen.GirlBuildingButton.GetInstance(cave);
                }

                if (GameGlobalStates.portalBuilded)
                {
                    NSCastleScreen.PortalButton.GetInstance(portal);
                }

                if (GameGlobalStates.ulviCaveCanBuilded || GameGlobalStates.portalCanBuilded ||
                    GameGlobalStates.ulviCaveBuilded || GameGlobalStates.portalBuilded)
                {
                    NSCastleScreen.CastleBuildingButton.GetInstance(castleBuilding);
                }

                //EventsWidget.GetInstance(transform);
                //QuestsWidget.GetInstance(transform);
                //BuffWidget.GetInstance(transform);
                SidebarButtonWidget.GetInstance(transform);
            }

            protected override async Task AfterShowOperationsAsync()
            {
                var notification = GameGlobalStates.castle_DialogNotificationId;
                if (notification.HasValue)
                {
                    GameGlobalStates.dialogNotification_DialogId = notification.Value;
                    UIManager.ShowNotification<DialogNotification>();
                }

                await Task.CompletedTask;
            }
        }
    }
}
