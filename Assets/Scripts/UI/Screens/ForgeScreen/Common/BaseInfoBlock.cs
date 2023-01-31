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
            protected Image consumeShardIcon;
            protected Image consumeIcon;

            protected Transform targetCell;
            protected TextMeshProUGUI targetCount;
            protected Image targetSubstrate;
            protected Image targetShardIcon;
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
                consumeShardIcon = consumeCell.Find("ShardIcon").GetComponent<Image>();
                consumeIcon = consumeCell.Find("Icon").GetComponent<Image>();

                targetCell = transform.Find("TargetCell");
                targetCount = targetCell.Find("Counter/Count").GetComponent<TextMeshProUGUI>();
                decButton = targetCell.Find("Counter/Dec").GetComponent<Button>();
                decButton.onClick.AddListener(DecClick);
                incButton = targetCell.Find("Counter/Inc").GetComponent<Button>();
                incButton.onClick.AddListener(IncClick);
                targetSubstrate = targetCell.Find("Substrate").GetComponent<Image>();
                targetShardIcon = targetCell.Find("ShardIcon").GetComponent<Image>();
                targetIcon = targetCell.Find("Icon").GetComponent<Image>();
            }

            protected virtual void IncClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                targetCount.text = (targetCountValue + 1).ToString();
            }

            protected virtual void DecClick()
            {
                SoundManager.PlayOneShot(FMODEventPath.UI_GenericButtonClick);
                targetCount.text = (targetCountValue - 1).ToString();
            }

            public int targetCountValue => int.Parse(targetCount.text);

            protected Sprite GetShardSubstrate(string rarity) => rarity switch
            {
                AdminBRO.Rarity.Basic => Resources.Load<Sprite>("Common/Images/Shards/BasicShardWithSubstrate"),
                AdminBRO.Rarity.Advanced => Resources.Load<Sprite>("Common/Images/Shards/AdvancedShardWithSubstrate"),
                AdminBRO.Rarity.Epic => Resources.Load<Sprite>("Common/Images/Shards/EpicShardWithSubstrate"),
                AdminBRO.Rarity.Heroic => Resources.Load<Sprite>("Common/Images/Shards/HeroicShardWithSubstrate"),
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
                AdminBRO.MatriarchItem.Key_Ulvi => Resources.Load<Sprite>("Prefabs/UI/Screens/ForgeScreen/Images/Slots/UlviHeadShardsForge"),
                AdminBRO.MatriarchItem.Key_Adriel => Resources.Load<Sprite>("Prefabs/UI/Screens/ForgeScreen/Images/Slots/AdrielHeadShardsForge"),
                AdminBRO.MatriarchItem.Key_Inge => Resources.Load<Sprite>("Prefabs/UI/Screens/ForgeScreen/Images/Slots/IngieHeadShardsForge"),
                AdminBRO.MatriarchItem.Key_Faye => Resources.Load<Sprite>("Prefabs/UI/Screens/ForgeScreen/Images/Slots/FayeHeadShardsForge"),
                AdminBRO.MatriarchItem.Key_Lili => Resources.Load<Sprite>("Prefabs/UI/Screens/ForgeScreen/Images/Slots/LiliHeadShardsForge"),
                _ => null
            };
        }
    }
}
