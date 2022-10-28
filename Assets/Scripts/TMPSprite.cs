using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overlewd
{
    public abstract class TMPSprite
    {
        //asset ClassesNLevel
        public const string ClassAssassin = "<sprite=\"ClassesNLevel\" name=\"ClassAssassin\">";
        public const string ClassBruiser = "<sprite=\"ClassesNLevel\" name=\"ClassBruiser\">";
        public const string ClassCaster = "<sprite=\"ClassesNLevel\" name=\"ClassCaster\">";
        public const string ClassHealer = "<sprite=\"ClassesNLevel\" name=\"ClassHealer\">";
        public const string ClassOverlord = "<sprite=\"ClassesNLevel\" name=\"ClassOverlord\">";
        public const string ClassTank = "<sprite=\"ClassesNLevel\" name=\"ClassTank\">";
        public const string ClassBoss = "<sprite=\"ClassesNLevel\" name=\"ClassBoss\">";
        public const string BackLevelMax = "<sprite=\"ClassesNLevel\" name=\"BackLevelMax\">";
        public const string BackLevel = "<sprite=\"ClassesNLevel\" name=\"BackLevel\">";
        
        //asset Resources
        public const string Energy = "<sprite=\"AssetResources\" name=\"Energy\">";
        public const string Crystal = "<sprite=\"AssetResources\" name=\"Crystal\">";
        public const string Wood = "<sprite=\"AssetResources\" name=\"Wood\">";
        public const string Gold = "<sprite=\"AssetResources\" name=\"Gold\">";
        public const string Gem = "<sprite=\"AssetResources\" name=\"Gem\">";
        public const string Copper = "<sprite=\"AssetResources\" name=\"Copper\">";
        public const string Stone = "<sprite=\"AssetResources\" name=\"Stone\">";
        public const string ShardHeroic = "<sprite=\"AssetResources\" name=\"ShardHeroic\">";
        public const string ShardEpic = "<sprite=\"AssetResources\" name=\"ShardEpic\">";
        public const string ShardAdvanced = "<sprite=\"AssetResources\" name=\"ShardAdvanced\">";
        public const string ShardBasic = "<sprite=\"AssetResources\" name=\"ShardBasic\">";
        public const string Scroll = "<sprite=\"AssetResources\" name=\"Scroll\">";
        
        //asset EventCurrency
        public const string EventCurrencyEar = "<sprite=\"AssetEventCurrency\" name=\"Ear\">";
        public const string EventCurrencyNutakuGold = "<sprite=\"AssetEventCurrency\" name=\"NutakuGold\">";
        
        //asset BuffsNDebuffs
        public const string BuffDispell = "<sprite=\"BuffsNDebuffs\" name=\"BuffDispell\">";
        public const string BuffRegeneration = "<sprite=\"BuffsNDebuffs\" name=\"BuffRegeneration\">";
        public const string BuffFocus = "<sprite=\"BuffsNDebuffs\" name=\"BuffFocus\">";
        public const string BuffAttackUp = "<sprite=\"BuffsNDebuffs\" name=\"BuffAttackUp\">";
        public const string BuffSafeguard = "<sprite=\"BuffsNDebuffs\" name=\"BuffSafeguard\">";
        public const string BuffImmunity = "<sprite=\"BuffsNDebuffs\" name=\"BuffImmunity\">";
        public const string BuffDefenceUp = "<sprite=\"BuffsNDebuffs\" name=\"BuffDefenceUp\">";
        public const string BuffBless = "<sprite=\"BuffsNDebuffs\" name=\"BuffBless\">";
        
        public const string DebuffStun = "<sprite=\"BuffsNDebuffs\" name=\"DebuffStun\">";
        public const string DebuffBlind = "<sprite=\"BuffsNDebuffs\" name=\"DebuffBlind\">";
        public const string DebuffPoison = "<sprite=\"BuffsNDebuffs\" name=\"DebuffPoison\">";
        public const string DebuffDefenceDown = "<sprite=\"BuffsNDebuffs\" name=\"DebuffDefenceDown\">";
        public const string DebuffHealBlock = "<sprite=\"BuffsNDebuffs\" name=\"DebuffHealBlock\">";
        public const string DebuffCurse = "<sprite=\"BuffsNDebuffs\" name=\"DebuffCurse\">";
        public const string DebuffSilence = "<sprite=\"BuffsNDebuffs\" name=\"DebuffSilence\">";
        
        public const string IconImmunity = "<sprite=\"BuffsNDebuffs\" name=\"Immunity\">";
        public const string IconAttackUp = "<sprite=\"BuffsNDebuffs\" name=\"AttackUp\">";
        public const string IconRegeneration = "<sprite=\"BuffsNDebuffs\" name=\"Regeneration\">";
        public const string IconSafeguard = "<sprite=\"BuffsNDebuffs\" name=\"Safeguard\">";
        public const string IconBlind = "<sprite=\"BuffsNDebuffs\" name=\"Blind\">";
        public const string IconDefenceUp = "<sprite=\"BuffsNDebuffs\" name=\"DefenceUp\">";
        public const string IconSilence = "<sprite=\"BuffsNDebuffs\" name=\"Silence\">";
        public const string IconCurse = "<sprite=\"BuffsNDebuffs\" name=\"Curse\">";
        public const string IconPoison = "<sprite=\"BuffsNDebuffs\" name=\"Poison\">";
        public const string IconDefenceDown = "<sprite=\"BuffsNDebuffs\" name=\"DefenceDown\">";
        public const string IconHealBlock = "<sprite=\"BuffsNDebuffs\" name=\"HealBlock\">";
        public const string IconBless = "<sprite=\"BuffsNDebuffs\" name=\"Bless\">";
        public const string IconFocus = "<sprite=\"BuffsNDebuffs\" name=\"Focus\">";
        public const string IconStun = "<sprite=\"BuffsNDebuffs\" name=\"Stun\">";
        public const string IconDispell = "<sprite=\"BuffsNDebuffs\" name=\"Dispell\">";
        public const string IconArrowUp = "<sprite=\"BuffsNDebuffs\" name=\"ArrowUp\">";
        public const string IconArrowDown = "<sprite=\"BuffsNDebuffs\" name=\"ArrowDown\">";
        
        //asset NotificationNMarkers
        public const string MarkerSideQuest = "<sprite=\"NotificationsNMarkers\" name=\"MarkerSideQuest\">";
        public const string MarkerMainQuest = "<sprite=\"NotificationsNMarkers\" name=\"MarkerMainQuest\">";
        public const string MarkerQuarterlyEvent = "<sprite=\"NotificationsNMarkers\" name=\"MarkerQuarterlyEvent\">";
        public const string MarkerMonthlyEvent = "<sprite=\"NotificationsNMarkers\" name=\"MarkerMonthlyEvent\">";
        public const string MarkerWeeklyEvent = "<sprite=\"NotificationsNMarkers\" name=\"MarkerWeeklyEvent\">";
        
        public const string NotificationError = "<sprite=\"NotificationsNMarkers\" name=\"NotificationError\">";
        public const string NotificationSale = "<sprite=\"NotificationsNMarkers\" name=\"NotificationSale\">";
        public const string NotificationNew = "<sprite=\"NotificationsNMarkers\" name=\"NotificationNew\">";
        public const string NotificationTimeLimit = "<sprite=\"NotificationsNMarkers\" name=\"NotificationTimeLimit\">";
        public const string NotificationDone = "<sprite=\"NotificationsNMarkers\" name=\"NotificationDone\">";
        public const string NotificationLock = "<sprite=\"NotificationsNMarkers\" name=\"NotificationLock\">";
    }
}
