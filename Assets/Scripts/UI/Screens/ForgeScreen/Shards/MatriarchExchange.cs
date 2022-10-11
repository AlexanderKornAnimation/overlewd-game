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

            public bool IsConsume => ctrl_InfoBlock.consumeMtrch == this;
            public bool IsTarget => ctrl_InfoBlock.targetMtrch == this;

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
                    button.interactable = true;
                }
                else if (IsTarget)
                {
                    isConsume.gameObject.SetActive(false);
                    isTarget.gameObject.SetActive(true);
                    shadeInfo.gameObject.SetActive(true);
                    msgTitle.gameObject.SetActive(false);
                    button.interactable = true;
                }
                else if (ctrl_InfoBlock.consumeMtrch == null)
                {
                    isConsume.gameObject.SetActive(false);
                    isTarget.gameObject.SetActive(false);
                    shadeInfo.gameObject.SetActive(true);
                    msgTitle.gameObject.SetActive(true);
                    msgTitle.text = "Tab to select consume shards";
                    button.interactable = true;
                }
                else if (ctrl_InfoBlock.targetMtrch == null)
                {
                    isConsume.gameObject.SetActive(false);
                    isTarget.gameObject.SetActive(false);
                    shadeInfo.gameObject.SetActive(true);
                    msgTitle.gameObject.SetActive(true);
                    msgTitle.text = "Tab to select target shards";
                    button.interactable = true;
                }
                else
                {
                    isConsume.gameObject.SetActive(false);
                    isTarget.gameObject.SetActive(false);
                    shadeInfo.gameObject.SetActive(false);
                    msgTitle.gameObject.SetActive(false);
                    button.interactable = false;
                }
            }

            protected override void ButtonClick()
            {
                if (IsConsume)
                {
                    ctrl_InfoBlock.consumeMtrch = null;
                }
                else if (IsTarget)
                {
                    ctrl_InfoBlock.targetMtrch = null;
                }
                else if (ctrl_InfoBlock.consumeMtrch == null)
                {
                    ctrl_InfoBlock.consumeMtrch = this;
                }
                else if (ctrl_InfoBlock.targetMtrch == null)
                {
                    ctrl_InfoBlock.targetMtrch = this;
                }
                else
                {
                    
                }

                shardsCtrl.RefreshState();
            }
        }
    }
}