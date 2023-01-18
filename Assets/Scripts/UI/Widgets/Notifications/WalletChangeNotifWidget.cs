using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;

namespace Overlewd
{
    public class WalletChangeNotifWidget : BaseWidget
    {
        private Transform walletBack;
        private WalletWidget wallet;

        protected override void Awake()
        {
            var canvas = transform.Find("Canvas");
            walletBack = canvas.Find("WalletBack");

            if (UIManager.GetWidgets<WalletWidget>().Count == 0)
            {
                wallet = WalletWidget.GetInstance(walletBack);
                UIManager.UnregisterWidget(wallet);
            }

            UITools.BottomHide(walletBack as RectTransform);
        }

        protected override void OnDestroy()
        {

        }

        public async void Show()
        {
            if (wallet != null)
            {
                await UniTask.Delay(100);
                wallet.Customize(GameData.player.prevInfo);
                await UITools.BottomShowAsync(walletBack as RectTransform);
                wallet.ShowChangesAnim();
                ShowAddPopups();
                await wallet.WaitChangesAnim();
                await UITools.BottomHideAsync(walletBack as RectTransform);
            }
            else
            {
                ShowAddPopups();
            }
            Destroy(gameObject);
        }

        private void ShowAddPopups()
        {
            var curInfo = GameData.player.info;
            var prevInfo = GameData.player.prevInfo;

            TryPopup(curInfo.Crystal.amount, prevInfo.Crystal.amount, GameData.currencies.Crystals.tmpSprite);
            TryPopup(curInfo.Wood.amount, prevInfo.Wood.amount, GameData.currencies.Wood.tmpSprite);
            TryPopup(curInfo.Stone.amount, prevInfo.Stone.amount, GameData.currencies.Stone.tmpSprite);
            TryPopup(curInfo.Copper.amount, prevInfo.Copper.amount, GameData.currencies.Copper.tmpSprite);
            TryPopup(curInfo.Gold.amount, prevInfo.Gold.amount, GameData.currencies.Gold.tmpSprite);
            TryPopup(curInfo.Gems.amount, prevInfo.Gems.amount, GameData.currencies.Gems.tmpSprite);
        }

        private void TryPopup(int curAmount, int prevAmount, string tmpSprite)
        {
            if (curAmount > prevAmount)
            {
                PopupNotifManager.PushNotif(new PopupNotifWidget.InitSettings
                {
                    title = "Add",
                    message = $"+{curAmount - prevAmount} {tmpSprite}"
                });
            }
        }

        public static WalletChangeNotifWidget GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateScreenPrefab<WalletChangeNotifWidget>
                ("Prefabs/UI/Widgets/Notifications/WalletChangeNotifWidget", parent);
        }
    }

    public static class WalletChangeNotifManager
    {
        public static void Show()
        {
            var notifInst = WalletChangeNotifWidget.GetInstance(UIManager.systemNotifRoot);
            notifInst.Show();
        }
    }
}

