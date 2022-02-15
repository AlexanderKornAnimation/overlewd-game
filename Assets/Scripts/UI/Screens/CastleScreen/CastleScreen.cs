using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class CastleScreen : BaseScreen
    {
        protected Transform cave;
        protected Transform stable;
        protected Transform crunch;
        protected Transform tower;
        protected Transform source;
        protected Transform market;
        protected Transform forge;
        protected Transform magicGuild;
        protected Transform portal;
        protected Transform capitol;
        protected Transform castleBuilding;

        protected Button contenViewerButton;

        protected virtual void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/CastleScreen/CastleScreen", transform);

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
            contenViewerButton.onClick.AddListener(() => { UIManager.ShowScreen<DebugContentViewer>(); });
        }

        private void Start()
        {
            Customize();
        }

        public override void StartShow()
        {
            SoundManager.PlayOneShoot(SoundManager.SoundPath.UI.CastleWindowSlideOn);
        }

        public override void StartHide()
        {
            SoundManager.PlayOneShoot(SoundManager.SoundPath.UI.CastleWindowSlideOff);
        }

        protected virtual void Customize()
        {
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

            var eventWidgetButton = EventsWidget.GetInstance(transform).transform.Find("Canvas").Find("EventsButton").GetComponent<Button>();
            QuestsWidget.GetInstance(transform);
            BuffWidget.GetInstance(transform);
            SidebarButtonWidget.GetInstance(transform);

            eventWidgetButton.gameObject.AddComponent<BlendPulseSelector>();
            eventWidgetButton.onClick.AddListener(() =>
            {
                Destroy(eventWidgetButton.gameObject.GetComponent<Selector>());
            });
        }
    }
}