using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class DevOverlay : BaseOverlayParent<DevOverlayInData>
    {
        private Button FTUE_Button;
        private Button FTUE_Dev_Button;
        private Button Reset_Button;
        private Button AddCrystals_Button;
        private Button Battle_Button;

        private TextMeshProUGUI deviceModel;
        private TextMeshProUGUI deviceId;

        void Awake()
        {
            var inst = ResourceManager.InstantiateScreenPrefab(
                "Prefabs/UI/Overlays/DevOverlay/DevOverlay", transform);
            
            var canvas = inst.transform.Find("Canvas");
            var background = canvas.Find("Background");
                
            FTUE_Button = background.Find("FTUE").GetComponent<Button>();
            FTUE_Button.onClick.AddListener(FTUE_ButtonClick);
            FTUE_Button.gameObject.SetActive(GameData.devMode);

            FTUE_Dev_Button = background.Find("FTUE_dev").GetComponent<Button>();
            FTUE_Dev_Button.onClick.AddListener(FTUE_Dev_ButtonClick);  
            FTUE_Dev_Button.gameObject.SetActive(!GameData.devMode);

            Reset_Button = background.Find("Reset").GetComponent<Button>();
            Reset_Button.onClick.AddListener(Reset_ButtonClick);

            AddCrystals_Button = background.Find("AddCrystals").GetComponent<Button>();
            AddCrystals_Button.onClick.AddListener(AddCrystals_ButtonClick);

            Battle_Button = background.Find("Battle").GetComponent<Button>();
            Battle_Button.onClick.AddListener(Battle_ButtonClick);

            var userInfo = canvas.Find("UserInfo");
            deviceModel = userInfo.Find("DeviceModel").GetComponent<TextMeshProUGUI>();
            deviceId = userInfo.Find("DeviceId").GetComponent<TextMeshProUGUI>();

#if !UNITY_EDITOR
            Battle_Button.gameObject.SetActive(false);
#endif
        }

        public override async Task BeforeShowMakeAsync()
        {
            deviceModel.text = "Device Midel: " + SystemInfo.deviceModel;
            deviceId.text = "Device Id:" + SystemInfo.deviceUniqueIdentifier;

            await Task.CompletedTask;
        }

        public override ScreenShow Show()
        {
            return gameObject.AddComponent<ScreenTopShow>();
        }
        
        public override ScreenHide Hide()
        {
            return gameObject.AddComponent<ScreenTopHide>();
        }

        private void FTUE_ButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            if (UIManager.HasScreen<CastleScreen>())
            {
                GameData.devMode = false;
                UIManager.ShowScreen<CastleScreen>();
            }
            else
            {
                LoadingScreen.RunFTUE();
            }
        }

        private void FTUE_Dev_ButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);

            GameData.devMode = true;
            if (UIManager.HasScreen<CastleScreen>())
            {
                UIManager.ShowScreen<CastleScreen>();
            }
            else
            {
                GameData.ftue.info.chapter1.SetAsMapChapter();
                UIManager.ShowScreen<MapScreen>();
            }
        }

        private async void Reset_ButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            await AdminBRO.resetAsync();
            Game.Quit();
        }

        private async void AddCrystals_ButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            await GameData.player.AddCrystals();
        }

        private void Battle_ButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakeScreen<BaseBattleScreen>().SetData(new BaseBattleScreenInData
            {
                ftueStageId = 24
            }).RunShowScreenProcess();
        }
    }

    public class DevOverlayInData : BaseOverlayInData
    {
        
    }
}