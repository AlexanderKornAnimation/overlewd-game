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

            public AdminBRO.MagicGuildSkill skillData;
            
            private void Awake()
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

            private void Start()
            {
                Customize();
            }

            public void Customize()
            {
                isLocked.SetActive(false);
                level.text = skillData.isLvlMax ? "MAX" : "Lvl " + skillData.currentSkillLevel;
                title.text = skillData.current.name;
                icon.sprite = ResourceManager.LoadSprite(skillData.current.icon);
                isMax.gameObject.SetActive(skillData.isLvlMax);
                Debug.Log(skillData.current.name);
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
                    spellId = skillData.current.id,
                }).RunShowPopupProcess();
            }
        }
    }
}