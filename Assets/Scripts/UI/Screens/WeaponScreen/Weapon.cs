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
            private GameObject frame;
            private bool isInitialized = false;

            public int weaponId;
            public AdminBRO.Equipment weaponData => GameData.equipment.GetById(weaponId);

            public int? characterId;
            public AdminBRO.Character characterData => GameData.characters.GetById(characterId);

            public Action<Weapon> OnSelect;

            private void Awake()
            {
                if (!isInitialized)
                    Initialize();
            }

            public void Initialize()
            {
                var canvas = transform.Find("Canvas");

                button = canvas.Find("Button").GetComponent<Button>();
                button.onClick.AddListener(ButtonClick);

                forAnotherClass = canvas.Find("ForAnotherClass").gameObject;
                notificationEquipped = canvas.Find("NotificationEquipped").gameObject;
                notificationNew = canvas.Find("NotificationNew").gameObject;
                weaponIcon = canvas.Find("WeaponIcon").GetComponent<Image>();
                equippedCharacterIcon = notificationEquipped.transform.Find("CharacterIcon").GetComponent<Image>();
                frame = canvas.Find("Frame").gameObject;
                isInitialized = true;
            }
            
            private void Start()
            {
                Customize();
            }

            public void Customize()
            {
                forAnotherClass.SetActive(!weaponData.IsMyClass(characterData.characterClass));
                button.gameObject.SetActive(weaponData.IsMyClass(characterData.characterClass) && !weaponData.IsMy(characterId));
                notificationEquipped.SetActive(weaponData.isEquipped && weaponData.IsMyClass(characterData.characterClass));
                equippedCharacterIcon.gameObject.SetActive(!weaponData.IsMy(characterId));
                weaponIcon.sprite = ResourceManager.LoadSprite(weaponData.icon);
                frame.SetActive(false);

                if (weaponData.IsMy(characterId))
                {
                    transform.SetAsFirstSibling();
                }
                else
                {
                    var equippedCharacter = GameData.characters.GetById(weaponData.characterId);
                    equippedCharacterIcon.sprite = ResourceManager.LoadSprite(equippedCharacter?.iconUrl);
                }
            }

            public void Select()
            {
                var siblingIndex = characterData.hasEquipment ? 1 : 0;
                transform.SetSiblingIndex(siblingIndex);
                frame.SetActive(true);
                button.gameObject.SetActive(false);
            }

            public void Deselect()
            {
                frame.SetActive(false);
                button.gameObject.SetActive(true);
            }
            
            private void ButtonClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                OnSelect?.Invoke(this);
            }

            public static Weapon GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<Weapon>("Prefabs/UI/Screens/WeaponScreen/Weapon",
                    parent);
            }
        }
    }
}