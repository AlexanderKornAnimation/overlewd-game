using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public enum GameDataEventId
    {
        None,

        BuyTradable,
        NutakuPayment,
        WalletStateChange,

        BuildingBuild,
        BuildingBuildCrystal,

        CharacterLvlUp,
        CharacterSkillLvlUp,
        CharacterMerge,

        MagicGuildSpellLvlUp,

        EquipmentEquipped,
        EquipmentUnequipped,

        BuyPotions,
        UsePotions,

        GachaBuy,

        ForgeMergeShards,
        ForgeExchangeShards,
        ForgeMergeEquip,
        
        PieceOfMemoryBuy,
    }

    public class GameDataEvent
    {
        public GameDataEventId id = GameDataEventId.None;
        public T As<T>() where T : GameDataEvent =>
            this as T;
    }

    public class MagicGuildDataEvent : GameDataEvent
    {
        public string skillType { get; set; }
    }

    public class GachaDataEvent : GameDataEvent
    {
        public List<AdminBRO.GachaBuyResult> buyResult { get; set; }
    }

    public class PlayerInfoDataEvent : GameDataEvent
    {
        public List<AdminBRO.PlayerInfo.WalletItem> walletChanges { get; set; }
    }
}
