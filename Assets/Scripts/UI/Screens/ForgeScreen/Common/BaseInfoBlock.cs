using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Overlewd
{
    namespace NSForgeScreen
    {
        public abstract class BaseInfoBlock : MonoBehaviour
        {
            protected TextMeshProUGUI msgTitle;
            protected Transform arrow;

            protected Transform consumeCell;
            protected TextMeshProUGUI consumeCount;
            protected Image consumeSubstrate;
            protected Image consumeIcon;

            protected Transform targetCell;
            protected TextMeshProUGUI targetCount;
            protected Image targetSubstrate;
            protected Image targetIcon;
            protected Button decButton;
            protected Button incButton;

            void Awake()
            {
                msgTitle = transform.Find("MsgTitle").GetComponent<TextMeshProUGUI>();
                arrow = transform.Find("Arrow");

                consumeCell = transform.Find("ConsumeCell");
                consumeCount = consumeCell.Find("Counter/Count").GetComponent<TextMeshProUGUI>();
                consumeSubstrate = consumeCell.Find("Substrate").GetComponent<Image>();
                consumeIcon = consumeCell.Find("Icon").GetComponent<Image>();

                targetCell = transform.Find("TargetCell");
                targetCount = targetCell.Find("Counter/Count").GetComponent<TextMeshProUGUI>();
                decButton = targetCell.Find("Counter/Dec").GetComponent<Button>();
                decButton.onClick.AddListener(DecClick);
                incButton = targetCell.Find("Counter/Inc").GetComponent<Button>();
                incButton.onClick.AddListener(IncClick);
                targetSubstrate = targetCell.Find("Substrate").GetComponent<Image>();
                targetIcon = targetCell.Find("Icon").GetComponent<Image>();
            }

            protected virtual void IncClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            }

            protected virtual void DecClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
            }

            public int targetCountValue => int.Parse(targetCount.text);

            protected Sprite GetShardSubstrate(string rarity) => rarity switch
            {
                AdminBRO.Rarity.Basic => Resources.Load<Sprite>("Prefabs/UI/Screens/ForgeScreen/Images/Slots/BasicShardWithSubstrate"),
                AdminBRO.Rarity.Advanced => Resources.Load<Sprite>("Prefabs/UI/Screens/ForgeScreen/Images/Slots/AdvancedShardWithSubstrate"),
                AdminBRO.Rarity.Epic => Resources.Load<Sprite>("Prefabs/UI/Screens/ForgeScreen/Images/Slots/EpicShardWithSubstrate"),
                AdminBRO.Rarity.Heroic => Resources.Load<Sprite>("Prefabs/UI/Screens/ForgeScreen/Images/Slots/HeroicShardWithSubstrate"),
                _ => null
            };

            protected Sprite GetSubstrate(string rarity) => rarity switch
            {
                AdminBRO.Rarity.Basic => Resources.Load<Sprite>("Prefabs/UI/Screens/ForgeScreen/Images/Slots/EquipmentBasicFullSize"),
                AdminBRO.Rarity.Advanced => Resources.Load<Sprite>("Prefabs/UI/Screens/ForgeScreen/Images/Slots/EquipmentAdvancedFullSize"),
                AdminBRO.Rarity.Epic => Resources.Load<Sprite>("Prefabs/UI/Screens/ForgeScreen/Images/Slots/EquipmentEpicFullSize"),
                AdminBRO.Rarity.Heroic => Resources.Load<Sprite>("Prefabs/UI/Screens/ForgeScreen/Images/Slots/EquipmentHeroicFullSize"),
                _ => null
            };

            protected Sprite GetShardIcon(AdminBRO.MatriarchItem matriarchData) => matriarchData?.key switch
            {
                AdminBRO.MatriarchItem.Key_Ulvi => Resources.Load<Sprite>("Prefabs/UI/Screens/ForgeScreen/Images/Slots/UlviHeadShards"),
                AdminBRO.MatriarchItem.Key_Adriel => Resources.Load<Sprite>("Prefabs/UI/Screens/ForgeScreen/Images/Slots/AdrielHeadShards"),
                AdminBRO.MatriarchItem.Key_Ingie => Resources.Load<Sprite>("Prefabs/UI/Screens/ForgeScreen/Images/Slots/IngieHeadShards"),
                AdminBRO.MatriarchItem.Key_Faye => Resources.Load<Sprite>("Prefabs/UI/Screens/ForgeScreen/Images/Slots/FayeHeadShards"),
                AdminBRO.MatriarchItem.Key_Lili => Resources.Load<Sprite>("Prefabs/UI/Screens/ForgeScreen/Images/Slots/LiliHeadShards"),
                _ => null
            };
        }
    }
}
