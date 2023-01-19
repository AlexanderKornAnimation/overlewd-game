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
        private List<WalletWidget> extWallets;
        private WalletWidget localWallet;

        protected override void Awake()
        {
            var canvas = transform.Find("Canvas");
            walletBack = canvas.Find("WalletBack");

            extWallets = UIManager.GetWidgets<WalletWidget>();
            if (extWallets.Count == 0)
            {
                localWallet = WalletWidget.GetInstance(walletBack);
            }
            UITools.BottomHide(walletBack as RectTransform);
        }

        public async void Show(WalletChangeStateDataEvent eventData)
        {
            WalletWidget.walletPlayerState = eventData.toInfo;

            if (localWallet != null)
            {
                await UITools.BottomShowAsync(walletBack as RectTransform);
                localWallet.ShowChangesAnim(eventData.fromInfo, eventData.toInfo);
                ShowAddPopups(eventData.fromInfo, eventData.toInfo);
                await UniTask.Delay(2000);
                await UITools.BottomHideAsync(walletBack as RectTransform);
            }
            else
            {
                extWallets.ForEach(w => w.ShowChangesAnim(eventData.fromInfo, eventData.toInfo));
                ShowAddPopups(eventData.fromInfo, eventData.toInfo);
            }
            Destroy(gameObject);
        }

        private void ShowAddPopups(AdminBRO.PlayerInfo from, AdminBRO.PlayerInfo to)
        {
            TryPopup(from.Crystal.amount, to.Crystal.amount, GameData.currencies.Crystals.tmpSprite);
            TryPopup(from.Wood.amount, to.Wood.amount, GameData.currencies.Wood.tmpSprite);
            TryPopup(from.Stone.amount, to.Stone.amount, GameData.currencies.Stone.tmpSprite);
            TryPopup(from.Copper.amount, to.Copper.amount, GameData.currencies.Copper.tmpSprite);
            TryPopup(from.Gold.amount, to.Gold.amount, GameData.currencies.Gold.tmpSprite);
            TryPopup(from.Gems.amount, to.Gems.amount, GameData.currencies.Gems.tmpSprite);
        }

        private void TryPopup(int prevAmount, int curAmount, string tmpSprite)
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
        public static void Show(WalletChangeStateDataEvent eventData)
        {
            var notifInst = WalletChangeNotifWidget.GetInstance(UIManager.systemNotifRoot);
            notifInst.Show(eventData);
        }
    }
}

