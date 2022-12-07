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
                GameData.buildings.magicGuild.GetSkillByType(skillType);

            
            private void Awake()
            {
                isLocked = transform.Find("IsLocked").gameObject;
                level = transform.Find("Level").GetComponent<TextMeshProUGUI>();
                title = transform.Find("Title").GetComponent<TextMeshProUGUI>();
                icon = transform.Find("SpellImage").GetComponent<Image>();
                
                isMax = transform.Find("IsMax").GetComponent<Button>();
                isMax.onClick.AddListener(ButtonClick);
                isOpen = transform.Find("IsOpen").GetComponent<Button>();
                isOpen.onClick.AddListener(ButtonClick);
            }

            private void Start()
            {
                Customize();
            }

            private void Customize()
            {
                isLocked.SetActive(skillData.locked);
                level.text = skillData.isLvlMax ? "MAX" : "Lvl " + skillData.currentSkillLevel;
                title.text = skillData.current.name;
                icon.sprite = ResourceManager.LoadSprite(skillData.current.icon);
                isMax.gameObject.SetActive(skillData.isLvlMax);
                UITools.DisableButton(isMax, !skillData.canUpgrade);
            }

            private void PlayAnimation()
            {
                UIfx.Inst(UIfx.UIFX_OVERLORD_SPELLS, transform);
            }

            public void OnLvlUp()
            {
                Customize();
                PlayAnimation();
            }

            private void ButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                UIManager.MakePopup<SpellPopup>().
                    SetData(new SpellPopupInData
                {
                    spellId = skillData.current.id
                }).DoShow();
            }
        }
    }
}