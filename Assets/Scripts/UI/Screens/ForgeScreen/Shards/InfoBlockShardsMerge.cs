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
                if (!isFilled)
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
                    consumeIcon.gameObject.SetActive(false);
                    targetIcon.gameObject.SetActive(false);

                    consumeShardIcon.sprite = GetShardIcon(consumeMtrch?.matriarchData);
                    targetShardIcon.sprite = GetShardIcon(consumeMtrch?.matriarchData);
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

                    //counter buttons
                    consumeCount.text = $"{shardsCount}/{shardsNeeded}";
                    UITools.DisableButton(decButton, targetCountValue == 1);
                    UITools.DisableButton(incButton, shardsNeeded >= maxShardsResult);
                }
            }

            public bool isFilled => consumeMtrch != null;
            public int mergeAmount =>
                GameData.buildings.forge.prices.mergeShardSettings.mergeAmount;
            public int maxShardsResult =>
                GameData.buildings.forge.prices.mergeShardSettings.maxPossibleResultAmount;
            public int? shardsCount =>
                GameData.matriarchs.GetShardByMatriarchId(consumeMtrch?.matriarchData?.id,
                    shardsCtrl.MergeRaritySelected())?.amount;
            public int shardsNeeded => targetCountValue * mergeAmount;
            public List<AdminBRO.PriceItem> mergePrice =>
                UITools.PriceMul(GameData.buildings.forge.prices.mergeShardSettings.
                    GetPrice(shardsCtrl.MergeRaritySelected()),
                    targetCountValue);
            public bool canMerge =>
                GameData.player.CanBuy(mergePrice) &&
                shardsCount >= shardsNeeded;
        }
    }
}
