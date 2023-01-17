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

            wallet = WalletWidget.GetInstance(walletBack);
            UIManager.UnregisterWidget(wallet);

            UITools.BottomHide(walletBack as RectTransform);
        }

        public async void Show()
        {
            await UniTask.Delay(100);
            wallet.Customize(GameData.player.prevInfo);
            await UITools.BottomShowAsync(walletBack as RectTransform);
            wallet.ShowChangesAnim();
            await wallet.WaitChangesAnim();
            await UITools.BottomHideAsync(walletBack as RectTransform);
            Destroy(gameObject);
        }

        public static WalletChangeNotifWidget GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateScreenPrefab<WalletChangeNotifWidget>
                ("Prefabs/UI/Widgets/Notifications/WalletChangeNotifWidget", parent);
        }
    }
}

