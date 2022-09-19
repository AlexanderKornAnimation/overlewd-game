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

            public int? newEquipId;
            private AdminBRO.Equipment selectedEquipData => GameData.equipment.GetById(newEquipId);

            public event Action<int, int> OnEquip;

            private void Awake()
            {
                var canvas = transform.Find("Canvas");
                var equippedItem = canvas.Find("EquippedItem");

                equippedItemIcon = equippedItem.Find("EquipIcon").GetComponent<Image>();
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

                selectedItem = canvas.Find("NewItem").gameObject;
                selectedItemIcon = selectedItem.transform.Find("EquipIcon").GetComponent<Image>();

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

                equipButton = selectedItem.transform.Find("Substrate").Find("EquipButton").GetComponent<Button>();
                equipButton.onClick.AddListener(EquipButtonClick);
            }

            private void Start()
            {
                Customize();
            }

            private void Customize()
            {
                selectedItem.SetActive(newEquipId.HasValue);

                equippedItemIcon.sprite = ResourceManager.LoadSprite(equipData.icon);
                equippedConstitution.text = "+" + equipData.constitution;
                equippedConstitutionBack.SetActive(equipData.constitution != 0);

                equippedAgility.text = "+" + equipData.agility;
                equippedAgilityBack.SetActive(equipData.agility != 0);

                equippedAccuracy.text = "+" + equipData.accuracy * 100 + "%";
                equippedAccuracyBack.SetActive(equipData.accuracy != 0);

                equippedDodge.text = "+" + equipData.dodge * 100 + "%";
                equippedDodgeBack.SetActive(equipData.dodge != 0);

                equippedCritrate.text = "+" + equipData.critrate * 100 + "%";
                equippedCritrateBack.SetActive(equipData.critrate != 0);

                equippedDamage.text = "+" + equipData.damage;
                equippedDamageBack.SetActive(equipData.damage != 0);

                if (selectedItem.activeSelf)
                {
                    selectedItemIcon.sprite = ResourceManager.LoadSprite(selectedEquipData.icon);
                    selectedConstitution.text = "+" + selectedEquipData.constitution;
                    selectedConstitutionArrow.text = GetArrowByStats(equipData.constitution, selectedEquipData.constitution);
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
            }

            private string GetArrowByStats(float equippedItemStat, float selectedItemStat)
            {
                if (equippedItemStat == selectedItemStat)
                {
                    return "";
                }

                return equippedItemStat < selectedItemStat ? TMPSprite.IconArrowUp : TMPSprite.IconArrowDown;
            }

            private async void EquipButtonClick()
            {
                var overlordData = GameData.characters.overlord;

                if (overlordData.id.HasValue && newEquipId.HasValue)
                {
                    SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                    await GameData.equipment.Equip(overlordData.id.Value, newEquipId.Value);
                    OnEquip?.Invoke(equipId, newEquipId.Value);
                }
            }

            public static EquipInfoPopup GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<EquipInfoPopup>(
                    "Prefabs/UI/Screens/OverlordScreen/EquipInfoPopup", parent);
            }
        }
    }
}