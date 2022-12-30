using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSLaboratoryScreen
    {
        public class MergeInfoWidget : MonoBehaviour
        {
            private Button missClickButton;
            private Image characterIcon;
            private GameObject sexSceneStatus;
            private TextMeshProUGUI titleRarity;
            
            public int? chId { get; set; }
            public AdminBRO.Character chData => GameData.characters.GetById(chId);
            
            private void Awake()
            {
                UITools.SetStretch(transform as RectTransform);
                var canvas = transform.Find("Canvas");
                var background = canvas.Find("Background");
                
                missClickButton = canvas.Find("MissclickButton").GetComponent<Button>();
                missClickButton.onClick.AddListener(MissclickButtonClick);
                characterIcon = background.Find("CharacterIcon").GetComponent<Image>();
                sexSceneStatus = background.Find("SexSceneStatus").gameObject;
                titleRarity = background.Find("TitleRarity").GetComponent<TextMeshProUGUI>();
            }

            private void Start()
            {
                Customize();
            }

            private void Customize()
            {
                if (chData != null)
                {
                    characterIcon.sprite = ResourceManager.LoadSprite(chData.iconUrl);
                    sexSceneStatus.SetActive(chData.isSexSceneOpen);
                    titleRarity.text = $"Upgraded to\n{chData.rarity}";
                }
            }

            private void MissclickButtonClick()
            {
                Destroy(gameObject);
            }

            public static MergeInfoWidget GetInstance(Transform parent)
            {
                return ResourceManager.InstantiateWidgetPrefab<MergeInfoWidget>(
                    "Prefabs/UI/Screens/LaboratoryScreen/MergeInfoWidget", parent);
            }
        }
    }
}