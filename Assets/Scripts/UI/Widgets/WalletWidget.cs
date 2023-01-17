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

        public void Customize(AdminBRO.PlayerInfo playerInfo = null)
        {
            var pInfo = playerInfo ?? GameData.player.info;
            crystal.text = $"{GameData.currencies.Crystals.tmpSprite}<size=44> {pInfo.Crystal.amount}";
            wood.text = $"{GameData.currencies.Wood.tmpSprite}<size=44> {pInfo.Wood.amount}";
            copper.text = $"{GameData.currencies.Copper.tmpSprite}<size=44> {pInfo.Copper.amount}";
            gold.text = $"{GameData.currencies.Gold.tmpSprite}<size=44> {pInfo.Gold.amount}";
            gems.text = $"{GameData.currencies.Gems.tmpSprite}<size=44> {pInfo.Gems.amount}";
            stone.text = $"{GameData.currencies.Stone.tmpSprite}<size=44> {pInfo.Stone.amount}";
        }

        public void ShowChangesAnim()
        {
            Customize();

            var curInfo = GameData.player.info;
            var prevInfo = GameData.player.prevInfo;

            var seq = DOTween.Sequence();
            TryJoin(seq, crystal, curInfo.Crystal.amount, prevInfo.Crystal.amount);
            TryJoin(seq, wood, curInfo.Wood.amount, prevInfo.Wood.amount);
            TryJoin(seq, stone, curInfo.Stone.amount, prevInfo.Stone.amount);
            TryJoin(seq, copper, curInfo.Copper.amount, prevInfo.Copper.amount);
            TryJoin(seq, gold, curInfo.Gold.amount, prevInfo.Gold.amount);
            TryJoin(seq, gems, curInfo.Gems.amount, prevInfo.Gems.amount);
            seq.SetLink(gameObject);
            seq.Play();
        }

        public async Task WaitChangesAnim()
        {
            await UniTask.Delay(2000);
        }

        private void TryJoin(Sequence seq, TextMeshProUGUI text, int curAmount, int prevAmount)
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

        public override void OnGameDataEvent(GameDataEvent eventData)
        {
            switch (eventData.id)
            {
                case GameDataEventId.WalletStateChange:
                    ShowChangesAnim();
                    break;
            }
        }

        public static WalletWidget GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateWidgetPrefab<WalletWidget>("Prefabs/UI/Widgets/WalletWidget/WalletWidget",
                parent);
        }
    }
}