using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Overlewd
{
    public enum GameDataEventId
    {
        None,

        BuyTradable,
        NutakuPayment,
        WalletChangeState,
        EntitiesIncome,

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
        public GameDataEventId id { get; set; } = GameDataEventId.None;
        public T As<T>() where T : GameDataEvent =>
            this as T;
        public bool Is<T>() where T : GameDataEvent =>
            this is T;
        public virtual void Handle()
        {

        }
    }

    public class MagicGuildDataEvent : GameDataEvent
    {
        public string skillType { get; set; }
    }

    public class GachaDataEvent : GameDataEvent
    {
        public List<AdminBRO.GachaBuyResult> buyResult { get; set; }
    }

    public class WalletChangeStateDataEvent : GameDataEvent
    {
        public AdminBRO.PlayerInfo fromInfo { get; set; }
        public AdminBRO.PlayerInfo toInfo { get; set; }
        public bool hasWalletChanges => (fromInfo == null) ? false :
            (fromInfo.Copper.amount != toInfo.Copper.amount) ||
            (fromInfo.Crystal.amount != toInfo.Crystal.amount) ||
            (fromInfo.Wood.amount != toInfo.Wood.amount) ||
            (fromInfo.Gold.amount != toInfo.Gold.amount) ||
            (fromInfo.Stone.amount != toInfo.Stone.amount) ||
            (fromInfo.Gems.amount != toInfo.Gems.amount);
        public bool hasPotionsChanges => (fromInfo == null) ? false :
            (fromInfo.hpPotionAmount != toInfo.hpPotionAmount) ||
            (fromInfo.manaPotionAmount != toInfo.manaPotionAmount) ||
            (fromInfo.energyPotionAmount != toInfo.energyPotionAmount) ||
            (fromInfo.replayAmount != toInfo.replayAmount);
        public bool hasEventWalletChanges => (fromInfo == null) ? false :
            toInfo.walletEvent.Exists(we => fromInfo.walletEvent.Exists(pwe => pwe.currencyId == we.currencyId && pwe.amount != we.amount));
        public bool hasAnyChanges =>
            hasWalletChanges ||
            hasPotionsChanges ||
            hasEventWalletChanges;

        public override void Handle()
        {
            if (hasWalletChanges)
            {
                WalletChangeNotifManager.Show(this);
            }

            if (hasPotionsChanges)
            {
                WalletChangeNotifWidget.TryPopup(fromInfo.hpPotionAmount, toInfo.hpPotionAmount, TMPSprite.BottleHealth);
                WalletChangeNotifWidget.TryPopup(fromInfo.manaPotionAmount, toInfo.manaPotionAmount, TMPSprite.BottleMana);
                WalletChangeNotifWidget.TryPopup(fromInfo.energyPotionAmount, toInfo.energyPotionAmount, TMPSprite.BottleEnergy);
                WalletChangeNotifWidget.TryPopup(fromInfo.replayAmount, toInfo.replayAmount, TMPSprite.Scroll);
            }

            if (hasEventWalletChanges)
            {
                foreach (var we in toInfo.walletEvent)
                {
                    var pwe = fromInfo.walletEvent.Find(_pwe => _pwe.currencyId == we.currencyId);
                    if (pwe != null)
                    {
                        WalletChangeNotifWidget.TryPopup(pwe.amount, we.amount, we.currencyData.tmpSprite);
                    }
                }
            }
        }
    }

    public class EntitiesIncomeDataEvent : GameDataEvent
    {
        public List<AdminBRO.QuestItem> fromQuests { get; set; }
        public List<AdminBRO.QuestItem> toQuests { get; set; }
        public List<AdminBRO.QuestItem> lastAddedQuests => ((fromQuests?.Count ?? 0) > 0 && toQuests != null) ?
            toQuests.FindAll(q => !fromQuests.Exists(pq => pq.id == q.id)) : new List<AdminBRO.QuestItem>();


        public List<AdminBRO.MemoryShardItem> fromMemoryShards { get; set; }
        public List<AdminBRO.MemoryShardItem> toMemoryShards { get; set; }
        public List<int> lastChangedMemoryShards => ((fromMemoryShards?.Count ?? 0) > 0 && toMemoryShards != null) ?
            toMemoryShards.
            FindAll(s => fromMemoryShards.Exists(ps => (ps.id == s.id) && (ps.amount != s.amount))).
            Select(s => s.id).ToList() :
            new List<int>();

        public List<AdminBRO.Equipment> fromEquipments { get; set; }
        public List<AdminBRO.Equipment> toEquipments { get; set; }
        public List<AdminBRO.Equipment> lastAddedEquipments => ((fromEquipments?.Count ?? 0) > 0 && toEquipments != null) ?
            toEquipments.FindAll(e => !fromEquipments.Exists(pe => pe.id == e.id)) : new List<AdminBRO.Equipment>();

        public List<AdminBRO.Character> fromCharacters { get; set; }
        public List<AdminBRO.Character> toCharacters { get; set; }
        public List<AdminBRO.Character> lastAddedCharacters => ((fromCharacters?.Count ?? 0) > 0 && toCharacters != null) ?
            toCharacters.FindAll(c => !fromCharacters.Exists(pc => pc.id == c.id)) : new List<AdminBRO.Character>();

        public override void Handle()
        {
            foreach (var q in lastAddedQuests)
            {
                PopupNotifManager.PushNotif(new PopupNotifWidget.InitSettings
                {
                    title = "Add quest",
                    message = q.name
                });
            }

            foreach (var shardId in lastChangedMemoryShards)
            {
                var fromShardState = fromMemoryShards.Find(s => s.id == shardId);
                var toShardState = toMemoryShards.Find(s => s.id == shardId);
                PopupNotifManager.PushNotif(new PopupNotifWidget.InitSettings
                {
                    title = $"Add {toShardState.matriarchData?.name} shards",
                    message = $"+{toShardState.amount - fromShardState.amount} {toShardState.tmpSprite}"
                });
            }

            foreach (var e in lastAddedEquipments)
            {
                PopupNotifManager.PushNotif(new PopupNotifWidget.InitSettings
                {
                    title = "Add equipment",
                    message = $"{e.name}"
                });
            }

            foreach (var c in lastAddedCharacters)
            {
                PopupNotifManager.PushNotif(new PopupNotifWidget.InitSettings
                {
                    title = "Add battle character",
                    message = $"{c.name}"
                });
            }
        }
    }
}
