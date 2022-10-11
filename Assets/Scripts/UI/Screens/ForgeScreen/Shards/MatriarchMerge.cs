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

            public bool IsConsume => ctrl_InfoBlock.consumeMtrch == this;

            public override void RefreshState()
            {
                if (!IsOpen)
                    return;

                base.RefreshState();

                if (IsConsume)
                {
                    isConsume.gameObject.SetActive(true);
                    isTarget.gameObject.SetActive(false);
                    shadeInfo.gameObject.SetActive(true);
                    msgTitle.gameObject.SetActive(false);
                }
                else if (ctrl_InfoBlock.consumeMtrch == null)
                {
                    isConsume.gameObject.SetActive(false);
                    isTarget.gameObject.SetActive(false);
                    shadeInfo.gameObject.SetActive(true);
                    msgTitle.gameObject.SetActive(true);
                    msgTitle.text = "Tab to select consume shards";
                }
                else
                {
                    isConsume.gameObject.SetActive(false);
                    isTarget.gameObject.SetActive(false);
                    shadeInfo.gameObject.SetActive(true);
                    msgTitle.gameObject.SetActive(true);
                    msgTitle.text = "Tab to reselect consume shards";
                }
            }

            protected override void ButtonClick()
            {
                if (IsConsume)
                {
                    ctrl_InfoBlock.consumeMtrch = null;
                }
                else if (ctrl_InfoBlock.consumeMtrch == null)
                {
                    ctrl_InfoBlock.consumeMtrch = this;
                }
                else
                {
                    ctrl_InfoBlock.consumeMtrch = this;
                }

                shardsCtrl.RefreshState();
            }
        }
    }
}