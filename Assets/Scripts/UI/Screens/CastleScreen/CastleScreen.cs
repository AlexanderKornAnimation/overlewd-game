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

        protected CastleScreenInData inputData;

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
        }

        public CastleScreen SetData(CastleScreenInData data)
        {
            inputData = data;
            return this;
        }

        public override async Task BeforeShowMakeAsync()
        {
            foreach (var building in GameData.buildings)
            {
                if (building.isBuilded)
                {
                    switch (building.key)
                    {
                        case AdminBRO.Building.Key_Harem:
                            NSCastleScreen.HaremButton.GetInstance(harem);
                            break;
                        case AdminBRO.Building.Key_Market:
                            NSCastleScreen.MarketButton.GetInstance(market);
                            break;
                        case AdminBRO.Building.Key_Forge:
                            NSCastleScreen.ForgeButton.GetInstance(forge);
                            break;
                        case AdminBRO.Building.Key_MagicGuild:
                            NSCastleScreen.MagicGuildButton.GetInstance(magicGuild);
                            break;
                        case AdminBRO.Building.Key_Portal:
                            NSCastleScreen.PortalButton.GetInstance(portal);
                            break;
                        case AdminBRO.Building.Key_Castle:
                            NSCastleScreen.CastleButton.GetInstance(castle);
                            break;
                        case AdminBRO.Building.Key_Municipality:
                            NSCastleScreen.MunicipalityButton.GetInstance(municipality);
                            break;
                        case AdminBRO.Building.Key_Cathedral:
                            NSCastleScreen.CathedralButton.GetInstance(cathedral);
                            break;
                        case AdminBRO.Building.Key_Catacombs:
                            NSCastleScreen.CatacombsButton.GetInstance(catacombs);
                            break;
                        case AdminBRO.Building.Key_Aerostat:
                            NSCastleScreen.AerostatButton.GetInstance(aerostat);
                            break;
                    }
                }
            }

            EventsWidget.GetInstance(transform);
            QuestsWidget.GetInstance(transform);
            BuffWidget.GetInstance(transform);
            SidebarButtonWidget.GetInstance(transform);

            await Task.CompletedTask;
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
    }

    public class CastleScreenInData : BaseScreenInData
    {
    
    }
}