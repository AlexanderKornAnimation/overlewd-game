using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Overlewd.FTUE;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class StartingScreen : BaseScreen
    {
        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/StartingScreen/StartingScreen", transform);

            var canvas = screenInst.transform.Find("Canvas");

            canvas.Find("FTUE").GetComponent<Button>().onClick.AddListener(() =>
            {
                SoundManager.PlayOneShoot(SoundPath.UI_GenericButtonClick);
                FTUE.GameData.Initialization();
                FTUE.GameGlobalStates.Reset();

                FTUE.GameGlobalStates.sexScreen_DialogId = 1;
                UIManager.ShowScreen<FTUE.SexScreen>();
            });

            canvas.Find("Castle").GetComponent<Button>().onClick.AddListener(() =>
            {
                SoundManager.PlayOneShoot(SoundPath.UI_GenericButtonClick);
                UIManager.ShowScreen<CastleScreen>();
            });
            
            canvas.Find("TempBattleScreen").GetComponent<Button>().onClick.AddListener(() =>
            {
                SoundManager.PlayOneShoot(SoundPath.UI_GenericButtonClick);
                UIManager.ShowScreen<TempBattleScreen>();
            });
        }
    }
}
