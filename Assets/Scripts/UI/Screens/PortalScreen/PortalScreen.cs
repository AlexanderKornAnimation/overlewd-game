using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Overlewd
{
    public class PortalScreen : BaseScreen
    {
        private Button memoriesButton;
        private Image memoriesButtonSelected;
        private Button battleGirlsButton;
        private Image battleGirlsButtonSelected;
        private Button equipButton;
        private Image equipButtonSelected;

        private GameObject memoriesScroll;
        private Transform memoriesScrollContent;
        private GameObject battleGirlsScroll;
        private Transform battleGirlsScrollContent;
        private GameObject equipScroll;
        private Transform equipScrollContent;

        private Transform tierButtonsScrollPos;

        private NSPortalScreen.BaseBanner selectedBanner;

        private List<NSPortalScreen.BaseBanner> banners = new List<NSPortalScreen.BaseBanner>();

        void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/PortalScreen/PortalScreen", transform);

            var canvas = screenInst.transform.Find("Canvas");
            var tabArea = canvas.Find("TabArea");

            memoriesButton = tabArea.Find("MemoriesButton").GetComponent<Button>();
            memoriesButton.onClick.AddListener(MemoriesButtonClick);
            memoriesButtonSelected = memoriesButton.transform.Find("ButtonSelected").GetComponent<Image>();
            battleGirlsButton = tabArea.Find("BattleGirlsButton").GetComponent<Button>();
            battleGirlsButton.onClick.AddListener(BattleGirlsButtonClick);
            battleGirlsButtonSelected = battleGirlsButton.transform.Find("ButtonSelected").GetComponent<Image>();
            equipButton = tabArea.Find("EquipButton").GetComponent<Button>();
            equipButton.onClick.AddListener(EquipButtonClick);
            equipButtonSelected = equipButton.transform.Find("ButtonSelected").GetComponent<Image>();

            memoriesScroll = canvas.Find("MemoriesScroll").gameObject;
            memoriesScrollContent = memoriesScroll.transform.Find("Viewport").Find("Content");
            battleGirlsScroll = canvas.Find("BattleGirlsScroll").gameObject;
            battleGirlsScrollContent = battleGirlsScroll.transform.Find("Viewport").Find("Content");
            equipScroll = canvas.Find("EquipScroll").gameObject;
            equipScrollContent = equipScroll.transform.Find("Viewport").Find("Content");

            tierButtonsScrollPos = canvas.Find("TierButtonsScrollPos");
        }

        void Start()
        {
            SidebarButtonWidget.GetInstance(transform);

            Customize();

            MemoriesButtonClick();
        }

        private void AddBanner(NSPortalScreen.BaseBanner newBanner)
        {
            newBanner.selectBanner += SelectBanner;
            newBanner.tierButtonsScroll = NSPortalScreen.TierButtonsScroll.GetInstance(tierButtonsScrollPos);

            banners.Add(newBanner);
        }

        private void Customize()
        {
            AddBanner(NSPortalScreen.BattleGirlsBannerTypeA.GetInstance(memoriesScrollContent));
            AddBanner(NSPortalScreen.BattleGirlsBannerTypeB.GetInstance(memoriesScrollContent));
            AddBanner(NSPortalScreen.PremiumSummonBanner.GetInstance(memoriesScrollContent));
            AddBanner(NSPortalScreen.EventSummonBanner.GetInstance(memoriesScrollContent));

            AddBanner(NSPortalScreen.BattleGirlsBannerTypeA.GetInstance(battleGirlsScrollContent));
            AddBanner(NSPortalScreen.PremiumSummonBanner.GetInstance(battleGirlsScrollContent));

            AddBanner(NSPortalScreen.PremiumSummonBanner.GetInstance(equipScrollContent));
            AddBanner(NSPortalScreen.BattleGirlsBannerTypeA.GetInstance(equipScrollContent));
            AddBanner(NSPortalScreen.BattleGirlsBannerTypeB.GetInstance(equipScrollContent));

            if (banners.Any())
            {
                SelectBanner(banners.First());
            }
        }

        private void MemoriesButtonClick()
        {
            SoundManager.PlayOneShoot(SoundPath.UI_GenericButtonClick);
            memoriesButtonSelected.gameObject.SetActive(true);
            memoriesScroll.SetActive(true);

            battleGirlsButtonSelected.gameObject.SetActive(false);
            battleGirlsScroll.SetActive(false);

            equipButtonSelected.gameObject.SetActive(false);
            equipScroll.SetActive(false);
        }

        private void BattleGirlsButtonClick()
        {
            SoundManager.PlayOneShoot(SoundPath.UI_GenericButtonClick);
            memoriesButtonSelected.gameObject.SetActive(false);
            memoriesScroll.SetActive(false);

            battleGirlsButtonSelected.gameObject.SetActive(true);
            battleGirlsScroll.SetActive(true);

            equipButtonSelected.gameObject.SetActive(false);
            equipScroll.SetActive(false);
        }

        private void EquipButtonClick()
        {
            SoundManager.PlayOneShoot(SoundPath.UI_GenericButtonClick);
            memoriesButtonSelected.gameObject.SetActive(false);
            memoriesScroll.SetActive(false);

            battleGirlsButtonSelected.gameObject.SetActive(false);
            battleGirlsScroll.SetActive(false);

            equipButtonSelected.gameObject.SetActive(true);
            equipScroll.SetActive(true);
        }

        private void SelectBanner(NSPortalScreen.BaseBanner banner)
        {
            selectedBanner?.Deselect();
            selectedBanner = banner;
            selectedBanner?.Select();
        }
    }

}
