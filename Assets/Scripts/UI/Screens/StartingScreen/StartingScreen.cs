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
        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/StartingScreen/StartingScreen", transform);

            var canvas = screenInst.transform.Find("Canvas");

            canvas.Find("FTUE_Progress").GetComponent<Button>().onClick.AddListener(() =>
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);

                GameData.ftue.progressMode = true;
                GameData.ftue.activeChapter.SetAsMapChapter();

                var firstSexStage = GameData.ftue.chapter1.GetStageByKey("sex1");
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
            });

            canvas.Find("FTUE").GetComponent<Button>().onClick.AddListener(() =>
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);

                GameData.ftue.progressMode = false;
                GameData.ftue.chapter1.SetAsMapChapter();
                UIManager.ShowScreen<MapScreen>();
            });

            canvas.Find("Reset_FTUE").GetComponent<Button>().onClick.AddListener(() =>
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                FTUEReset();
            });

            canvas.Find("Castle").GetComponent<Button>().onClick.AddListener(() =>
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                UIManager.ShowScreen<CastleScreen>();
            });
            
            canvas.Find("BattleScreen").GetComponent<Button>().onClick.AddListener(() =>
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                UIManager.ShowScreen<BaseBattleScreen>();
            });
        }

        private async void FTUEReset()
        {
            await GameData.FTUEReset();
            await GameData.BuildingsReset();
        }
    }
}
