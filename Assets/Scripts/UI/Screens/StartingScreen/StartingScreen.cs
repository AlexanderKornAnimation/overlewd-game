using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class StartingScreen : BaseFullScreen
    {
        private Button FTUE_Progress_Button;
        private Button FTUE_Button;
        private Button Reset_FTUE_Button;
        private Button Castle_Button;

        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/StartingScreen/StartingScreen", transform);

            var canvas = screenInst.transform.Find("Canvas");

            FTUE_Progress_Button = canvas.Find("FTUE_Progress").GetComponent<Button>();
            FTUE_Progress_Button.onClick.AddListener(FTUE_Progress_ButtonClick);

            FTUE_Button = canvas.Find("FTUE").GetComponent<Button>();
            FTUE_Button.onClick.AddListener(FTUE_ButtonClick);

            Reset_FTUE_Button = canvas.Find("Reset_FTUE").GetComponent<Button>();
            Reset_FTUE_Button.onClick.AddListener(Reset_FTUE_ButtonClick);

            Castle_Button = canvas.Find("Castle").GetComponent<Button>();
            Castle_Button.onClick.AddListener(Castle_ButtonClick);
        }

        private void FTUE_Progress_ButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);

            GameData.progressMode = true;
            GameData.ftue.activeChapter.SetAsMapChapter();

            var firstSexStage = GameData.ftue.info.chapter1.GetStageByKey("sex1");
            if (firstSexStage.isComplete)
            {
                UIManager.MakeScreen<MapScreen>().
                    SetData(new MapScreenInData()).
                    RunShowScreenProcess();
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

        private void FTUE_ButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);

            GameData.progressMode = false;
            GameData.ftue.info.chapter1.SetAsMapChapter();
            UIManager.ShowScreen<MapScreen>();
        }

        private void Reset_FTUE_ButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            FTUEReset();
        }

        private void Castle_ButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            GameData.progressMode = false;
            UIManager.ShowScreen<CastleScreen>();
        }

        private async void FTUEReset()
        {
            await GameData.ftue.Reset();
            await GameData.buildings.Reset();
        }
    }
}
