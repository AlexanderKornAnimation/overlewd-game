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
    namespace NSWeaponScreen
    {
        public class Weapon : MonoBehaviour
        {
            private Button button;
            private GameObject forAnotherClass;
            private GameObject notificationEquipped;
            private GameObject notificationNew;
            private Image weaponIcon;
            private Image equippedCharacterIcon;

            public int weaponId;
            public AdminBRO.Equipment weaponData => GameData.equipment.GetById(weaponId);

            public int? characterId;
            public AdminBRO.Character characterData => GameData.characters.GetById(characterId);


            private void Awake()
            {
                var canvas = transform.Find("Canvas");

                button = canvas.Find("Button").GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);

                forAnotherClass = canvas.Find("ForAnotherClass").gameObject;
                notificationEquipped = canvas.Find("NotificationEquipped").gameObject;
                notificationNew = canvas.Find("NotificationNew").gameObject;
                weaponIcon = canvas.Find("WeaponIcon").GetComponent<Image>();
                equippedCharacterIcon = notificationEquipped.transform.Find("CharacterIcon").GetComponent<Image>();
            }

            private void Start()
            {
                Customize();
            }

            public void Customize()
            {
                forAnotherClass.SetActive(weaponData.characterClass != characterData.characterClass);
                button.gameObject.SetActive(weaponData.characterClass == characterData.characterClass);
                notificationEquipped.SetActive(weaponData.isEquipped);
                equippedCharacterIcon.gameObject.SetActive(!weaponData.IsMy(characterId));

                weaponIcon.color = weaponData.isEquipped ? Color.gray : Color.white;
                
                if (weaponData.IsMy(characterId))
                {
                    transform.SetAsFirstSibling();
                }
                else
                {
                    var equippedCharacter = GameData.characters.GetById(weaponData.characterId);
                    // equippedCharacterIcon.sprite = ResourceManager.LoadSprite(equippedCharacter.teamEditPersIcon);
                }
            }

            public async Task Equip(int chId, int eqId)
            {
                await GameData.equipment.Equip(chId, eqId);
            }

            public async Task Unequip(int chId, int eqId)
            {
                await GameData.equipment.Unequip(chId, eqId);
            }
            
            private async void ButtonClick()
            {
                if (characterId.HasValue)
                {
                    SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                    
                    if (weaponData.isEquipped)
                    {
                        if (weaponData.IsMy(characterId.Value))
                        {
                            await Unequip(characterId.Value, weaponId);
                        }
                        else
                        {
                            await Unequip(weaponData.characterId.Value, weaponId);

                            if (characterData.hasEquipment)
                            {
                                await Unequip(characterId.Value, characterData.equipmentData.id);
                            }
                            await Equip(characterId.Value, weaponId);
                        }
                    }
                    else
                    {
                        if (characterData.hasEquipment)
                        {
                           await Unequip(characterId.Value, characterData.equipmentData.id);
                        }
                        await Equip(characterId.Value, weaponId);
                    }
                }
            }

            public static Weapon GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<Weapon>("Prefabs/UI/Screens/WeaponScreen/Weapon",
                    parent);
            }
        }
    }
}