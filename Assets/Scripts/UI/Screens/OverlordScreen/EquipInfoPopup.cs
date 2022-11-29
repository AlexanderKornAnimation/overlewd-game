using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSOverlordScreen
    {
        public class EquipInfoPopup : MonoBehaviour
        {
            private Image equippedItemIcon;
            private TextMeshProUGUI equippedItemName;

            private GameObject equippedSpeedBack;
            private TextMeshProUGUI equippedSpeed;

            private GameObject equippedPowerBack;
            private TextMeshProUGUI equippedPower;

            private GameObject equippedConstitutionBack;
            private TextMeshProUGUI equippedConstitution;

            private GameObject equippedAgilityBack;
            private TextMeshProUGUI equippedAgility;

            private GameObject equippedAccuracyBack;
            private TextMeshProUGUI equippedAccuracy;

            private GameObject equippedDodgeBack;
            private TextMeshProUGUI equippedDodge;

            private GameObject equippedCritrateBack;
            private TextMeshProUGUI equippedCritrate;

            private GameObject equippedDamageBack;
            private TextMeshProUGUI equippedDamage;

            private GameObject selectedItem;
            private Image selectedItemIcon;
            private TextMeshProUGUI selectedItemName;

            private GameObject selectedSpeedBack;
            private TextMeshProUGUI selectedSpeed;
            private TextMeshProUGUI selectedSpeedArrow;

            private GameObject selectedPowerBack;
            private TextMeshProUGUI selectedPower;
            private TextMeshProUGUI selectedPowerArrow;

            private GameObject selectedConstitutionBack;
            private TextMeshProUGUI selectedConstitution;
            private TextMeshProUGUI selectedConstitutionArrow;

            private GameObject selectedAgilityBack;
            private TextMeshProUGUI selectedAgility;
            private TextMeshProUGUI selectedAgilityArrow;

            private GameObject selectedAccuracyBack;
            private TextMeshProUGUI selectedAccuracy;
            private TextMeshProUGUI selectedAccuracyArrow;

            private GameObject selectedDodgeBack;
            private TextMeshProUGUI selectedDodge;
            private TextMeshProUGUI selectedDodgeArrow;

            private GameObject selectedCritRateBack;
            private TextMeshProUGUI selectedCritRate;
            private TextMeshProUGUI selectedCritRateArrow;

            private GameObject selectedDamageBack;
            private TextMeshProUGUI selectedDamage;
            private TextMeshProUGUI selectedDamageArrow;
            private Button equipButton;

            public int equipId;
            public AdminBRO.Equipment equipData => GameData.equipment.GetById(equipId);

            public int? selectedEquipId;
            private AdminBRO.Equipment selectedEquipData => GameData.equipment.GetById(selectedEquipId);

            public event Action<int, int> OnEquip;

            private Button missClickButton;
 
            
            private void Awake()
            {
                UITools.SetStretch(gameObject.GetComponent<RectTransform>());
                missClickButton = transform.Find("MissClickButton").GetComponent<Button>();
                missClickButton.onClick.AddListener(MissClickButtonClick);
                var equippedItem = transform.Find("Equips/EquippedItem");
                equippedItemIcon = equippedItem.Find("EquipIcon").GetComponent<Image>();
                equippedItemName = equippedItem.Find("EquipName").GetComponent<TextMeshProUGUI>();

                equippedSpeedBack = equippedItem.Find("Speed").gameObject;
                equippedSpeed = equippedSpeedBack.transform.Find("Stat").GetComponent<TextMeshProUGUI>();

                equippedPowerBack = equippedItem.Find("Power").gameObject;
                equippedPower = equippedPowerBack.transform.Find("Stat").GetComponent<TextMeshProUGUI>();

                equippedConstitutionBack = equippedItem.Find("Constitution").gameObject;
                equippedConstitution = equippedConstitutionBack.transform.Find("Stat").GetComponent<TextMeshProUGUI>();

                equippedAgilityBack = equippedItem.Find("Agility").gameObject;
                equippedAgility = equippedAgilityBack.transform.Find("Stat").GetComponent<TextMeshProUGUI>();

                equippedAccuracyBack = equippedItem.Find("Accuracy").gameObject;
                equippedAccuracy = equippedAccuracyBack.transform.Find("Stat").GetComponent<TextMeshProUGUI>();

                equippedDodgeBack = equippedItem.Find("Dodge").gameObject;
                equippedDodge = equippedDodgeBack.transform.Find("Stat").GetComponent<TextMeshProUGUI>();

                equippedCritrateBack = equippedItem.Find("Critrate").gameObject;
                equippedCritrate = equippedCritrateBack.transform.Find("Stat").GetComponent<TextMeshProUGUI>();

                equippedDamageBack = equippedItem.Find("Damage").gameObject;
                equippedDamage = equippedDamageBack.transform.Find("Stat").GetComponent<TextMeshProUGUI>();

                selectedItem = transform.Find("Equips/NewItem").gameObject;
                selectedItemIcon = selectedItem.transform.Find("EquipIcon").GetComponent<Image>();
                selectedItemName = selectedItem.transform.Find("EquipName").GetComponent<TextMeshProUGUI>();

                selectedSpeedBack = selectedItem.transform.Find("Speed").gameObject;
                selectedSpeed = selectedSpeedBack.transform.Find("Stat").GetComponent<TextMeshProUGUI>();
                selectedSpeedArrow = selectedSpeed.transform.Find("Arrow").GetComponent<TextMeshProUGUI>();

                selectedPowerBack = selectedItem.transform.Find("Power").gameObject;
                selectedPower = selectedPowerBack.transform.Find("Stat").GetComponent<TextMeshProUGUI>();
                selectedPowerArrow = selectedPower.transform.Find("Arrow").GetComponent<TextMeshProUGUI>();

                selectedConstitutionBack = selectedItem.transform.Find("Constitution").gameObject;
                selectedConstitution = selectedConstitutionBack.transform.Find("Stat").GetComponent<TextMeshProUGUI>();
                selectedConstitutionArrow = selectedConstitution.transform.Find("Arrow").GetComponent<TextMeshProUGUI>();

                selectedAgilityBack = selectedItem.transform.Find("Agility").gameObject;
                selectedAgility = selectedAgilityBack.transform.Find("Stat").GetComponent<TextMeshProUGUI>();
                selectedAgilityArrow = selectedAgility.transform.Find("Arrow").GetComponent<TextMeshProUGUI>();

                selectedAccuracyBack = selectedItem.transform.Find("Accuracy").gameObject;
                selectedAccuracy = selectedAccuracyBack.transform.Find("Stat").GetComponent<TextMeshProUGUI>();
                selectedAccuracyArrow = selectedAccuracy.transform.Find("Arrow").GetComponent<TextMeshProUGUI>();

                selectedDodgeBack = selectedItem.transform.Find("Dodge").gameObject;
                selectedDodge = selectedDodgeBack.transform.Find("Stat").GetComponent<TextMeshProUGUI>();
                selectedDodgeArrow = selectedDodge.transform.Find("Arrow").GetComponent<TextMeshProUGUI>();

                selectedCritRateBack = selectedItem.transform.Find("Critrate").gameObject;
                selectedCritRate = selectedCritRateBack.transform.Find("Stat").GetComponent<TextMeshProUGUI>();
                selectedCritRateArrow = selectedCritRate.transform.Find("Arrow").GetComponent<TextMeshProUGUI>();

                selectedDamageBack = selectedItem.transform.Find("Damage").gameObject;
                selectedDamage = selectedDamageBack.transform.Find("Stat").GetComponent<TextMeshProUGUI>();
                selectedDamageArrow = selectedDamage.transform.Find("Arrow").GetComponent<TextMeshProUGUI>();

                equipButton = selectedItem.transform.Find("EquipButton").GetComponent<Button>();
                equipButton.onClick.AddListener(EquipButtonClick);
            }

            private void Start()
            {
                Customize();
            }

            private void Customize()
            {
                CustomizeEquippedItem();

                selectedItem.SetActive(selectedEquipId.HasValue);

                if (selectedItem.activeSelf)
                {
                    CustomizeSelectedItem();
                }
            }

            private void CustomizeSelectedItem()
            {
                selectedItemIcon.sprite = ResourceManager.LoadSprite(selectedEquipData.icon);
                selectedItemName.text = selectedEquipData.name;

                selectedSpeed.text = "+" + selectedEquipData.speed;
                selectedSpeedArrow.text = GetArrowByStats(equipData.speed, selectedEquipData.speed);
                selectedSpeedBack.SetActive(selectedEquipData.speed != 0);

                selectedPower.text = "+" + selectedEquipData.power;
                selectedPowerArrow.text = GetArrowByStats(equipData.power, selectedEquipData.power);
                selectedPowerBack.SetActive(selectedEquipData.power != 0);

                selectedConstitution.text = "+" + selectedEquipData.constitution;
                selectedConstitutionArrow.text =
                    GetArrowByStats(equipData.constitution, selectedEquipData.constitution);
                selectedConstitutionBack.SetActive(selectedEquipData.constitution != 0.0f);

                selectedAgility.text = "+" + selectedEquipData.agility;
                selectedAgilityArrow.text = GetArrowByStats(equipData.agility, selectedEquipData.agility);
                selectedAgilityBack.SetActive(selectedEquipData.agility != 0.0f);

                selectedAccuracy.text = "+" + selectedEquipData.accuracy * 100 + "%";
                selectedAccuracyArrow.text = GetArrowByStats(equipData.accuracy, selectedEquipData.accuracy);
                selectedAccuracyBack.SetActive(selectedEquipData.accuracy != 0.0f);

                selectedDodge.text = "+" + selectedEquipData.dodge * 100 + "%";
                selectedDodgeArrow.text = GetArrowByStats(equipData.dodge, selectedEquipData.dodge);
                selectedDodgeBack.SetActive(selectedEquipData.dodge != 0.0f);

                selectedCritRate.text = "+" + selectedEquipData.critrate * 100 + "%";
                selectedCritRateArrow.text = GetArrowByStats(equipData.critrate, selectedEquipData.critrate);
                selectedCritRateBack.SetActive(selectedEquipData.critrate != 0.0f);

                selectedDamage.text = "+" + selectedEquipData.damage;
                selectedDamageArrow.text = GetArrowByStats(equipData.damage, selectedEquipData.damage);
                selectedDamageBack.SetActive(selectedEquipData.damage != 0.0f);
            }

            private void CustomizeEquippedItem()
            {
                equippedItemIcon.sprite = ResourceManager.LoadSprite(equipData.icon);
                equippedItemName.text = equipData.name;

                equippedSpeed.text = "+" + equipData.speed;
                equippedSpeedBack.SetActive(equipData.speed != 0);

                equippedPower.text = "+" + equipData.power;
                equippedPowerBack.SetActive(equipData.power != 0);

                equippedConstitution.text = "+" + equipData.constitution;
                equippedConstitutionBack.SetActive(equipData.constitution != 0);

                equippedAgility.text = "+" + equipData.agility;
                equippedAgilityBack.SetActive(equipData.agility != 0);

                equippedAccuracy.text = "+" + equipData.accuracy * 100 + "%";
                equippedAccuracyBack.SetActive(equipData.accuracy != 0);

                equippedDodge.text = "+" + equipData.dodge * 100 + "%";
                equippedDodgeBack.SetActive(equipData.dodge != 0.0f);

                equippedCritrate.text = "+" + equipData.critrate * 100 + "%";
                equippedCritrateBack.SetActive(equipData.critrate != 0.0f);

                equippedDamage.text = "+" + equipData.damage;
                equippedDamageBack.SetActive(equipData.damage != 0);
            }

            private string GetArrowByStats(float equippedItemStat, float selectedItemStat)
            {
                if (equippedItemStat == selectedItemStat)
                {
                    return "";
                }

                return equippedItemStat < selectedItemStat ? TMPSprite.IconArrowUp : TMPSprite.IconArrowDown;
            }

            protected virtual void MissClickButtonClick()
            {
                Destroy(gameObject);
            }
            
            private async void EquipButtonClick()
            {
                var overlordData = GameData.characters.overlord;

                if (overlordData.id.HasValue && selectedEquipId.HasValue)
                {
                    SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                    await GameData.equipment.Equip(overlordData.id.Value, selectedEquipId.Value);
                    OnEquip?.Invoke(equipId, selectedEquipId.Value);
                }
                
                Destroy(gameObject);
            }

            public static EquipInfoPopup GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<EquipInfoPopup>(
                    "Prefabs/UI/Screens/OverlordScreen/EquipInfoPopup", parent);
            }
        }
    }
}