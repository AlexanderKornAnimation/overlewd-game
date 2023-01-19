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
        WalletChangeState,

        QuestsUpdate,
        QuestClaimRewards,

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
        public override void Handle()
        {
            WalletChangeNotifManager.Show(this);
        }
    }

    public class QuestClaimRewardsDataEvent : GameDataEvent
    {
        public int? questId { get; set; }
        public override void Handle()
        {
            var qData = GameData.quests.GetById(questId);
            if (qData != null)
            {
                foreach (var r in qData.rewards.FindAll(r => !r.isCurrency))
                {
                    PopupNotifManager.PushNotif(new PopupNotifWidget.InitSettings
                    {
                        title = "Claim",
                        message = $"{ r.amount } { r.tmpSprite }"
                    });
                }
            }
        }
    }

    public class QuestsUpdateDataEvent : GameDataEvent
    {
        public override void Handle()
        {
            foreach (var q in GameData.quests.quests)
            {
                if (q.isLastAdded)
                {
                    q.isLastAdded = false;
                    PopupNotifManager.PushNotif(new PopupNotifWidget.InitSettings
                    {
                        title = "Add quest",
                        message = q.name
                    });
                }
            }
        }
    }
}
