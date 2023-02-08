using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace Overlewd
{
    public static class FMODEventPath
    {
        //buttons
        public const string UI_CastleScreenButtons = "event:/UI/Buttons/Castle_Screen/Basic_menu_click";
        public const string UI_GenericButtonClick = "event:/UI/Buttons/Generic/Button_click";
        public const string UI_DialogNextButtonClick = "event:/UI/Buttons/Dialogue/Text_field_click_next";
        public const string UI_DefeatPopupHaremButtonClick = "event:/UI/PopUps/Battle/Boosts/Matriarch_Boost";
        public const string UI_FreeBuildButton = "event:/UI/Buttons/Building/Build_8_hours_button";
        public const string UI_FreeSpellLearnButton = "event:/UI/Buttons/Learn_Spell/Learn_Spell_8_hours";
        public const string UI_StartBattle = "event:/UI/Buttons/Battle/Start_battle_for_crystals";

        //screens
        public const string UI_CastleWindowShow = "event:/UI/Windows/Castle_Screen/Window_slide_on";
        public const string UI_CastleWindowHide = "event:/UI/Windows/Castle_Screen/Window_slide_off";
        public const string UI_GenericWindowShow = "event:/UI/Windows/Generic/Window_slide_on";
        public const string UI_GenericWindowHide = "event:/UI/Windows/Generic/Window_slide_off";
        public const string UI_BattleScreenShow = "event:/UI/Windows/Battle_Screen/Battle_Screen_Transition";
        public const string UI_PortalScreenFTUE = "event:/Animations/Chapter_Scenes/Chapter-1/Portals_Chamber_Build";

        //overlays
        public const string UI_SidebarOverlayShow = "event:/UI/Windows/Castle_Screen/Deeds_slide_on";
        public const string UI_SidebarOverlayHide = "event:/UI/Windows/Castle_Screen/Deeds_slide_off";
        public const string UI_QuestOverlayShow = "event:/UI/Buttons/Castle_Screen/Quests_menu_slide_on";
        public const string UI_QuestOverlayHide = "event:/UI/Buttons/Castle_Screen/Quests_menu_slide_off";

        //popups & nootifications
        public const string UI_GenericPopupShow = "event:/UI/Windows/Generic/Window_popup_slide";
        public const string UI_GenericDialogNotificationShow = "event:/UI/PopUps/Text/Generic_Text_Window_PopUp";

        //battle
        //Sound for click at mana potion button
        public const string UI_Battle_Healpotion = "event:/UI/Battle_Interface/Misc/Healpotion";
        //Sound for click at mana potion button
        public const string UI_Battle_Manapotion = "event:/UI/Battle_Interface/Misc/Manapotion";
        //Sound for melee attack
        public const string UI_Battle_Attack = "event:/UI/Battle_Interface/Actions/Attack";
        //Sound for selection character
        public const string UI_Battle_SelectPers = "event:/UI/Battle_Interface/Misc/Hero_select";
        //Sound for Overlord's basic attack. Should be played after starting animation
        public const string Battle_Overlord_hit = "event:/Animations/Battle/Generic_Hits/Overlord_hit";
        //Sound for Overlord's first AOE attack 
        public const string Batlle_Overlord_AOE_Hits_AOE1 = "event:/Animations/Battle/Overlord_AOE_Hits/AOE1";
        

        //screens bg music
        public const string Castle_Screen_BGM_Attn = "snapshot:/Castle_Screen_BGM_Attn";//�������� ����� �������� �����
        public const string Music_CastleScreen = "event:/Music/CastleScreen/CastleScreen_BGM";
        public const string Music_SexScreen = "event:/Music/LewdScreen/BGM_SexScene_1";
        public const string Music_DialogScreen = "event:/Music/DialogueScreen/BGM_Dialogue_Chill_1";
        public const string Music_MapScreen = "event:/Music/MapScreen/BGM_MapScene_1";
        public const string Music_Battle_BGM_1 = "event:/Music/BattleScreen/Battle_BGM_1";

        //gacha
        public const string Gacha_x1_open = "event:/Animations/Gacha/x1/Gacha_x1_open";
        public const string Gacha_x10_open = "event:/Animations/Gacha/x10/Gacha_x10_open";
        public const string SFX_UI_Portal_Portal_Animation = "event:/Animations/Gacha/Portal/Portal_Animation";
        public const string SFX_UI_Gacha_Shards_x1_reveal  = "event:/Animations/Gacha/x1/Gacha_x1_reveal";

        //Shop
        public const string SFX_UI_Shop_Buy_Success = "event:/UI/Shop/Buy_Success";
        public const string SFX_UI_Shop_Buy_Fail = "event:/UI/Shop/Buy_Failure";

        //castle screen
        public const string Castle_BuildingAppear = "event:/Animations/Castle_Scenes/Building_Appear";
        public const string Castle_BuildingUpgrade = "event:/Animations/Castle_Scenes/Building_Upgrade";

        //Items_Management
        public const string SFX_UI_Equip_ON = "event:/UI/Equipment/Equip_ON";
        public const string SFX_UI_Equip_OFF = "event:/UI/Equipment/Equip_OFF";

        //VO
        //������ / ��������� � ���. ����������� ������ ���� ������ �� ���� ������������ �������.
        public const string VO_Ulvi_Winning_a_battle = "event:/VO/Placeholders/Ulvi/Reactions/Winning_a_battle";
        public const string VO_Ulvi_Losing_a_battle = "event:/VO/Placeholders/Ulvi/Reactions/Losing_a_battle";
        public const string VO_Adriel_Winning_a_battle = "event:/VO/Placeholders/Adriel/Reactions/Winning_a_battle";
        public const string VO_Adriel_Losing_a_battle = "event:/VO/Placeholders/Adriel/Reactions/Losing_a_battle";
        public const string VO_Inge_Winning_a_battle = "event:/VO/Placeholders/Inge/Reactions/Winning_a_battle";
        public const string VO_Inge_Losing_a_battle = "event:/VO/Placeholders/Inge/Reactions/Losing_a_battle";
        //��� ��������� ������ �� ���������.
        public const string VO_Ulvi_equipping_armor = "event:/VO/Placeholders/Ulvi/Reactions/equipping_armor";
        public const string VO_Adriel_equipping_armor = "event:/VO/Placeholders/Adriel/Reactions/equipping_armor";
        public const string VO_Inge_equipping_armor = "event:/VO/Placeholders/Inge/Reactions/equipping_armor";
        //��� ������ �� ������ (������� �� ��������):
        public const string VO_Ulvi_Reactions_catacombs = "event:/VO/Placeholders/Ulvi/Reactions/catacombs";
        public const string VO_Ulvi_Reactions_forge = "event:/VO/Placeholders/Ulvi/Reactions/forge";
        public const string VO_Ulvi_Reactions_harem = "event:/VO/Placeholders/Ulvi/Reactions/harem";
        public const string VO_Ulvi_Reactions_laboratory = "event:/VO/Placeholders/Ulvi/Reactions/laboratory";
        public const string VO_Ulvi_Reactions_mages_guild = "event:/VO/Placeholders/Ulvi/Reactions/mages_guild";
        public const string VO_Ulvi_Reactions_portal = "event:/VO/Placeholders/Ulvi/Reactions/portal";
        public const string VO_Ulvi_Reactions_battle_girls = "event:/VO/Placeholders/Ulvi/Reactions/battle_girls";
        public const string VO_Ulvi_Reactions_matriarch_screen = "event:/VO/Placeholders/Ulvi/Reactions/matriarch_screen";
        public const string VO_Ulvi_Reactions_market = "event:/VO/Placeholders/Ulvi/Reactions/market";
        public const string VO_Ulvi_Reactions_eventbook = "event:/VO/Placeholders/Ulvi/Reactions/eventbook";

        public const string VO_Adriel_Reactions_catacombs = "event:/VO/Placeholders/Adriel/Reactions/catacombs";
        public const string VO_Adriel_Reactions_forge = "event:/VO/Placeholders/Adriel/Reactions/forge";
        public const string VO_Adriel_Reactions_harem = "event:/VO/Placeholders/Adriel/Reactions/harem";
        public const string VO_Adriel_Reactions_laboratory = "event:/VO/Placeholders/Adriel/Reactions/laboratory";
        public const string VO_Adriel_Reactions_mages_guild = "event:/VO/Placeholders/Adriel/Reactions/mages_guild";
        public const string VO_Adriel_Reactions_portal = "event:/VO/Placeholders/Adriel/Reactions/portal";
        public const string VO_Adriel_Reactions_battle_girls = "event:/VO/Placeholders/Adriel/Reactions/battle_girls";
        public const string VO_Adriel_Reactions_matriarch_screen = "event:/VO/Placeholders/Adriel/Reactions/matriarch_screen";
        public const string VO_Adriel_Reactions_market = "event:/VO/Placeholders/Adriel/Reactions/market";
        public const string VO_Adriel_Reactions_eventbook = "event:/VO/Placeholders/Adriel/Reactions/eventbook";

        public const string VO_Ingie_Reactions_catacombs = "event:/VO/Placeholders/Ingie/Reactions/catacombs";
        public const string VO_Ingie_Reactions_forge = "event:/VO/Placeholders/Ingie/Reactions/forge";
        public const string VO_Ingie_Reactions_harem = "event:/VO/Placeholders/Ingie/Reactions/harem";
        public const string VO_Ingie_Reactions_laboratory = "event:/VO/Placeholders/Ingie/Reactions/laboratory";
        public const string VO_Ingie_Reactions_mages_guild = "event:/VO/Placeholders/Ingie/Reactions/mages_guild";
        public const string VO_Ingie_Reactions_portal = "event:/VO/Placeholders/Ingie/Reactions/portal";
        public const string VO_Ingie_Reactions_battle_girls = "event:/VO/Placeholders/Ingie/Reactions/battle_girls";
        public const string VO_Ingie_Reactions_matriarch_screen = "event:/VO/Placeholders/Ingie/Reactions/matriarch_screen";
        public const string VO_Ingie_Reactions_market = "event:/VO/Placeholders/Ingie/Reactions/market";
        public const string VO_Ingie_Reactions_eventbook = "event:/VO/Placeholders/Ingie/Reactions/eventbook";
        //��� �� ��� ��������� ��������� / ���������� � ����� ����� ����������� ������ �����:
        public const string VO_Ulvi_loyalty_increase = "event:/VO/Placeholders/Ulvi/Reactions/loyalty_increase";
        public const string VO_Adriel_loyalty_increase = "event:/VO/Placeholders/Adriel/Reactions/loyalty_increase";
        public const string VO_Ingie_loyalty_increase = "event:/VO/Placeholders/Inge/Reactions/loyalty_increase";
    }

    public class FMODBank
    {
        private Bank bank;
        private RESULT status;

        private FMODBank(string path)
        {
            var bankInMemory = File.ReadAllBytes(path);
            status = RuntimeManager.StudioSystem.loadBankMemory(bankInMemory,
                LOAD_BANK_FLAGS.NORMAL,
                out bank);
        }

        public void Unload()
        {
            bank.unload();
            RuntimeManager.StudioSystem.update();
        }

        public bool IsValid()
        {
            return status == RESULT.OK;
        }

        public static FMODBank LoadFromFile(string bankPath)
        {
            var bank = new FMODBank(bankPath);
            if (bank.IsValid())
            {
                return bank;
            }

            bank.Unload();
            return null;
        }
    }

    public class FMODEvent
    {
        private EventInstance instance;
        public string path { get; private set; }

        private FMODEvent(string eventPath)
        {
            instance = RuntimeManager.CreateInstance(eventPath);
            path = eventPath;
            instance.start();
        }

        public bool IsPlaying()
        {
            PLAYBACK_STATE state;
            instance.getPlaybackState(out state);
            return (state != PLAYBACK_STATE.STOPPED) &&
                (state != PLAYBACK_STATE.STOPPING);
        }

        public void Play()
        {
            instance.setPaused(false);
        }

        public void Pause()
        {
            instance.setPaused(true);
        }

        public void Stop(bool allowFade = true)
        {
            var stopMode = allowFade ?
                FMOD.Studio.STOP_MODE.ALLOWFADEOUT :
                FMOD.Studio.STOP_MODE.IMMEDIATE;
            instance.stop(stopMode);
            instance.release();
            SoundManager.RemoveEvent(this);
        }

        public void SetPitch(float pitch)
        {
            instance.setPitch(pitch);
        }

        public static FMODEvent GetInstance(string eventPath)
        {
            EventDescription eventDesc;
            RuntimeManager.StudioSystem.getEvent(eventPath, out eventDesc);
            if (eventDesc.isValid())
            {
                return new FMODEvent(eventPath);
            }
            UnityEngine.Debug.LogWarning($"event not found: {eventPath}");
            return null;
        }
    }

    public static class SoundManager
    {
        private static FMODEvent bgMusic;
        private static List<FMODEvent> instances = new List<FMODEvent>();
        private static List<EventDescription> localEvents = new List<EventDescription>();

        public static IEnumerator<int> PreloadBanks()
        {
            RuntimeManager.LoadBank("BGM");
            RuntimeManager.LoadBank("Master");
            RuntimeManager.LoadBank("Voiceovers_local");
            RuntimeManager.LoadBank("Master.strings");

            while (!RuntimeManager.HaveAllBanksLoaded)
            {
                yield return 0;
            }

#if UNITY_WEBGL
            RuntimeManager.CoreSystem.mixerSuspend();
            RuntimeManager.CoreSystem.mixerResume();
#endif
        }

        public static void Initialize()
        {
            Bank[] banks;
            RuntimeManager.StudioSystem.getBankList(out banks);
            foreach (var bank in banks)
            {
                EventDescription[] bankEvents;
                bank.getEventList(out bankEvents);
                foreach (var eventItem in bankEvents)
                {
                    localEvents.Add(eventItem);
                }
            }
        }

        public static bool HasLocalEvent(string eventPath)
        {
            return localEvents.Exists(item => 
                {
                    string localEventPath;
                    item.getPath(out localEventPath);
                    return localEventPath == eventPath.Trim();
                });
        }

        public static void PlayBGMusic(string eventPath)
        {
            if (bgMusic?.path != eventPath)
            {
                bgMusic?.Stop();
                bgMusic = FMODEvent.GetInstance(eventPath);
            }
        }

        public static void StopBGMusic()
        {
            bgMusic?.Stop();
            bgMusic = null;
        }

        public static FMODEvent GetEventInstance(string eventPath, string bankId = null)
        {
            if (String.IsNullOrEmpty(eventPath))
                return null;

            if (!HasLocalEvent(eventPath) && !String.IsNullOrEmpty(bankId))
            {
                ResourceManager.LoadFMODBank(bankId);
            }

            var inst = instances.Find(item => (item.path == eventPath.Trim()) && item.IsPlaying());
            if (inst != null)
            {
                return inst;
            }

            var newInst = FMODEvent.GetInstance(eventPath.Trim());
            if (newInst != null)
            {
                instances.Add(newInst);
                return newInst;
            }

            return null;
        }

        public static void PlayOneShot(string eventPath, string bankId = null)
        {
            if (String.IsNullOrEmpty(eventPath))
                return;

            if (!HasLocalEvent(eventPath) && !String.IsNullOrEmpty(bankId))
            {
                ResourceManager.LoadFMODBank(bankId);
            }

            RuntimeManager.PlayOneShot(eventPath.Trim());
        }

        public static void StopAll()
        {
            foreach (var inst in instances.ToList())
            {
                inst.Stop();
            }
        }

        public static void RemoveEvent(FMODEvent soundEvent)
        {
            if (soundEvent.IsPlaying())
            {
                soundEvent.Stop();
            }
            else
            {
                instances.Remove(soundEvent);
            }
        }
    }
}