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
        private Button Reset_Button;
        private Button FTUE_Dev_Button;
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

            Reset_Button = backRect.Find("Reset").GetComponent<Button>();
            Reset_Button.onClick.AddListener(Reset_ButtonClick);

            FTUE_Dev_Button = backRect.Find("FTUE_dev").GetComponent<Button>();
            FTUE_Dev_Button.onClick.AddListener(FTUE_Dev_ButtonClick);

            Battle_Button = backRect.Find("Battle").GetComponent<Button>();
            Battle_Button.onClick.AddListener(Battle_ButtonClick);

            showHideButton = backRect.Find("ShowHideButton").GetComponent<Button>();
            showHideButton.onClick.AddListener(ShowHideButtonClick);

            ShowHideButtonClick();
            
#if !UNITY_EDITOR
            Battle_Button.gameObject.SetActive(false);
#endif

#if !(DEV_BUILD || UNITY_EDITOR)
            FTUE_Dev_Button.gameObject.SetActive(false);
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

            GameData.devMode = false;
            GameData.ftue.activeChapter.SetAsMapChapter();

            var firstSexStage = GameData.ftue.info.chapter1.GetStageByKey("sex1");
            if (firstSexStage.isComplete)
            {
                UIManager.ShowScreen<MapScreen>();
            }
            else
            {
                UIManager.MakeScreen<SexScreen>().SetData(new SexScreenInData
                {
                    ftueStageId = firstSexStage.id,
                }).RunShowScreenProcess();
            }
        }

        private void Reset_ButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            ResetAndQuit();
        }

        private void FTUE_Dev_ButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);

            GameData.devMode = true;
            GameData.ftue.info.chapter1.SetAsMapChapter();
            UIManager.ShowScreen<MapScreen>();
        }

        private void Battle_ButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            GameData.devMode = true;
            UIManager.MakeScreen<BaseBattleScreen>().SetData(new BaseBattleScreenInData
            {
                battleId = 19
            }).RunShowScreenProcess();
        }

        private async void ResetAndQuit()
        {
            await AdminBRO.resetAsync();
            Game.Quit();
        }

        public static DevWidget GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateWidgetPrefab<DevWidget>("Prefabs/UI/Widgets/DevWidget/DevWidget", parent);
        }
    }
}