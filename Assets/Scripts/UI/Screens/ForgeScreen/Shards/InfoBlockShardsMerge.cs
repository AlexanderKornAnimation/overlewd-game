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
        public class InfoBlockShardsMerge : BaseInfoBlock
        {
            public ShardContentMerge shardsCtrl { get; set; }
            public MatriarchMerge consumeMtrch { get; set; }
            public MatriarchMerge targetMtrch { get; set; }

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
