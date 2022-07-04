using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSMagicGuildScreen
    {
        public class Spell : MonoBehaviour
        {
            private GameObject isLocked;
            private TextMeshProUGUI level;
            private TextMeshProUGUI title;
            private Image icon;
            private Button isMax;
            private Button isOpen;

            public string skillType { get; set; }
            public AdminBRO.MagicGuildSkill skillData =>
                GameData.buildings.GetMagicGuildSkillByType(skillType);
            
            void Awake()
            {
                isLocked = transform.Find("IsLocked").gameObject;
                level = transform.Find("Level").GetComponent<TextMeshProUGUI>();
                title = transform.Find("Title").GetComponent<TextMeshProUGUI>();
                icon = transform.Find("SpellImage").GetComponent<Image>();
                
                isMax = transform.Find("IsMax").GetComponent<Button>();
                isMax.onClick.AddListener(IsMaxButtonClick);
                isOpen = transform.Find("IsOpen").GetComponent<Button>();
                isOpen.onClick.AddListener(IsOpenButtonClick);
            }

            void Start()
            {
                Customize();
            }

            public void Customize()
            {
                var _skillData = skillData;

                isLocked.SetActive(false);
                level.text = _skillData.isLvlMax ? "MAX" : "Lvl " + _skillData.currentSkillLevel;
                title.text = _skillData.current.name;
                icon.sprite = ResourceManager.LoadSprite(_skillData.current.icon);
                isMax.gameObject.SetActive(_skillData.isLvlMax);
            }

            private void IsMaxButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                UIManager.MakePopup<SpellPopup>().
                    SetData(new SpellPopupInData
                {
                    spellId = skillData.current.id
                }).RunShowPopupProcess();
            }

            private void IsOpenButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                UIManager.MakePopup<SpellPopup>().
                    SetData(new SpellPopupInData
                {
                    spellId = skillData.current.id
                }).RunShowPopupProcess();
            }
        }
    }
}