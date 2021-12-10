using System;
using System.Collections;
using System.Collections.Generic;
using Overlewd.FTUE;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class CastleScreen : BaseScreen
    {
        protected Transform cave;
        protected Transform portal;

        private Transform stable;
        private Transform crunch;
        private Transform tower;
        private Transform source;
        private Transform market;
        private Transform forge;
        private Transform magicGuild;
        private Transform capitol;
        private Transform castleBuilding;

        protected virtual void Start()
        {
            var screenPrefab = (GameObject) Instantiate(Resources.Load("Prefabs/UI/Screens/CastleScreen/CastleScreen"));
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

            NSCastleScreen.HaremButton.GetInstance(cave);
            NSCastleScreen.PortalButton.GetInstance(portal);
            NSCastleScreen.HaremButton.GetInstance(stable);
            NSCastleScreen.HaremButton.GetInstance(crunch);
            NSCastleScreen.HaremButton.GetInstance(tower);
            NSCastleScreen.HaremButton.GetInstance(source);
            NSCastleScreen.MarketButton.GetInstance(market);
            NSCastleScreen.ForgeButton.GetInstance(forge);
            NSCastleScreen.MagicGuildButton.GetInstance(magicGuild);
            NSCastleScreen.CapitolButton.GetInstance(capitol);
            NSCastleScreen.CastleBuildingButton.GetInstance(castleBuilding);

            screenRectTransform.Find("Canvas").Find("ContentViewer").GetComponent<Button>().onClick.AddListener(() =>
            {
                UIManager.ShowScreen<DebugContentViewer>();
            });

            EventsWidget.GetInstance(transform);
            QuestsWidget.GetInstance(transform);
            BuffWidget.GetInstance(transform);
            SidebarButtonWidget.GetInstance(transform);
        }
    }
}