using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class BattleGirlScreen : BaseFullScreen
    {
        private AdminBRO.Character character;
        
        private Button backButton;
        private Button forgeButton;
        private Button sexSceneButton;
        
        private Button levelUpButton;
        private GameObject levelUpButtonMaxLevel;

        private TextMeshProUGUI speed;
        private TextMeshProUGUI power;
        private TextMeshProUGUI constitution;
        private TextMeshProUGUI agility;

        private TextMeshProUGUI accuracy;
        private TextMeshProUGUI dodgeChance;
        private TextMeshProUGUI critChance;
        private TextMeshProUGUI health;
        private TextMeshProUGUI damageDealt;

        private Image rarityBack;
        private TextMeshProUGUI classIcon;
        private TextMeshProUGUI className;
        private TextMeshProUGUI characterName;
        private TextMeshProUGUI potency;

        private Image weapon;
        private Button weaponScreenButton;

        private void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab
                ("Prefabs/UI/Screens/BattleGirlScreen/BattleGirlScreen", transform);

            var canvas = screenInst.transform.Find("Canvas");
            var info = canvas.Find("Info");
            var mainStats = info.Find("StatsBack").Find("MainStats");
            var secondaryStats = info.Find("StatsBack").Find("SecondaryStats");

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);

            weapon = canvas.Find("Weapon").GetComponent<Image>();
            weaponScreenButton = weapon.transform.Find("WeaponScreenButton").GetComponent<Button>();
            weaponScreenButton.onClick.AddListener(WeaponScreenButtonClick);
            
            levelUpButton = canvas.Find("LevelUpButton").GetComponent<Button>();
            levelUpButtonMaxLevel = levelUpButton.transform.Find("MaxLevel").GetComponent<GameObject>();
            
            forgeButton = canvas.Find("ForgeButton").GetComponent<Button>();
            sexSceneButton = canvas.Find("SexSceneButton").GetComponent<Button>();

            speed = mainStats.Find("Speed").Find("Stat").GetComponent<TextMeshProUGUI>();
            power = mainStats.Find("Power").Find("Stat").GetComponent<TextMeshProUGUI>();
            constitution = mainStats.Find("Constitution").Find("Stat").GetComponent<TextMeshProUGUI>();
            agility = mainStats.Find("Agility").Find("Stat").GetComponent<TextMeshProUGUI>();

            accuracy = secondaryStats.Find("Accuracy").Find("Stat").GetComponent<TextMeshProUGUI>();
            dodgeChance = secondaryStats.Find("DodgeChance").Find("Stat").GetComponent<TextMeshProUGUI>();
            critChance = secondaryStats.Find("CriticalChance").Find("Stat").GetComponent<TextMeshProUGUI>();
            health = secondaryStats.Find("Health").Find("Stat").GetComponent<TextMeshProUGUI>();
            damageDealt = secondaryStats.Find("DamageDealt").Find("Stat").GetComponent<TextMeshProUGUI>();

            rarityBack = info.Find("ClassBack").GetComponent<Image>();
            classIcon = rarityBack.transform.Find("Icon").GetComponent<TextMeshProUGUI>();
            className = rarityBack.transform.Find("ClassName").GetComponent<TextMeshProUGUI>();
            characterName = info.Find("NameBack").Find("Name").GetComponent<TextMeshProUGUI>();
            potency = info.Find("PotencyBack").Find("Value").GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            Customize();
        }

        public BattleGirlScreen SetData(AdminBRO.Character character)
        {
            this.character = character;
            return this;
        }
        
        private void Customize()
        {
            
        }
        
        private void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<TeamEditScreen>();
        }

        private void WeaponScreenButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<WeaponScreen>();
        }

        private void LevelUpButtonClick()
        {
        }

        private void ForgeButtonClick()
        {
            
        }

        private void SexSceneButtonClick()
        {
            
        }
    }
}