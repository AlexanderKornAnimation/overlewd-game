using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSTeamEditScreen
    {
         public class Slot : MonoBehaviour
        {
            private Transform slotEmptyHint;
            private Transform slotFull;
            private Image characterIcon;
            private Image background;
            private TextMeshProUGUI characterClass;
            private Button button;
            private TextMeshProUGUI level;
            private TextMeshProUGUI potencyValue;
            private Image weaponIcon;
            private Button weaponButton;
            private Button weaponScreenButton;
            private TextMeshProUGUI weaponPotency;

            private TextMeshProUGUI basicSkillEffect;
            private TextMeshProUGUI basicSkillLevel;
            private Button basicSkillButton;
            private TextMeshProUGUI ultimateSkillEffect;
            private TextMeshProUGUI ultimateSkillLevel;
            private Button ultimateSkillButton;
            private TextMeshProUGUI passiveSkillEffect;
            private TextMeshProUGUI passiveSkillLevel;
            private Button passiveSkillButton;
            
            public string chTeamPos { get; set; }

            private AdminBRO.Character characterData => chTeamPos == AdminBRO.Character.TeamPosition_Slot1 ?
                GameData.characters.slot1Ch : chTeamPos == AdminBRO.Character.TeamPosition_Slot2 ? 
                    GameData.characters.slot2Ch : null;

            public event Action<int?> OnSlotClick;

            private void Awake()
            {
                slotEmptyHint = transform.Find("SlotEmptyHint");
                slotFull = transform.Find("SlotFull");
                
                var headline = slotFull.Find("Headline");
                characterIcon = slotFull.Find("Character").GetComponent<Image>();
                background = slotFull.Find("Background").GetComponent<Image>();
                characterClass = headline.Find("CharacterClass").GetComponent<TextMeshProUGUI>();
                button = slotFull.Find("SlotButton").GetComponent<Button>();
                button.onClick.AddListener(SlotClick);
                level = slotFull.Find("Headline").Find("LevelBack").Find("Level").GetComponent<TextMeshProUGUI>();
                potencyValue = headline.Find("Potency").GetComponent<TextMeshProUGUI>();
                
                var weaponInfo = slotFull.Find("WeaponInfo");
                weaponIcon = weaponInfo.Find("WeaponIcon").GetComponent<Image>();
                weaponButton = weaponIcon.transform.Find("Button").GetComponent<Button>();
                weaponButton.onClick.AddListener(WeaponButtonClick);
                weaponPotency = weaponInfo.Find("PotencyBack/Potency").GetComponent<TextMeshProUGUI>();
                weaponScreenButton = weaponInfo.Find("SwapWeaponButton").GetComponent<Button>();
                weaponScreenButton.onClick.AddListener(SwapWeaponButtonClick);
                
                var skills = slotFull.Find("Skills");
                var basicSkill = skills.Find("BasicSkill");
                basicSkillLevel = basicSkill.Find("LevelBack/Level").GetComponent<TextMeshProUGUI>();
                basicSkillEffect = basicSkill.Find("Effect").GetComponent<TextMeshProUGUI>();
                basicSkillButton = basicSkill.GetComponent<Button>();
                basicSkillButton.onClick.AddListener(() => SkillButtonClick(AdminBRO.CharacterSkill.Type_Attack));

                var ultimateSkill = skills.Find("UltimateSkill");
                ultimateSkillLevel = skills.Find("UltimateSkill/LevelBack/Level").GetComponent<TextMeshProUGUI>();
                ultimateSkillEffect = ultimateSkill.Find("Effect").GetComponent<TextMeshProUGUI>();
                ultimateSkillButton = ultimateSkill.GetComponent<Button>();
                ultimateSkillButton.onClick.AddListener(() => SkillButtonClick(AdminBRO.CharacterSkill.Type_Enhanced));
                
                var passiveSkill = skills.Find("PassiveSkill");
                passiveSkillLevel = skills.Find("PassiveSkill/LevelBack/Level").GetComponent<TextMeshProUGUI>();
                passiveSkillEffect = passiveSkill.Find("Effect").GetComponent<TextMeshProUGUI>();
                passiveSkillButton = passiveSkill.GetComponent<Button>();
                passiveSkillButton.onClick.AddListener(() => SkillButtonClick(AdminBRO.CharacterSkill.Type_Passive));
            }

            private void Start()
            {
                Customize();
            }

            public void Customize()
            {
                if (characterData != null)
                {
                    slotEmptyHint.gameObject.SetActive(false);
                    slotFull.gameObject.SetActive(true);
                    characterIcon.sprite = ResourceManager.LoadSprite(characterData.teamEditSlotPersIcon);
                    background.sprite = SlotBackgroundByRarity(characterData.rarity);
                    characterClass.text = characterData.classMarker;
                    level.text = characterData.level.ToString();
                    potencyValue.text = characterData.potency.ToString();
                
                    weaponIcon.sprite = ResourceManager.LoadSprite(characterData.characterEquipmentData?.icon);
                    weaponPotency.text = characterData.potency.ToString();
                    weaponIcon.gameObject.SetActive(characterData.hasEquipment);

                    basicSkillLevel.text = characterData.basicSkill.level.ToString();
                    ultimateSkillLevel.text = characterData.ultimateSkill.level.ToString();
                    passiveSkillLevel.text = characterData.passiveSkill.level.ToString();
                }
                else
                {
                    slotEmptyHint.gameObject.SetActive(true);
                    slotFull.gameObject.SetActive(false);
                }
                
                basicSkillLevel.text = characterData?.basicSkill?.level.ToString();
                basicSkillEffect.text = characterData?.basicSkill?.effectSprite;

                ultimateSkillLevel.text = characterData?.ultimateSkill?.level.ToString();
                ultimateSkillEffect.text = characterData?.ultimateSkill?.effectSprite;

                passiveSkillLevel.text = characterData?.passiveSkill?.level.ToString();
                passiveSkillEffect.text = characterData?.passiveSkill?.effectSprite;
            }

            private void WeaponButtonClick()
            {
                var weaponInfo = CharacterEquipInfo.GetInstance(transform.parent);
                weaponInfo.eqId = characterData?.characterEquipmentData?.id;
            }
            
            private void SkillButtonClick(string type)
            {
                var skillInfo = CharacterSkillInfo.GetInstance(transform.parent);
                skillInfo.chId = characterData?.id;
                skillInfo.skilltype = type;
            }
            
            private void SlotClick()
            {
                OnSlotClick?.Invoke(characterData?.id);
            }
            
            private void SwapWeaponButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                UIManager.MakeScreen<WeaponScreen>().
                    SetData(new WeaponScreenInData
                    {
                        characterId = characterData.id,
                        prevScreenInData = UIManager.prevScreenInData.prevScreenInData
                    }).RunShowScreenProcess();
            }
            
            private Sprite SlotBackgroundByRarity(string rarity)
            {
                return rarity switch
                {
                    AdminBRO.Rarity.Basic => ResourceManager.InstantiateAsset<Sprite>("Prefabs/UI/Screens/TeamEditScreen/Images/GirlCellBasic"),
                    AdminBRO.Rarity.Advanced => ResourceManager.InstantiateAsset<Sprite>("Prefabs/UI/Screens/TeamEditScreen/Images/GirlCellAdvanced"),
                    AdminBRO.Rarity.Epic => ResourceManager.InstantiateAsset<Sprite>("Prefabs/UI/Screens/TeamEditScreen/Images/GirlCellEpic"),
                    AdminBRO.Rarity.Heroic => ResourceManager.InstantiateAsset<Sprite>("Prefabs/UI/Screens/TeamEditScreen/Images/GirlCellHeroic"),
                    _ => null
                };
            }
        }
    }
}