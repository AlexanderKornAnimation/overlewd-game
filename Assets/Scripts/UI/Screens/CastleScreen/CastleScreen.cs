using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class CastleScreen : BaseFullScreen
    {
        protected Transform harem;
        protected Transform market;
        protected Transform forge;
        protected Transform magicGuild;
        protected Transform portal;
        protected Transform castle;
        protected Transform municipality;
        protected Transform cathedral;
        protected Transform catacombs;
        protected Transform aerostat;

        protected Transform eventWidget;

        protected Button contenViewerButton;

        protected FMODEvent music;

        protected virtual void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/CastleScreen/CastleScreen", transform);

            var canvas = screenInst.transform.Find("Canvas");

            harem = canvas.Find("Harem");
            portal = canvas.Find("Portal");
            market = canvas.Find("Market");
            forge = canvas.Find("Forge");
            magicGuild = canvas.Find("MagicGuild");
            castle = canvas.Find("Castle");
            municipality = canvas.Find("Municipality");
            cathedral = canvas.Find("Cathedral");
            catacombs = canvas.Find("Catacombs");
            aerostat = canvas.Find("Aerostat");

            contenViewerButton = canvas.Find("ContentViewer").GetComponent<Button>();
            contenViewerButton.onClick.AddListener(() => { UIManager.ShowScreen<DebugContentViewer>(); });
        }

        private void Start()
        {
            Customize();
        }

        public override void StartShow()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_CastleWindowShow);
            music = SoundManager.GetEventInstance(FMODEventPath.Music_CastleScreen);
        }

        public override void StartHide()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_CastleWindowHide);
            music?.Stop();
        }

        protected virtual void Customize()
        {
            NSCastleScreen.HaremButton.GetInstance(harem);
            NSCastleScreen.MarketButton.GetInstance(market);
            NSCastleScreen.ForgeButton.GetInstance(forge);
            NSCastleScreen.MagicGuildButton.GetInstance(magicGuild);
            NSCastleScreen.PortalButton.GetInstance(portal);
            NSCastleScreen.CastleButton.GetInstance(castle);
            NSCastleScreen.MunicipalityButton.GetInstance(municipality);
            NSCastleScreen.CathedralButton.GetInstance(cathedral);
            NSCastleScreen.CatacombsButton.GetInstance(catacombs);
            NSCastleScreen.AerostatButton.GetInstance(aerostat);

            EventsWidget.GetInstance(transform);
            QuestsWidget.GetInstance(transform);
            BuffWidget.GetInstance(transform);
            SidebarButtonWidget.GetInstance(transform);
        }
    }
}