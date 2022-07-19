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

            private Button levelUpButton;

            private Image[] priceIcons = new Image[2];
            private TextMeshProUGUI[] priceAmounts = new TextMeshProUGUI[2];

            private void Awake()
            {
                icon = transform.Find("Icon").GetComponent<Image>();
                title = transform.Find("Title").GetComponent<TextMeshProUGUI>();
                description = transform.Find("Description").GetComponent<TextMeshProUGUI>();
                levelUpButton = transform.Find("LevelUpButton").GetComponent<Button>();
                levelUpButton.onClick.AddListener(LevelUpButtonClick);
            }

            public void Customize()
            {
                icon.sprite = ResourceManager.LoadSprite(skillData?.icon);
                title.text = skillData?.name;
                description.text = skillData?.description;

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
                    if (characterData.CanSkillLvlUp(skillData))
                    {
                        await GameData.characters.SkillLvlUp(characterId.Value, skillData.id);
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