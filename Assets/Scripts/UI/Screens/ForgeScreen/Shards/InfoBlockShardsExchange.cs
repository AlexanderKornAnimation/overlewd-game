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
                if (!isFilled)
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
                    consumeIcon.gameObject.SetActive(false);
                    targetIcon.gameObject.SetActive(false);

                    consumeSubstrate.sprite = GetShardSubstrate(shardsCtrl.ExchangeRaritySelected());
                    consumeShardIcon.sprite = GetShardIcon(consumeMtrch?.matriarchData);
                    targetSubstrate.sprite = GetShardSubstrate(shardsCtrl.ExchangeRaritySelected());
                    targetShardIcon.sprite = GetShardIcon(targetMtrch?.matriarchData);

                    //counter buttons
                    consumeCount.text = $"{shardsCount}/{shardsNeeded}";
                    UITools.DisableButton(decButton, targetCountValue == 1);
                    UITools.DisableButton(incButton, shardsNeeded >= maxShardsResult);
                }
            }

            public bool isFilled => consumeMtrch != null && targetMtrch != null;
            public int exchangeAmount => exchangeSettings.exchangeAmount;
            public int maxShardsResult => exchangeSettings.maxPossibleResultAmount;
            public int? shardsCount => 
                GameData.matriarchs.GetShardByMatriarchId(consumeMtrch?.matriarchData?.id,
                    shardsCtrl.ExchangeRaritySelected())?.amount;
            public int shardsNeeded => targetCountValue * exchangeAmount;
            public List<AdminBRO.PriceItem> exchangePrice => 
                UITools.PriceMul(exchangeSettings.GetPrice(shardsCtrl.ExchangeRaritySelected()),
                    targetCountValue);
            public bool canExchange =>
                GameData.player.CanBuy(exchangePrice) &&
                shardsCount >= shardsNeeded;
            public AdminBRO.ForgePrice.ExchangeShardSettings exchangeSettings =>
                GameData.buildings.forge.prices.exchangeShardSettings;
        }
    }
}
