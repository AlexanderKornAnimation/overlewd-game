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

                sideTabs = transform.Find("SideTabs");
                sideTabs.Find("Tabs/Basic").GetComponent<Button>().onClick.AddListener(() => SideTabClick(SideTabBasic));
                sideTabs.Find("Tabs/Advanced").GetComponent<Button>().onClick.AddListener(() => SideTabClick(SideTabAdvanced));
                sideTabs.Find("Tabs/Epic").GetComponent<Button>().onClick.AddListener(() => SideTabClick(SideTabEpic));
                sideTabs.Find("Tabs/Heroic").GetComponent<Button>().onClick.AddListener(() => SideTabClick(SideTabHeroic));

                var matrs = transform.Find("Matriarchs");
                mUlvi = matrs.Find("Ulvi").gameObject.AddComponent<MatriarchExchange>();
                mUlvi.matriarchKey = GameData.matriarchs.Ulvi.key;
                mAdriel = matrs.Find("Adriel").gameObject.AddComponent<MatriarchExchange>();
                mAdriel.matriarchKey = GameData.matriarchs.Adriel.key;
                mIngie = matrs.Find("Ingie").gameObject.AddComponent<MatriarchExchange>();
                mIngie.matriarchKey = GameData.matriarchs.Ingie.key;
                mFaye = matrs.Find("Faye").gameObject.AddComponent<MatriarchExchange>();
                mFaye.matriarchKey = GameData.matriarchs.Faye.key;
                mLili = matrs.Find("Lili").gameObject.AddComponent<MatriarchExchange>();
                mLili.matriarchKey = GameData.matriarchs.Lili.key;
            }

            private void Start()
            {

            }

            private void SideTabClick(int tabId)
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                sideTabs.Find("Selected/Basic").gameObject.SetActive(tabId == SideTabBasic);
                sideTabs.Find("Selected/Advanced").gameObject.SetActive(tabId == SideTabAdvanced);
                sideTabs.Find("Selected/Epic").gameObject.SetActive(tabId == SideTabEpic);
                sideTabs.Find("Selected/Heroic").gameObject.SetActive(tabId == SideTabHeroic);
            }

            protected override void MergeButtonClick()
            {

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
        }
    }
}
