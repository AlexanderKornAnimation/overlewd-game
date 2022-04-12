using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace FTUE
    {
        public class SummoningScreen : Overlewd.SummoningScreen
        {
            protected override void Start()
            {
                Customize();
            }

            protected override void Customize()
            {
                var shardPositions = transform.Find("Canvas").Find("ShardPositions");

                // for (int i = 1; i <= shardPositions.childCount; i++)
                // {
                //     var pos = shardPositions.Find($"Shard{i}");
                //     NSSummoningScreen.Shard.GetInstance(pos);
                // }
            }

            protected override void BackButtonClick()
            {

            }

            protected override void HaremButtonClick()
            {

            }

            protected override void PortalButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                UIManager.ShowScreen<PortalScreen>();
            }
        }
    }
}