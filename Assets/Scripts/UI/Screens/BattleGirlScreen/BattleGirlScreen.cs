using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    public class BattleGirlScreen : BaseFullScreenParent<BattleGirlScreenInData>
    {
        private Button backButton;
        private Button laboratoryButton;
        private Button sexSceneButton;
        private Image sexSceneOpen;
        private Image sexSceneClose;
        private TextMeshProUGUI sexSceneClosedTitle;

        private Transform levelUpButton;
        private Button levelUpButtonCanLvlUp;
        private Image[] levelPriceIcons = new Image[2];
        private TextMeshProUGUI[] levelPriceAmounts = new TextMeshProUGUI[2];

        private TextMeshProUGUI speed;
        private TextMeshProUGUI power;
        private TextMeshProUGUI constitution;
        private TextMeshProUGUI agility;

        private TextMeshProUGUI accuracy;
        private TextMeshProUGUI dodgeChance;
        private TextMeshProUGUI critChance;
        private TextMeshProUGUI health;
        private TextMeshProUGUI damageDealt;

        private TextMeshProUGUI classIcon;
        private TextMeshProUGUI className;
        private TextMeshProUGUI characterName;
        private TextMeshProUGUI potency;
        private TextMeshProUGUI level;

        private GameObject rarityBasic;
        private GameObject rarityAdvanced;
        private GameObject rarityEpic;
        private GameObject rarityHeroic;

        private Image weapon;
        private Button weaponScreenButton;
        private Transform currencyBack;
        private Image girl;

        private GameObject passiveSkillGO;
        private GameObject basicSkillGO;
        private GameObject ultimateSkillGO;
        private NSBattleGirlScreen.Skill basicSkill;
        private NSBattleGirlScreen.Skill passiveSkill;
        private NSBattleGirlScreen.Skill ultimateSkill;

        private void Awake()
        {
            var screenInst = ResourceManager.InstantiateScreenPrefab
                ("Prefabs/UI/Screens/BattleGirlScreen/BattleGirlScreen", transform);

            var canvas = screenInst.transform.Find("Canvas");
            var info = canvas.Find("Info");
            var mainStats = info.Find("StatsBack").Find("MainStats");
            var secondaryStats = info.Find("StatsBack").Find("SecondaryStats");
            var levelBack = canvas.Find("LevelBack");
            var skills = canvas.Find("Skills");

            backButton = canvas.Find("BackButton").GetComponent<Button>();
            backButton.onClick.AddListener(BackButtonClick);

            weapon = canvas.Find("Weapon").GetComponent<Image>();
            weaponScreenButton = canvas.Find("WeaponScreenButton").GetComponent<Button>();
            weaponScreenButton.onClick.AddListener(WeaponScreenButtonClick);

            levelUpButton = canvas.Find("LevelUpButton");
            levelUpButtonCanLvlUp = levelUpButton.Find("CanLvlUp").GetComponent<Button>();
            levelUpButtonCanLvlUp.onClick.AddListener(LevelUpButtonClick);

            laboratoryButton = canvas.Find("ForgeButton").GetComponent<Button>();
            laboratoryButton.onClick.AddListener(LaboratoryButtonClick);

            var sexSceneButtonTr = canvas.Find("SexSceneButton");
            sexSceneButton = sexSceneButtonTr.Find("IsOpen").GetComponent<Button>();
            sexSceneButton.onClick.AddListener(SexSceneButtonClick);
            sexSceneOpen = sexSceneButtonTr.transform.Find("IsOpen").GetComponent<Image>();
            sexSceneClose = sexSceneButtonTr.GetComponent<Image>();
            sexSceneClosedTitle = sexSceneButtonTr.transform.Find("Title").GetComponent<TextMeshProUGUI>();

            speed = mainStats.Find("Speed").Find("Stat").GetComponent<TextMeshProUGUI>();
            power = mainStats.Find("Power").Find("Stat").GetComponent<TextMeshProUGUI>();
            constitution = mainStats.Find("Constitution").Find("Stat").GetComponent<TextMeshProUGUI>();
            agility = mainStats.Find("Agility").Find("Stat").GetComponent<TextMeshProUGUI>();

            accuracy = secondaryStats.Find("Accuracy").Find("Stat").GetComponent<TextMeshProUGUI>();
            dodgeChance = secondaryStats.Find("DodgeChance").Find("Stat").GetComponent<TextMeshProUGUI>();
            critChance = secondaryStats.Find("CriticalChance").Find("Stat").GetComponent<TextMeshProUGUI>();
            health = secondaryStats.Find("Health").Find("Stat").GetComponent<TextMeshProUGUI>();
            damageDealt = secondaryStats.Find("DamageDealt").Find("Stat").GetComponent<TextMeshProUGUI>();

            var classInfo = info.Find("ClassInfo");
            classIcon = classInfo.transform.Find("Icon").GetComponent<TextMeshProUGUI>();
            className = classInfo.transform.Find("ClassName").GetComponent<TextMeshProUGUI>();
            characterName = info.Find("NameBack").Find("Name").GetComponent<TextMeshProUGUI>();
            potency = info.Find("PotencyBack").Find("Value").GetComponent<TextMeshProUGUI>();
            level = levelBack.Find("Level").GetComponent<TextMeshProUGUI>();

            rarityBasic = classInfo.Find("RarityBasic").gameObject;
            rarityAdvanced = classInfo.Find("RarityAdvanced").gameObject;
            rarityEpic = classInfo.Find("RarityEpic").gameObject;
            rarityHeroic = classInfo.Find("RarityHeroic").gameObject;

            currencyBack = canvas.Find("CurrencyBack");
            girl = canvas.Find("Girl").GetComponent<Image>();

            passiveSkillGO = skills.Find("PassiveSkill").gameObject;
            passiveSkill = passiveSkillGO.AddComponent<NSBattleGirlScreen.Skill>();

            basicSkillGO = skills.Find("BasicSkill").gameObject;
            basicSkill = basicSkillGO.AddComponent<NSBattleGirlScreen.Skill>();

            ultimateSkillGO = skills.Find("UltimateSkill").gameObject;
            ultimateSkill = ultimateSkillGO.AddComponent<NSBattleGirlScreen.Skill>();

        }

        public override async Task BeforeShowMakeAsync()
        {
            Customize();

            await Task.CompletedTask;
        }

        private void Customize()
        {
            var characterData = inputData?.characterData;
            if (characterData != null)
            {
                
                speed.text = characterData.speed.ToString();
                power.text = characterData.power.ToString();
                constitution.text = characterData.constitution.ToString();
                agility.text = characterData.agility.ToString();

                accuracy.text = characterData.accuracy * 100 + "%";
                dodgeChance.text = characterData.dodge * 100 + "%";
                critChance.text = characterData.critrate * 100 + "%";
                health.text = characterData.health.ToString();
                damageDealt.text = characterData.damage.ToString();

                potency.text = characterData.potency.ToString();

                classIcon.text = characterData.classMarker;
                className.text = characterData.characterClass;
                characterName.text = characterData.name;
                level.text = characterData.level.ToString();

                girl.sprite = ResourceManager.LoadSprite(characterData.fullScreenPersIcon);

                passiveSkill.skillType =  AdminBRO.CharacterSkill.Type_Passive;
                passiveSkill.characterId = inputData?.characterId;
                passiveSkill.Customize();

                basicSkill.skillType = AdminBRO.CharacterSkill.Type_Attack;
                basicSkill.characterId = inputData?.characterId;
                basicSkill.Customize();

                ultimateSkill.skillType = AdminBRO.CharacterSkill.Type_Enhanced;
                ultimateSkill.characterId = inputData?.characterId;
                ultimateSkill.Customize();

                levelUpButtonCanLvlUp.gameObject.SetActive(!characterData.isLvlMax);
                sexSceneOpen.sprite = ResourceManager.LoadSprite(characterData.sexSceneOpenedBanner);
                sexSceneClose.sprite = ResourceManager.LoadSprite(characterData.sexSceneClosedBanner);
                sexSceneOpen.gameObject.SetActive(characterData.sexSceneId.HasValue);

                var sexSceneRarity = characterData.sexSceneVisibleByRarity ?? "_";
                sexSceneClosedTitle.text = $"Upgrade {characterData.name} to {sexSceneRarity}\nto unlock sex scene!";
                
                rarityBasic.SetActive(characterData.isBasic);
                rarityAdvanced.SetActive(characterData.isAdvanced);
                rarityEpic.SetActive(characterData.isEpic);
                rarityHeroic.SetActive(characterData.isHeroic);
                
                weapon.gameObject.SetActive(characterData.hasEquipment);
                
                if (weapon.gameObject.activeSelf)
                {
                    weapon.sprite = ResourceManager.LoadSprite(characterData.equipmentData?.icon);
                }

                for (int i = 0; i < characterData?.levelUpPrice?.Count; i++)
                {
                    var price = characterData.levelUpPrice[i];
                    var currency = GameData.currencies.GetById(price.currencyId);

                    levelPriceIcons[i] = levelUpButtonCanLvlUp.transform.Find($"Resource{i + 1}").GetComponent<Image>();
                    levelPriceAmounts[i] = levelPriceIcons[i]?.transform.Find("Count").GetComponent<TextMeshProUGUI>();

                    levelPriceIcons[i].sprite = ResourceManager.LoadSprite(currency.iconUrl);
                    levelPriceAmounts[i].text = price.amount.ToString();
                }
            }

            UITools.FillWallet(currencyBack);
        }

        private void BackButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            if (inputData == null)
            {
                UIManager.ShowScreen<TeamEditScreen>();
            }
            else
            {
                UIManager.MakeScreen<TeamEditScreen>().
                    SetData(inputData.prevScreenInData.As<TeamEditScreenInData>())
                    .RunShowScreenProcess();
            }
        }

        private void WeaponScreenButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            if (inputData == null)
            {
                UIManager.ShowScreen<WeaponScreen>();
            }
            else
            {
                UIManager.MakeScreen<WeaponScreen>().
                    SetData(new WeaponScreenInData
                {
                    prevScreenInData = inputData,
                    ftueStageId = inputData.ftueStageId,
                    eventStageId = inputData.eventStageId,
                    characterId = inputData.characterId
                }).RunShowScreenProcess();
            }
        }

        private void LaboratoryButtonClick()
        {
            SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            UIManager.ShowScreen<LaboratoryScreen>();
        }

        public override void OnGameDataEvent(GameDataEvent eventData)
        {
            switch (eventData?.type)
            {
                case GameDataEvent.Type.CharacterLvlUp:
                    Customize();
                    break;
                case GameDataEvent.Type.CharacterSkillLvlUp:
                    basicSkill.Customize();
                    ultimateSkill.Customize();
                    passiveSkill.Customize();
                    break;
            }
        }

        private async void LevelUpButtonClick()
        {
            if (inputData != null && inputData.characterId.HasValue)
            {
                var characterData = inputData?.characterData;
                if (characterData.canLvlUp)
                {
                    await GameData.characters.LvlUp(inputData.characterId.Value);
                }
                else
                {
                    UIManager.ShowPopup<DeclinePopup>();
                }
            }
        }

        private void SexSceneButtonClick()
        {
            var sexSceneId = inputData?.characterData?.sexSceneId;
            if (sexSceneId.HasValue)
            {
                UIManager.MakeScreen<SexScreen>().
                    SetData(new SexScreenInData
                {
                    dialogId = sexSceneId,
                    prevScreenInData = inputData
                }).RunShowScreenProcess();
            }
        }
    }

    public class BattleGirlScreenInData : BaseFullScreenInData
    {
        public int? characterId;
        public AdminBRO.Character characterData => GameData.characters.GetById(characterId);
    }
}