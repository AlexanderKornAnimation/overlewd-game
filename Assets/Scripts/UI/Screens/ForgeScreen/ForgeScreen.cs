using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class ForgeScreen : BaseFullScreenParent<ForgeScreenInData>
    {
        public const int TabShard = 0;
        public const int TabBattleGirlsEquip = 1;
        public const int TabOverlordEquip = 2;

        private Transform tabArea;
        private Transform tabAreaShards;
        private Transform tabAreaGirls;
        private Transform tabAreaOverlord;
        private Button backButton;

        private WalletWidget walletWidget;
        private Transform shardsContent;
        private NSForgeScreen.ShardContentExchange shardsContentExhange;
        private NSForgeScreen.ShardContentMerge shardsContentMerge;
        private NSForgeScreen.BattleGirlsEquipContent battleGirlsEquipContent;
        private NSForgeScreen.OverlordEquipContent overlordEquipContent;

        private void Awake()
        {
            var screenInst =
                ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/ForgeScreen/ForgeScreen", transform);

            var canvas = screenInst.transform.Find("Canvas");
            tabArea = canvas.Find("TabArea");
            tabAreaShards = tabArea.Find("Shards");
            tabAreaGirls = tabArea.Find("BattleGirlsEquip");
            tabAreaOverlord = tabArea.Find("OverlordEquip");

            tabAreaShards.Find("Button").GetComponent<Button>().onClick.AddListener(ShardsTabClick);
            tabAreaShards.Find("TabOpened/MergeTab").GetComponent<Button>().onClick.AddListener(ShardsMergeTabClick);
            tabAreaShards.Find("TabOpened/ExchangeTab").GetComponent<Button>().onClick.AddListener(ShardsExchangeTabClick);
            tabAreaGirls.Find("Button").GetComponent<Button>().onClick.AddListener(BattleGirlsTabClick);
            tabAreaOverlord.Find("Button").GetComponent<Button>().onClick.AddListener(OverlordTabClick);

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);

            shardsContent = canvas.Find("ShardsContent");
            shardsContentExhange = shardsContent.Find("ShardsContentExchange").GetComponent<NSForgeScreen.ShardContentExchange>();
            shardsContentMerge = shardsContent.Find("ShardsContentMerge").GetComponent<NSForgeScreen.ShardContentMerge>();
            battleGirlsEquipContent = canvas.Find("BattleGirlsEquipContent").GetComponent<NSForgeScreen.BattleGirlsEquipContent>();
            overlordEquipContent = canvas.Find("OverlordContent").GetComponent<NSForgeScreen.OverlordEquipContent>();
            walletWidget = WalletWidget.GetInstance(canvas.Find("WalletWidgetPos"));
        }

        public override async Task BeforeShowMakeAsync()
        {
            switch (inputData?.activeTabId)
            {
                case TabShard:
                    ShardsTabClick();
                    break;
                case TabBattleGirlsEquip:
                    BattleGirlsTabClick();
                    break;
                case TabOverlordEquip:
                    OverlordTabClick();
                    break;
                default:
                    ShardsTabClick();
                    break;
            }

            await Task.CompletedTask;
        }

        public override async Task BeforeShowAsync()
        {
            SoundManager.GetEventInstance(FMODEventPath.Castle_Screen_BGM_Attn);
            await Task.CompletedTask;
        }

        public override async Task BeforeHideAsync()
        {
            SoundManager.StopAll();
            await Task.CompletedTask;
        }

        private void ShardsTabClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);

            tabAreaShards.Find("TabOpened").gameObject.SetActive(true);
            tabAreaShards.Find("Button/IconBack/IconBackSelected").gameObject.SetActive(true);
            shardsContent.gameObject.SetActive(true);

            tabAreaGirls.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 451);
            tabAreaGirls.Find("Button/IconBack/IconBackSelected").gameObject.SetActive(false);
            battleGirlsEquipContent.gameObject.SetActive(false);

            tabAreaOverlord.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 329);
            tabAreaOverlord.Find("Button/IconBack/IconBackSelected").gameObject.SetActive(false);
            overlordEquipContent.gameObject.SetActive(false);
        }

        private void ShardsMergeTabClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);

            tabAreaShards.Find("TabOpened/MergeTabSelected").gameObject.SetActive(true);
            tabAreaShards.Find("TabOpened/ExchangeTabSelected").gameObject.SetActive(false);
            shardsContentMerge.gameObject.SetActive(true);
            shardsContentExhange.gameObject.SetActive(false);
        }

        private void ShardsExchangeTabClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);

            tabAreaShards.Find("TabOpened/MergeTabSelected").gameObject.SetActive(false);
            tabAreaShards.Find("TabOpened/ExchangeTabSelected").gameObject.SetActive(true);
            shardsContentMerge.gameObject.SetActive(false);
            shardsContentExhange.gameObject.SetActive(true);
        }

        private void BattleGirlsTabClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);

            tabAreaShards.Find("TabOpened").gameObject.SetActive(false);
            tabAreaShards.Find("Button/IconBack/IconBackSelected").gameObject.SetActive(false);
            shardsContent.gameObject.SetActive(false);

            tabAreaGirls.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 672);
            tabAreaGirls.Find("Button/IconBack/IconBackSelected").gameObject.SetActive(true);
            battleGirlsEquipContent.gameObject.SetActive(true);

            tabAreaOverlord.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 550);
            tabAreaOverlord.Find("Button/IconBack/IconBackSelected").gameObject.SetActive(false);
            overlordEquipContent.gameObject.SetActive(false);
        }

        private void OverlordTabClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);

            tabAreaShards.Find("TabOpened").gameObject.SetActive(false);
            tabAreaShards.Find("Button/IconBack/IconBackSelected").gameObject.SetActive(false);
            shardsContent.gameObject.SetActive(false);

            tabAreaGirls.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 672);
            tabAreaGirls.Find("Button/IconBack/IconBackSelected").gameObject.SetActive(false);
            battleGirlsEquipContent.gameObject.SetActive(false);

            tabAreaOverlord.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 550);
            tabAreaOverlord.Find("Button/IconBack/IconBackSelected").gameObject.SetActive(true);
            overlordEquipContent.gameObject.SetActive(true);
        }
        
        private void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<CastleScreen>();
        }
    }

    public class ForgeScreenInData : BaseFullScreenInData
    {
        public int? activeTabId;
    }
}
