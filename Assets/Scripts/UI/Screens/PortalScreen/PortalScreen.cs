using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Overlewd
{
    public class PortalScreen : BaseFullScreen
    {
        protected Button memoriesButton;
        protected Image memoriesButtonSelected;
        protected Button battleGirlsButton;
        protected Image battleGirlsButtonSelected;
        protected Button equipButton;
        protected Image equipButtonSelected;

        protected GameObject memoriesScroll;
        protected Transform memoriesScrollContent;
        protected GameObject battleGirlsScroll;
        protected Transform battleGirlsScrollContent;
        protected GameObject equipScroll;
        protected Transform equipScrollContent;

        protected Transform tierButtonsScrollPos;

        protected NSPortalScreen.BaseBanner selectedBanner;

        protected List<NSPortalScreen.BaseBanner> banners = new List<NSPortalScreen.BaseBanner>();

        protected virtual void Awake()
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

        protected virtual void Start()
        {
            SidebarButtonWidget.GetInstance(transform);

            Customize();

            MemoriesButtonClick();
        }

        protected virtual void AddBanner(NSPortalScreen.BaseBanner newBanner)
        {
            newBanner.selectBanner += SelectBanner;
            newBanner.tierButtonsScroll = NSPortalScreen.TierButtonsScroll.GetInstance(tierButtonsScrollPos);

            banners.Add(newBanner);
        }

        protected virtual void Customize()
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

        protected virtual void MemoriesButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            memoriesButtonSelected.gameObject.SetActive(true);
            memoriesScroll.SetActive(true);

            battleGirlsButtonSelected.gameObject.SetActive(false);
            battleGirlsScroll.SetActive(false);

            equipButtonSelected.gameObject.SetActive(false);
            equipScroll.SetActive(false);
        }

        protected virtual void BattleGirlsButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            memoriesButtonSelected.gameObject.SetActive(false);
            memoriesScroll.SetActive(false);

            battleGirlsButtonSelected.gameObject.SetActive(true);
            battleGirlsScroll.SetActive(true);

            equipButtonSelected.gameObject.SetActive(false);
            equipScroll.SetActive(false);
        }

        protected virtual void EquipButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            memoriesButtonSelected.gameObject.SetActive(false);
            memoriesScroll.SetActive(false);

            battleGirlsButtonSelected.gameObject.SetActive(false);
            battleGirlsScroll.SetActive(false);

            equipButtonSelected.gameObject.SetActive(true);
            equipScroll.SetActive(true);
        }

        protected virtual void SelectBanner(NSPortalScreen.BaseBanner banner)
        {
            selectedBanner?.Deselect();
            selectedBanner = banner;
            selectedBanner?.Select();
        }
    }

}
