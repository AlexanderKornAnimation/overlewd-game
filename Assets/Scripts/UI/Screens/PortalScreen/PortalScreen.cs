using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Overlewd
{
    public class PortalScreen : BaseFullScreen
    {
        private Button shardsButton;
        private Image shardsButtonSelected;
        private Button battleGirlsButton;
        private Image battleGirlsButtonSelected;
        private Button battleGirlsEquipButton;
        private Image battleGirlsEquipButtonSelected;
        private Button overlordEquipButton;
        private Image overlordEquipButtonSelected;
        private Button backButton;

        private GameObject battleGirlsEquipContent;
        private GameObject shardsContent;
        private GameObject battleGirlsContent;
        private GameObject overlordEquipContent;

        private NSPortalScreen.BaseBanner selectedBanner;

        private List<NSPortalScreen.BaseBanner> battleGirlsBanners = new List<NSPortalScreen.BaseBanner>();
        private List<NSPortalScreen.BaseBanner> battleGirlsEquipBanners = new List<NSPortalScreen.BaseBanner>();
        private List<NSPortalScreen.BaseBanner> overlordBanners = new List<NSPortalScreen.BaseBanner>();
        private List<NSPortalScreen.BaseBanner> memoryBanners = new List<NSPortalScreen.BaseBanner>();

        protected virtual void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab("Prefabs/UI/Screens/PortalScreen/PortalScreen", transform);

            var canvas = screenInst.transform.Find("Canvas");
            var tabArea = canvas.Find("TabArea");
            var bannersBack = canvas.Find("BannersBackground");

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);
            
            shardsButton = tabArea.Find("ShardsButton").GetComponent<Button>();
            shardsButton.onClick.AddListener(ShardsButtonClick);
            shardsButtonSelected = shardsButton.transform.Find("ButtonSelected").GetComponent<Image>();
            
            battleGirlsButton = tabArea.Find("BattleGirlsButton").GetComponent<Button>();
            battleGirlsButton.onClick.AddListener(BattleGirlsButtonClick);
            battleGirlsButtonSelected = battleGirlsButton.transform.Find("ButtonSelected").GetComponent<Image>();

            battleGirlsEquipButton = tabArea.Find("BattleGirlsEquipButton").GetComponent<Button>();
            battleGirlsEquipButton.onClick.AddListener(BattleGirlsEquipButtonClick);
            battleGirlsEquipButtonSelected = battleGirlsEquipButton.transform.Find("ButtonSelected").GetComponent<Image>();
            
            overlordEquipButton = tabArea.Find("OverlordEquipButton").GetComponent<Button>();
            overlordEquipButton.onClick.AddListener(OverlordEquipButtonClick);
            overlordEquipButtonSelected = overlordEquipButton.transform.Find("ButtonSelected").GetComponent<Image>();

            shardsContent = bannersBack.Find("ShardsContent").gameObject;
            battleGirlsContent = bannersBack.Find("BattleGirlsContent").gameObject;
            overlordEquipContent = bannersBack.Find("OverlordEquipContent").gameObject;
            battleGirlsEquipContent = bannersBack.Find("BattleGirlsEquipContent").gameObject;
        }

        protected virtual void Start()
        {
            Customize();

            BattleGirlsButtonClick();
        }

        protected virtual void AddBanner(NSPortalScreen.BaseBanner newBanner, List<NSPortalScreen.BaseBanner> banners)
        {
            newBanner.selectBanner += SelectBanner;
            
            SetPosition(newBanner, banners);
            banners.Add(newBanner);
        }

        private void SetPosition(NSPortalScreen.BaseBanner banner, List<NSPortalScreen.BaseBanner> banners)
        {
            const float offsetX = 586f;

            if (banners.Count == 0)
                return;
            
            var lastBannerPos = banners.Last().GetComponent<RectTransform>().anchoredPosition;
            var bannerRect = banner.GetComponent<RectTransform>();
            bannerRect.anchoredPosition = new Vector2(lastBannerPos.x + offsetX, lastBannerPos.y);
        }
        
        protected virtual void Customize()
        {
            foreach (var gacha in GameData.gacha.items)
            {
                switch (gacha.gachaType)
                {
                    case (AdminBRO.GachItem.Type_Linear, AdminBRO.GachItem.TabType_OverlordEquipment):
                        var bannerTypeA = NSPortalScreen.BannerTypeA.GetInstance(overlordEquipContent.transform);
                        bannerTypeA.gachaItem = gacha;
                        AddBanner(bannerTypeA, overlordBanners);
                        break;
                    case (AdminBRO.GachItem.Type_Stepwise, AdminBRO.GachItem.TabType_OverlordEquipment):
                        var bannerTypeB = NSPortalScreen.BannerTypeB.GetInstance(overlordEquipContent.transform);
                        bannerTypeB.gachaItem = gacha;
                        AddBanner(bannerTypeB, overlordBanners);
                        break;
                }
            }
        }
        
        private void MoveBanners(NSPortalScreen.BaseBanner banner, List<NSPortalScreen.BaseBanner> banners)
        {
            var isLeftDirection = banner.GetComponent<RectTransform>().anchoredPosition.x >
                                  selectedBanner?.GetComponent<RectTransform>().anchoredPosition.x;

            var bannerRect = banner.GetComponent<RectTransform>();
            
            while (bannerRect.anchoredPosition.x != 0)
            {
                foreach (var _banner in banners)
                {
                    if(isLeftDirection)
                        _banner.MoveLeft();
                    else
                        _banner.MoveRight();
                }
            }
        }
        
        protected virtual void SelectBanner(NSPortalScreen.BaseBanner banner)
        {
            selectedBanner?.Deselect();

            switch (banner.gachaItem.tabType)
            {
                case AdminBRO.GachItem.TabType_OverlordEquipment:
                    MoveBanners(banner,overlordBanners);
                    break;
            }
            
            selectedBanner = banner;
            selectedBanner?.Select();
        }
        
        protected virtual void ShardsButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            shardsButtonSelected.gameObject.SetActive(true);
            shardsContent.SetActive(true);
            
            battleGirlsButtonSelected.gameObject.SetActive(false);
            battleGirlsContent.SetActive(false);
            
            overlordEquipButtonSelected.gameObject.SetActive(false);
            overlordEquipContent.SetActive(false);
            
            battleGirlsEquipButtonSelected.gameObject.SetActive(false);
            battleGirlsEquipContent.SetActive(false);
            
            
            if (memoryBanners.Any())
            {
                SelectBanner(memoryBanners.First());
            }
        }

        private void BattleGirlsEquipButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            battleGirlsEquipButtonSelected.gameObject.SetActive(true);
            battleGirlsContent.SetActive(true);
            
            shardsButtonSelected.gameObject.SetActive(false);
            shardsContent.SetActive(false);

            battleGirlsButtonSelected.gameObject.SetActive(false);
            battleGirlsContent.SetActive(false);

            overlordEquipButtonSelected.gameObject.SetActive(false);
            overlordEquipContent.SetActive(false);
            
            if (battleGirlsBanners.Any())
            {
                SelectBanner(battleGirlsBanners.First());
            }
        }
        
        protected virtual void BattleGirlsButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            battleGirlsButtonSelected.gameObject.SetActive(true);
            battleGirlsContent.SetActive(true);
            
            shardsButtonSelected.gameObject.SetActive(false);
            shardsContent.SetActive(false);

            overlordEquipButtonSelected.gameObject.SetActive(false);
            overlordEquipContent.SetActive(false);
            
            battleGirlsEquipButtonSelected.gameObject.SetActive(false);
            battleGirlsContent.SetActive(false);
            
            if (battleGirlsEquipBanners.Any())
            {
                SelectBanner(battleGirlsEquipBanners.First());
            }
        }

        protected virtual void OverlordEquipButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            overlordEquipButtonSelected.gameObject.SetActive(true);
            overlordEquipContent.SetActive(true);
            
            shardsButtonSelected.gameObject.SetActive(false);
            shardsContent.SetActive(false);

            battleGirlsButtonSelected.gameObject.SetActive(false);
            battleGirlsContent.SetActive(false);

            battleGirlsEquipButtonSelected.gameObject.SetActive(false);
            battleGirlsContent.SetActive(false);
            
            if (overlordBanners.Any())
            {
                SelectBanner(overlordBanners.First());
            }
        }

        private void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<CastleScreen>();
        }

    }
}
