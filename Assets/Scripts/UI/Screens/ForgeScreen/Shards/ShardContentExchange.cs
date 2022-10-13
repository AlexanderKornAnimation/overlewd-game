using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSForgeScreen
    {
        public class ShardContentExchange : BaseContent
        {
            public const int SideTabBasic = 0;
            public const int SideTabAdvanced = 1;
            public const int SideTabEpic = 2;
            public const int SideTabHeroic = 3;

            private InfoBlockShardsExchange infoBlock;

            private Transform sideTabs;

            private MatriarchExchange mUlvi;
            private MatriarchExchange mAdriel;
            private MatriarchExchange mIngie;
            private MatriarchExchange mFaye;
            private MatriarchExchange mLili;

            protected override void Awake()
            {
                base.Awake();

                infoBlock = transform.Find("InfoBlock").GetComponent<InfoBlockShardsExchange>();
                infoBlock.shardsCtrl = this;

                sideTabs = transform.Find("SideTabs");
                sideTabs.Find("Tabs/Basic").GetComponent<Button>().onClick.AddListener(() => SideTabClick(SideTabBasic));
                sideTabs.Find("Tabs/Advanced").GetComponent<Button>().onClick.AddListener(() => SideTabClick(SideTabAdvanced));
                sideTabs.Find("Tabs/Epic").GetComponent<Button>().onClick.AddListener(() => SideTabClick(SideTabEpic));
                sideTabs.Find("Tabs/Heroic").GetComponent<Button>().onClick.AddListener(() => SideTabClick(SideTabHeroic));

                var matrs = transform.Find("Matriarchs");
                mUlvi = matrs.Find("Ulvi").gameObject.AddComponent<MatriarchExchange>();
                mUlvi.matriarchKey = GameData.matriarchs.Ulvi.key;
                mUlvi.shardsCtrl = this;
                mUlvi.ctrl_InfoBlock = infoBlock;

                mAdriel = matrs.Find("Adriel").gameObject.AddComponent<MatriarchExchange>();
                mAdriel.matriarchKey = GameData.matriarchs.Adriel.key;
                mAdriel.shardsCtrl = this;
                mAdriel.ctrl_InfoBlock = infoBlock;

                mIngie = matrs.Find("Ingie").gameObject.AddComponent<MatriarchExchange>();
                mIngie.matriarchKey = GameData.matriarchs.Ingie.key;
                mIngie.shardsCtrl = this;
                mIngie.ctrl_InfoBlock = infoBlock;

                mFaye = matrs.Find("Faye").gameObject.AddComponent<MatriarchExchange>();
                mFaye.matriarchKey = GameData.matriarchs.Faye.key;
                mFaye.shardsCtrl = this;
                mFaye.ctrl_InfoBlock = infoBlock;

                mLili = matrs.Find("Lili").gameObject.AddComponent<MatriarchExchange>();
                mLili.matriarchKey = GameData.matriarchs.Lili.key;
                mLili.shardsCtrl = this;
                mLili.ctrl_InfoBlock = infoBlock;
            }

            private void Start()
            {
                RefreshState();
            }

            private void OnEnable()
            {
                RefreshState();
            }

            private void SideTabClick(int tabId)
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                sideTabs.Find("Selected/Basic").gameObject.SetActive(tabId == SideTabBasic);
                sideTabs.Find("Selected/Advanced").gameObject.SetActive(tabId == SideTabAdvanced);
                sideTabs.Find("Selected/Epic").gameObject.SetActive(tabId == SideTabEpic);
                sideTabs.Find("Selected/Heroic").gameObject.SetActive(tabId == SideTabHeroic);

                RefreshState();
            }

            protected override void MergeButtonClick()
            {
                Exchange();
            }

            protected override void PortalButtonClick()
            {
                UIManager.MakeScreen<PortalScreen>().
                SetData(new PortalScreenInData
                {
                    activeButtonId = PortalScreen.TabShards
                }).RunShowScreenProcess();
            }

            protected override void MarketButtonClick()
            {
                UIManager.ShowOverlay<MarketOverlay>();
            }

            public void RefreshState()
            {
                infoBlock.RefreshState();
                mUlvi.RefreshState();
                mAdriel.RefreshState();
                mIngie.RefreshState();
                mFaye.RefreshState();
                mLili.RefreshState();

                //merge button
                mergeButton.gameObject.SetActive(infoBlock.isFilled);
                SetMergeBtnPrice(infoBlock.exchangePrice);
                UITools.DisableButton(mergeButton, !infoBlock.canExchange);
            }

            public string ExchangeRaritySelected()
            {
                var selectedRarity = AdminBRO.Rarity.Basic;
                selectedRarity = sideTabs.Find("Selected/Basic").gameObject.activeSelf ? AdminBRO.Rarity.Basic : selectedRarity;
                selectedRarity = sideTabs.Find("Selected/Advanced").gameObject.activeSelf ? AdminBRO.Rarity.Advanced : selectedRarity;
                selectedRarity = sideTabs.Find("Selected/Epic").gameObject.activeSelf ? AdminBRO.Rarity.Epic : selectedRarity;
                selectedRarity = sideTabs.Find("Selected/Heroic").gameObject.activeSelf ? AdminBRO.Rarity.Heroic : selectedRarity;
                return selectedRarity;
            }

            private async void Exchange()
            {
                await GameData.buildings.forge.ExchangeShard(
                    infoBlock.consumeMtrch.matriarchData.id,
                    infoBlock.targetMtrch.matriarchData.id,
                    ExchangeRaritySelected(),
                    infoBlock.shardsNeeded);
                RefreshState();
            }
        }
    }
}
