using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class StartingScreen : BaseFullScreenParent<StartingScreenInData>
    {
        private Button FTUE_Button;
        private Button Castle_Button;
        private Button Reset_Button;
        private Button FTUE_Dev_Button;
        private Button Battle_Button;

        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/StartingScreen/StartingScreen", transform);

            var canvas = screenInst.transform.Find("Canvas");

            FTUE_Button = canvas.Find("FTUE").GetComponent<Button>();
            FTUE_Button.onClick.AddListener(FTUE_ButtonClick);

            Castle_Button = canvas.Find("Castle").GetComponent<Button>();
            Castle_Button.onClick.AddListener(Castle_ButtonClick);

            Reset_Button = canvas.Find("Reset").GetComponent<Button>();
            Reset_Button.onClick.AddListener(Reset_ButtonClick);

            FTUE_Dev_Button = canvas.Find("FTUE_Dev").GetComponent<Button>();
            FTUE_Dev_Button.onClick.AddListener(FTUE_Dev_ButtonClick);

            Battle_Button = canvas.Find("Battle").GetComponent<Button>();
            Battle_Button.onClick.AddListener(Battle_ButtonClick);

#if !UNITY_EDITOR
            Battle_Button.gameObject.SetActive(false);
#endif

#if !(DEV_BUILD || UNITY_EDITOR)
            FTUE_Dev_Button.gameObject.SetActive(false);
#endif
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
                UIManager.MakeScreen<SexScreen>().
                    SetData(new SexScreenInData
                    {
                        ftueStageId = firstSexStage.id,
                    }).RunShowScreenProcess();
            }
        }

        private void Castle_ButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            GameData.devMode = true;
            UIManager.ShowScreen<CastleScreen>();
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
            UIManager.MakeScreen<BaseBattleScreen>().
                SetData(new BaseBattleScreenInData
                {
                    battleId = 19
                }).RunShowScreenProcess();
        }

        private async void ResetAndQuit()
        {
            await AdminBRO.resetAsync();
            Game.Quit();
        }
    }

    public class StartingScreenInData : BaseFullScreenInData
    {

    }
}
