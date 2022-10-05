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
        public class MatriarchMerge : MatriarchBase
        {
            public ShardContentMerge shardsCtrl { get; set; }
            public InfoBlockShardsMerge ctrl_InfoBlock { get; set; }

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