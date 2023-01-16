using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Overlewd
{
    public class WalletNotifWidget : BaseWidget
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
            await UITools.BottomShowAsync(walletBack as RectTransform);
            wallet.ShowChangesAnim();
            await wallet.WaitChangesAnim();
            await UITools.BottomHideAsync(walletBack as RectTransform);
            Destroy(gameObject);
        }

        public static WalletNotifWidget GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateScreenPrefab<WalletNotifWidget>
                ("Prefabs/UI/Widgets/Notifications/WalletNotifWidget", parent);
        }
    }
}

