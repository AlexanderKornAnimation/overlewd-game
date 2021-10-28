using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class MarketScreen : BaseScreen
    {

        void Start()
        {
            var screenPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/MarketScreen/Market"));
            var screenRectTransform = screenPrefab.GetComponent<RectTransform>();
            screenRectTransform.SetParent(transform, false);
            UIManager.SetStretch(screenRectTransform);


            screenRectTransform.Find("CanvasRoot").Find("MainMenuButton").GetComponent<Button>().onClick.AddListener(() => 
            {
                UIManager.ShowScreen<CastleScreen>();
            });

            var bundlesGrid = screenRectTransform.Find("CanvasRoot").Find("BottomGrid");
            NSMarketScreen.BundleTypeA.GetInstance(bundlesGrid);
            NSMarketScreen.BundleTypeB.GetInstance(bundlesGrid);
            NSMarketScreen.BundleTypeC.GetInstance(bundlesGrid);
            NSMarketScreen.BundleTypeD.GetInstance(bundlesGrid);
        }

        private void AddResourceToGrid(Texture2D texture, Transform grid, string name = "")
        {
            var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            var resPrefab = (GameObject)Instantiate(Resources.Load("Prefabs/UI/Screens/MarketScreen/ResourceItem"));
            var image = resPrefab.transform.Find("RootCanvas").Find("Image").GetComponent<Image>();
            image.sprite = sprite;
            var text = resPrefab.transform.Find("RootCanvas").Find("Image").Find("Text").GetComponent<Text>();
            text.text = name;
            resPrefab.transform.SetParent(grid, false);
        }

        void Update()
        {

        }
    }
}
