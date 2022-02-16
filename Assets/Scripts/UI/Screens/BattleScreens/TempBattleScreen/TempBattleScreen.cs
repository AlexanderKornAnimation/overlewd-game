using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Overlewd
{
    public class TempBattleScreen : BaseScreen
    {
        private void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/BattleScreens/TempBattleScreen/TempBattleScreen", transform);
            var canvas = screenInst.transform.Find("Canvas");
                
            canvas.Find("Button").GetComponent<Button>().onClick.AddListener(() =>
            {
                SoundManager.PlayOneShoot(SoundPath.UI.GenericButtonClick);
                UIManager.ShowScreen<StartingScreen>();
            });
        }

        public override void StartShow()
        {
            SoundManager.PlayOneShoot(SoundPath.UI.BattleScreenShow);
        }
    }
}