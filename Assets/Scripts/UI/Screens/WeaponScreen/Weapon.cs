using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

            public event Action<Weapon> onEquip;
            public event Action<Weapon> onUnequip;

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
                bool isEquipped = characterId == weaponData.characterId || weaponData.isEquipped;
                notificationEquipped.SetActive(isEquipped);
                
                if (weaponData.isEquipped && weaponData.characterId != characterId)
                {
                    var equippedCharacter = GameData.characters.GetById(weaponData.characterId);
                    equippedCharacterIcon.sprite = ResourceManager.LoadSprite(equippedCharacter.teamEditPersIcon);
                }
            }

            private async void ButtonClick()
            {
                if (characterId.HasValue)
                {
                    SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                    
                    if (weaponData.isEquipped)
                    {
                        await GameData.equipment.Unequip(characterId.Value, weaponId);
                        onUnequip?.Invoke(this);
                    }
                    else
                    {
                        await GameData.equipment.Equip(characterId.Value, weaponId);
                        onEquip?.Invoke(this);
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