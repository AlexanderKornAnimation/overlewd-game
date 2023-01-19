using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Overlewd
{
    public class WalletWidget : BaseWidget
    {
        private static AdminBRO.PlayerInfo _walletPlayerState;
        public static AdminBRO.PlayerInfo walletPlayerState
        {
            get => _walletPlayerState ?? GameData.player.info;
            set => _walletPlayerState = value;
        }

        private TextMeshProUGUI crystal;
        private TextMeshProUGUI wood;
        private TextMeshProUGUI stone;
        private TextMeshProUGUI copper;
        private TextMeshProUGUI gold;
        private TextMeshProUGUI gems;

        protected override void Awake()
        {
            base.Awake();

            crystal = transform.Find("Crystal").GetComponent<TextMeshProUGUI>();
            wood = transform.Find("Wood").GetComponent<TextMeshProUGUI>();
            stone = transform.Find("Stone").GetComponent<TextMeshProUGUI>();
            copper = transform.Find("Copper").GetComponent<TextMeshProUGUI>();
            gold = transform.Find("Gold").GetComponent<TextMeshProUGUI>();
            gems = transform.Find("Gems").GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            Customize();
        }

        public void Customize()
        {
            crystal.text = $"{GameData.currencies.Crystals.tmpSprite}<size=44> {walletPlayerState.Crystal.amount}";
            wood.text = $"{GameData.currencies.Wood.tmpSprite}<size=44> {walletPlayerState.Wood.amount}";
            copper.text = $"{GameData.currencies.Copper.tmpSprite}<size=44> {walletPlayerState.Copper.amount}";
            gold.text = $"{GameData.currencies.Gold.tmpSprite}<size=44> {walletPlayerState.Gold.amount}";
            gems.text = $"{GameData.currencies.Gems.tmpSprite}<size=44> {walletPlayerState.Gems.amount}";
            stone.text = $"{GameData.currencies.Stone.tmpSprite}<size=44> {walletPlayerState.Stone.amount}";
        }

        public void ShowChangesAnim(AdminBRO.PlayerInfo from, AdminBRO.PlayerInfo to)
        {
            Customize();

            var seq = DOTween.Sequence();
            TryJoin(seq, crystal, from.Crystal.amount, to.Crystal.amount);
            TryJoin(seq, wood, from.Wood.amount, to.Wood.amount);
            TryJoin(seq, stone, from.Stone.amount, to.Stone.amount);
            TryJoin(seq, copper, from.Copper.amount, to.Copper.amount);
            TryJoin(seq, gold, from.Gold.amount, to.Gold.amount);
            TryJoin(seq, gems, from.Gems.amount, to.Gems.amount);
            seq.SetLink(gameObject);
            seq.Play();
        }

        private void TryJoin(Sequence seq, TextMeshProUGUI text, int prevAmount, int curAmount)
        {
            if (curAmount > prevAmount)
            {
                text.color = Color.green;
                seq.Join(text.DOColor(Color.white, 0.8f));
            }
            if (curAmount < prevAmount)
            {
                text.color = Color.red;
                seq.Join(text.DOColor(Color.white, 0.8f));
            }
        }

        public static WalletWidget GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateWidgetPrefab<WalletWidget>("Prefabs/UI/Widgets/WalletWidget/WalletWidget",
                parent);
        }
    }
}