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
        public class ShardContentMerge : BaseContent
        {
            public const int SideTabBasic = 0;
            public const int SideTabAdvanced = 1;
            public const int SideTabEpic = 2;

            private InfoBlockShardsMerge infoBlock;

            private Transform sideTabs;

            private MatriarchMerge mUlvi;
            private MatriarchMerge mAdriel;
            private MatriarchMerge mIngie;
            private MatriarchMerge mFaye;
            private MatriarchMerge mLili;

            protected override void Awake()
            {
                base.Awake();

                infoBlock = transform.Find("InfoBlock").GetComponent<InfoBlockShardsMerge>();

                sideTabs = transform.Find("SideTabs");
                sideTabs.Find("Tabs/Basic").GetComponent<Button>().onClick.AddListener(() => SideTabClick(SideTabBasic));
                sideTabs.Find("Tabs/Advanced").GetComponent<Button>().onClick.AddListener(() => SideTabClick(SideTabAdvanced));
                sideTabs.Find("Tabs/Epic").GetComponent<Button>().onClick.AddListener(() => SideTabClick(SideTabEpic));

                var matrs = transform.Find("Matriarchs");
                mUlvi = matrs.Find("Ulvi").gameObject.AddComponent<MatriarchMerge>();
                mUlvi.matriarchKey = GameData.matriarchs.Ulvi.key;
                mAdriel = matrs.Find("Adriel").gameObject.AddComponent<MatriarchMerge>();
                mAdriel.matriarchKey = GameData.matriarchs.Adriel.key;
                mIngie = matrs.Find("Ingie").gameObject.AddComponent<MatriarchMerge>();
                mIngie.matriarchKey = GameData.matriarchs.Ingie.key;
                mFaye = matrs.Find("Faye").gameObject.AddComponent<MatriarchMerge>();
                mFaye.matriarchKey = GameData.matriarchs.Faye.key;
                mLili = matrs.Find("Lili").gameObject.AddComponent<MatriarchMerge>();
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
