using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
                SoundManager.PlayOneShoot(SoundManager.SoundPath.UI.ButtonClick);
                FTUE.GameData.Initialization();
                FTUE.GameGlobalStates.Reset();

                FTUE.GameGlobalStates.sexScreen_DialogId = 1;
                UIManager.ShowScreen<FTUE.SexScreen>();
            });

            canvas.Find("Castle").GetComponent<Button>().onClick.AddListener(() =>
            {
                SoundManager.PlayOneShoot(SoundManager.SoundPath.UI.ButtonClick);
                UIManager.ShowScreen<CastleScreen>();
            });
        }
    }
}
