using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSBattleGirlScreen
    {
        public class Skill : MonoBehaviour
        {
            public int? characterId;
            public AdminBRO.Character characterData => GameData.characters.GetById(characterId);

            public string skillType { get; set; }
            public AdminBRO.CharacterSkill skillData => characterData?.skills.FirstOrDefault(s => s.type == skillType);

            private Image icon;
            private TextMeshProUGUI title;
            private TextMeshProUGUI description;
            private Transform levelBack;
            private TextMeshProUGUI level;

            private Button levelUpButton;
            private GameObject levelUpLocked;

            private Image[] priceIcons = new Image[2];
            private TextMeshProUGUI[] priceAmounts = new TextMeshProUGUI[2];

            private void Awake()
            {
                icon = transform.Find("Icon").GetComponent<Image>();
                title = transform.Find("Title").GetComponent<TextMeshProUGUI>();
                levelBack = transform.Find("LevelBack");
                level = levelBack.Find("Level").GetComponent<TextMeshProUGUI>();
                description = transform.Find("Description").GetComponent<TextMeshProUGUI>();
                levelUpButton = transform.Find("LevelUpButton").GetComponent<Button>();
                levelUpButton.onClick.AddListener(LevelUpButtonClick);
                levelUpLocked = transform.Find("LevelUpLocked").gameObject;
            }

            public void Customize()
            {
                title.text = skillData?.name;
                description.text = skillData?.description;
                level.text = skillData?.level.ToString();
                levelUpLocked.SetActive(!characterData.CanSkillLvlUpByLevel(skillData));
                
                for (int i = 0; i < skillData?.levelUpPrice?.Count; i++)
                {
                    priceIcons[i] = levelUpButton.transform.Find($"Resource{i + 1}").GetComponent<Image>();
                    priceAmounts[i] = priceIcons[i].transform.Find("Count").GetComponent<TextMeshProUGUI>();

                    var iconUrl = GameData.currencies.GetById(skillData?.levelUpPrice[i]?.currencyId).iconUrl;
                    priceIcons[i].sprite = ResourceManager.LoadSprite(iconUrl);
                    priceAmounts[i].text = skillData?.levelUpPrice[i]?.amount.ToString();
                }
            }

            private async void LevelUpButtonClick()
            {
                if (characterData != null && characterId.HasValue && skillData != null)
                {
                    if (characterData.CanSkillLvlUpByPrice(skillData))
                    {
                        await GameData.characters.SkillLvlUp(characterId.Value, skillData.id);
                        SpineWidget.GetInstanceDisposable(GameData.animations["uifx_lvlup01"], levelBack.transform);
                        Customize();
                    }
                    else
                    {
                        UIManager.ShowPopup<DeclinePopup>();
                    }
                }
            }
        }
    }
}