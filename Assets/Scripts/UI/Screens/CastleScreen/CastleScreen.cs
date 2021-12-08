using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class CastleScreen : BaseScreen
    {
        private Transform cave;
        private Transform stable;
        private Transform crunch;
        private Transform tower;
        private Transform source;
        private Transform market;
        private Transform forge;
        private Transform magicGuild;
        private Transform portal;
        private Transform capitol;
        private Transform castleBuilding;
        
        private void Start()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/CastleScreen/Castle"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);

            var canvas = screenRectTransform.Find("Canvas");

            cave = canvas.Find("Cave");
            stable = canvas.Find("Stable");
            crunch = canvas.Find("Crunch");
            tower = canvas.Find("Tower");
            source = canvas.Find("Source");
            market = canvas.Find("Market");
            forge = canvas.Find("Forge");
            magicGuild = canvas.Find("MagicGuild");
            portal = canvas.Find("Portal");
            capitol = canvas.Find("Capitol");
            castleBuilding = canvas.Find("Castle");

            NSCastleScreen.HaremButton.GetInstance(cave);
            NSCastleScreen.HaremButton.GetInstance(stable);
            NSCastleScreen.HaremButton.GetInstance(crunch);
            NSCastleScreen.HaremButton.GetInstance(tower);
            NSCastleScreen.HaremButton.GetInstance(source);
            NSCastleScreen.MarketButton.GetInstance(market);
            NSCastleScreen.ForgeButton.GetInstance(forge);
            NSCastleScreen.MagicGuildButton.GetInstance(magicGuild);
            NSCastleScreen.PortalButton.GetInstance(portal);
            NSCastleScreen.CapitolButton.GetInstance(capitol);
            NSCastleScreen.CastleBuildingButton.GetInstance(castleBuilding);
            
            screenRectTransform.Find("Canvas").Find("ContentViewer").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<DebugContentViewer>();
            });

            EventsWidget.CreateInstance(transform);
            QuestsWidget.CreateInstance(transform);
            BuffWidget.CreateInstance(transform);
            SidebarButtonWidget.CreateInstance(transform);
        }
    }
}
