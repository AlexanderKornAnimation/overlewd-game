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
        public class InfoBlockShardsExchange : BaseInfoBlock
        {
            public ShardContentExchange shardsCtrl { get; set; }
            public MatriarchExchange consumeMtrch { get; set; }
            public MatriarchExchange targetMtrch { get; set; }

            protected override void IncClick()
            {
                base.IncClick();

            }

            protected override void DecClick()
            {
                base.DecClick();

            }

            public void RefreshState()
            {

            }
        }
    }
}
