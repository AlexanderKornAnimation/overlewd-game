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
                    msgTitle.text = "Tap to matriarchs whose shards you want to merge";
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

                    consumeIcon.sprite = GetShardIcon(consumeMtrch?.matriarchData);
                    targetIcon.sprite = GetShardIcon(targetMtrch?.matriarchData);
                    switch (shardsCtrl.MergeRaritySelected())
                    {
                        case AdminBRO.Rarity.Basic:
                            consumeSubstrate.sprite = GetShardSubstrate(AdminBRO.Rarity.Basic);
                            targetSubstrate.sprite = GetShardSubstrate(AdminBRO.Rarity.Advanced);
                            break;
                        case AdminBRO.Rarity.Advanced:
                            consumeSubstrate.sprite = GetShardSubstrate(AdminBRO.Rarity.Advanced);
                            targetSubstrate.sprite = GetShardSubstrate(AdminBRO.Rarity.Epic);
                            break;
                        case AdminBRO.Rarity.Epic:
                            consumeSubstrate.sprite = GetShardSubstrate(AdminBRO.Rarity.Epic);
                            targetSubstrate.sprite = GetShardSubstrate(AdminBRO.Rarity.Heroic);
                            break;
                    }
                }
            }
        }
    }
}
