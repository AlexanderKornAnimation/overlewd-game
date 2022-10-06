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

                shardsCtrl.RefreshState();
            }

            protected override void DecClick()
            {
                base.DecClick();


                shardsCtrl.RefreshState();
            }

            public void RefreshState()
            {
                if (consumeMtrch == null || targetMtrch == null)
                {
                    msgTitle.text = "Tap to matriarchs whose shards you want to exchange";
                    msgTitle.gameObject.SetActive(true);
                    arrow.gameObject.SetActive(false);
                    consumeCell.gameObject.SetActive(false);
                    targetCell.gameObject.SetActive(false);
                }
                else
                {
                    msgTitle.gameObject.SetActive(false);
                    arrow.gameObject.SetActive(true);
                    consumeCell.gameObject.SetActive(true);
                    targetCell.gameObject.SetActive(true);

                    consumeSubstrate.sprite = GetShardSubstrate(shardsCtrl.ExchangeRaritySelected());
                    consumeIcon.sprite = GetShardIcon(consumeMtrch?.matriarchData);
                    targetSubstrate.sprite = GetShardSubstrate(shardsCtrl.ExchangeRaritySelected());
                    targetIcon.sprite = GetShardIcon(targetMtrch?.matriarchData);
                }
            }
        }
    }
}
