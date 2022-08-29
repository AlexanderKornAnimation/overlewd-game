using System;
using System.Collections;
using System.Collections.Generic;
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

        private void Awake()
        {
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
            crystal.text = $"{GameData.currencies.Crystals.sprite}<size=44> {GameData.player.Crystal.amount}";
            wood.text = $"{GameData.currencies.Wood.sprite}<size=44> {GameData.player.Wood.amount}";
            copper.text = $"{GameData.currencies.Copper.sprite}<size=44> {GameData.player.Copper.amount}";
            gold.text = $"{GameData.currencies.Gold.sprite}<size=44> {GameData.player.Gold.amount}";
            gems.text = $"{GameData.currencies.Gems.sprite}<size=44> {GameData.player.Gems.amount}";
            stone.text = $"{GameData.currencies.Stone.sprite}<size=44> {GameData.player.Stone.amount}";
        }

        public static WalletWidget GetInstance(Transform parent)
        {
            return ResourceManager.InstantiateWidgetPrefab<WalletWidget>("Prefabs/UI/Widgets/WalletWidget/WalletWidget",
                parent);
        }
    }
}