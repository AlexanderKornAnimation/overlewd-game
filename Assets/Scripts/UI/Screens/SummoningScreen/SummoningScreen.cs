using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class SummoningScreen : BaseFullScreenParent<SummoningScreenInData>
    {
        private Button portalButton;
        private Button summonButton;
        private TextMeshProUGUI summonButtonText;
        private Transform canvas;
        private Transform shardsPos;

        private SpineScene portalFullScreenAnim;
        private NSSummoningScreen.BaseShardsAnimCtrl animCtrl;

        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/SummoningScreen/SummoningScreen", transform);

            canvas = screenInst.transform.Find("Canvas");
            portalButton = canvas.Find("PortalButton").GetComponent<Button>();
            portalButton.onClick.AddListener(PortalButtonClick);

            summonButton = canvas.Find("SummonButton").GetComponent<Button>();
            summonButtonText = summonButton.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            summonButton.onClick.AddListener(SummonButtonClick);

            shardsPos = canvas.Find("ShardsPos");
        }

        public override async Task AfterShowAsync()
        {
            portalFullScreenAnim.Play();
            await UniTask.WaitUntil(() => portalFullScreenAnim.isComplete);
            Destroy(portalFullScreenAnim.gameObject);

            MakeShardsAnimCtrl(inputData.isMany, inputData.tabType, inputData.summonData);

            await Task.CompletedTask;
        }

        public override async Task BeforeShowMakeAsync()
        {
            portalFullScreenAnim = SpineScene.GetInstance(GameData.animations["gacha_portal_scene1"], shardsPos, false);
            portalFullScreenAnim.Pause();

            portalButton.gameObject.SetActive(false);
            summonButton.gameObject.SetActive(false);

            await Task.CompletedTask;
        }

        private async void SummonButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);

            portalButton.gameObject.SetActive(false);
            summonButton.gameObject.SetActive(false);

            animCtrl.HideShards();
            await UniTask.Delay(1400);
            Destroy(animCtrl?.gameObject);

            var summonData = inputData.isMany ?
                await GameData.gacha.BuyMany(inputData.gachaId) :
                await GameData.gacha.Buy(inputData.gachaId);

            MakeShardsAnimCtrl(inputData.isMany, inputData.tabType, summonData);
        }

        private void PortalButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.MakeScreen<PortalScreen>().
                SetData(new PortalScreenInData
                {
                    activeGachaId = inputData?.gachaId
                })
                .DoShow();
        }

        private IEnumerator WaitShardsIsOpened()
        {
            while (!(animCtrl?.IsCompleteOpened ?? true))
            {
                yield return new WaitForSeconds(1.0f);
            }

            portalButton.gameObject.SetActive(true);
            summonButton.gameObject.SetActive(true);
            PortalScreenHelper.MakeSummonButton(inputData.gachaData, inputData.isMany, summonButton, summonButtonText);

            switch (GameData.ftue.stats.lastEndedStageData?.lerningKey)
            {
                case (FTUE.CHAPTER_2, FTUE.DIALOGUE_1):
                    GameData.ftue.chapter2.ShowNotifByKey("ch2gachatutor2");
                    break;
                case (FTUE.CHAPTER_2, FTUE.DIALOGUE_3):
                    UIManager.MakeScreen<MemoryScreen>().
                        SetData(new MemoryScreenInData
                        {
                            girlKey = AdminBRO.MatriarchItem.Key_Ulvi,
                        }).DoShow();
                    break;
                case (FTUE.CHAPTER_3, FTUE.SEX_1):
                    UIManager.ShowScreen<OverlordScreen>();
                    break;
            }
        }

        private void MakeShardsAnimCtrl(bool many, string gachaTabType, List<AdminBRO.GachaBuyResult> summonData)
        {
            if (many)
            {
                SoundManager.PlayOneShot(FMODEventPath.Gacha_x10_open);
                animCtrl = NSSummoningScreen.GroupShardsAnimCtrl.GetInstance(shardsPos);
            }
            else
            {
                SoundManager.PlayOneShot(FMODEventPath.Gacha_x1_open);
                animCtrl = NSSummoningScreen.SingleShardAnimCtrl.GetInstance(shardsPos);
            }
            animCtrl?.SetShardsData(SummoningScreenShardsData.FromSummonData(gachaTabType, summonData));

            StartCoroutine(WaitShardsIsOpened());
        }
    }

    public class SummoningScreenInData : BaseFullScreenInData
    {
        public int? gachaId;
        public string tabType;
        public bool isMany;
        public List<AdminBRO.GachaBuyResult> summonData;

        public AdminBRO.GachaItem gachaData =>
            GameData.gacha.GetGachaById(gachaId);
    }

    public class SummoningScreenShardsData
    {
        public List<Shard> shards = new List<Shard>();
        public string type;

        public bool isBattleCharactersType => type == AdminBRO.GachaItem.TabType_Characters;
        public bool isEquipmentsType => type == AdminBRO.GachaItem.TabType_CharactersEquipment ||
            type == AdminBRO.GachaItem.TabType_OverlordEquipment;
        public bool isMemoriesType => type == AdminBRO.GachaItem.TabType_MatriachsShards;

        public static SummoningScreenShardsData FromSummonData(string gachaTabType,
            List<AdminBRO.GachaBuyResult> summonData)
        {
            var result = new SummoningScreenShardsData();
            result.type = gachaTabType;
            if (summonData != null)
            {
                foreach (var s in summonData)
                {
                    result.shards.Add(new Shard
                    {
                        icon = ResourceManager.LoadSprite(s.icon),
                        rarity = s.rarity
                    });
                }
            }
            return result;
        }

        public class Shard
        {
            public Sprite icon;
            public string rarity;

            public bool isBasic => rarity == AdminBRO.Rarity.Basic;
            public bool isAdvanced => rarity == AdminBRO.Rarity.Advanced;
            public bool isEpic => rarity == AdminBRO.Rarity.Epic;
            public bool isHeroic => rarity == AdminBRO.Rarity.Heroic;
        }
    }
}
