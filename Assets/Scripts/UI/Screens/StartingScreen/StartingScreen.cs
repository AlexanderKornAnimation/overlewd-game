using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Overlewd.FTUE;
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

                GameGlobalStates.ftueProgressMode = true;
                GameGlobalStates.ftueChapterData = GameData.ftue.activeChapter;

                if (GameGlobalStates.ftueChapterData.key == "chapter1")
                {
                    var firstSexStage = GameGlobalStates.ftueChapterData.GetStageByKey("sex1");
                    if (!firstSexStage?.isComplete ?? false)
                    {
                        UIManager.MakeScreen<FTUE.SexScreen>().
                            SetData(new SexScreenInData
                            {
                                ftueStageId = firstSexStage.id,
                            }).RunShowScreenProcess();
                    }
                    else
                    {
                        UIManager.ShowScreen<MapScreen>();
                    }
                }
                else
                {
                    UIManager.ShowScreen<MapScreen>();
                }
            });

            canvas.Find("FTUE").GetComponent<Button>().onClick.AddListener(() =>
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);

                GameGlobalStates.ftueProgressMode = false;
                GameGlobalStates.ftueChapterData = GameData.ftue.firstChapter;
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
