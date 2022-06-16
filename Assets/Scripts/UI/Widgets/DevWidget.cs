using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class DevWidget : BaseWidget
    {
        private Button FTUE_Button;
        private Button FTUE_Dev_Button;
        private Button Reset_Button;
        private Button AddCrystals_Button;
        private Button Battle_Button;
        private Button showHideButton;
        
        private RectTransform backRect;
        private bool isOpen = true;

        private void Awake()
        {
            var canvas = transform.Find("Canvas");
            backRect = canvas.Find("BackRect").GetComponent<RectTransform>();
                
            FTUE_Button = backRect.Find("FTUE").GetComponent<Button>();
            FTUE_Button.onClick.AddListener(FTUE_ButtonClick);
            FTUE_Button.gameObject.SetActive(GameData.devMode);

            FTUE_Dev_Button = backRect.Find("FTUE_dev").GetComponent<Button>();
            FTUE_Dev_Button.onClick.AddListener(FTUE_Dev_ButtonClick);  
            FTUE_Dev_Button.gameObject.SetActive(!GameData.devMode);

            Reset_Button = backRect.Find("Reset").GetComponent<Button>();
            Reset_Button.onClick.AddListener(Reset_ButtonClick);

            AddCrystals_Button = backRect.Find("AddCrystals").GetComponent<Button>();
            AddCrystals_Button.onClick.AddListener(AddCrystals_ButtonClick);

            Battle_Button = backRect.Find("Battle").GetComponent<Button>();
            Battle_Button.onClick.AddListener(Battle_ButtonClick);

            showHideButton = backRect.Find("ShowHideButton").GetComponent<Button>();
            showHideButton.onClick.AddListener(ShowHideButtonClick);

            ShowHideButtonClick();
            
#if !UNITY_EDITOR
            Battle_Button.gameObject.SetActive(false);
#endif
        }
        
        private async Task ShowAsync()
        {
            await UITools.TopShowAsync(backRect);
            isOpen = true;
        }

        private async Task HideAsync()
        {
            await UITools.TopHideAsync(backRect);
            isOpen = false;
        }

        private async void ShowHideButtonClick()
        {
            if (isOpen)
                await HideAsync();
            else
                await ShowAsync();
        }
        
        private void FTUE_ButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            LoadingScreen.RunFTUE();
        }

        private void FTUE_Dev_ButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);

            GameData.devMode = true;
            GameData.ftue.info.chapter1.SetAsMapChapter();
            UIManager.ShowScreen<MapScreen>();
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
                battleId = 19
            }).RunShowScreenProcess();
        }

        public static DevWidget GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateWidgetPrefab<DevWidget>("Prefabs/UI/Widgets/DevWidget/DevWidget", parent);
        }
    }
}