using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSForgeScreen
    {
        public class MatriarchExchange : MatriarchBase
        {
            public ShardContentExchange shardsCtrl { get; set; }
            public InfoBlockShardsExchange ctrl_InfoBlock { get; set; }

            protected override void Awake()
            {
                base.Awake();
            }

            public void RefreshState()
            {

            }
        }
    }
}