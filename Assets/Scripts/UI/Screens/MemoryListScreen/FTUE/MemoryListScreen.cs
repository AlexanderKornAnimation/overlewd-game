using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Overlewd.NSMemoryListScreen;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace FTUE
    {
        public class MemoryListScreen : Overlewd.MemoryListScreen
        {
            protected override void Awake()
            {
                base.Awake();
            }
            
            protected override void Start()
            {
                base.Start();
            }
            
            protected override void UlviButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                LeaveTab(prevTab);
                EnterTab(ulviTab);
            }

            protected override void AdrielButtonClick()
            {

            }

            protected override void FayeButtonClick()
            {

            }

            protected override void IngieButtonClick()
            {

            }

            protected override void LiliButtonClick()
            {
            }
        }
    }
}