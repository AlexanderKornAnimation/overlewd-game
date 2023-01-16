using Cysharp.Threading.Tasks;
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

        public void Customize()
        {
            crystal.text = $"{GameData.currencies.Crystals.tmpSprite}<size=44> {GameData.player.Crystal.amount}";
            wood.text = $"{GameData.currencies.Wood.tmpSprite}<size=44> {GameData.player.Wood.amount}";
            copper.text = $"{GameData.currencies.Copper.tmpSprite}<size=44> {GameData.player.Copper.amount}";
            gold.text = $"{GameData.currencies.Gold.tmpSprite}<size=44> {GameData.player.Gold.amount}";
            gems.text = $"{GameData.currencies.Gems.tmpSprite}<size=44> {GameData.player.Gems.amount}";
            stone.text = $"{GameData.currencies.Stone.tmpSprite}<size=44> {GameData.player.Stone.amount}";
        }

        public void ShowChangesAnim()
        {
            Customize();
        }

        public async Task WaitChangesAnim()
        {
            await UniTask.Delay(2000);
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