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
        /// <summary>
        /// Sound for click at mana potion button
        /// </summary>
        public const string UI_Battle_Manapotion = "event:/UI/Battle_Interface/Misc/Manapotion";
        /// <summary>
        /// Sound for melee attack
        /// </summary>
        public const string UI_Battle_Attack = "event:/UI/Battle_Interface/Actions/Attack";
        /// <summary>
        /// Sound for selection character
        /// </summary>
        public const string UI_Battle_SelectPers = "event:/UI/Battle_Interface/Misc/Hero_select";
        /// <summary>
        /// Sound for Overlord's basic attack. Should be played after starting animation
        /// </summary>
        public const string Battle_Overlord_hit = "event:/Animations/Battle/Generic_Hits/Overlord_hit";
        /// <summary>
        /// Sound for Overlord's first AOE attack 
        /// </summary>
        public const string Batlle_Overlord_AOE_Hits_AOE1 = "event:/Animations/Battle/Overlord_AOE_Hits/AOE1";

        //battle bg music
        /// <summary>
        /// Background battle music
        /// </summary>
        public const string Music_Battle_BGM_1 = "event:/Music/BattleScreen/Battle_BGM_1";

        //screens bg music
        public const string Music_CastleScreen = "event:/Music/CastleScreen/CastleScreen_BGM";
        public const string Music_SexScreen = "event:/Music/LewdScreen/BGM_SexScene_1";
        public const string Music_DialogScreen = "event:/Music/DialogueScreen/BGM_Dialogue_Chill_1";
        public const string Music_MapScreen = "event:/Music/MapScreen/BGM_MapScene_1";

        //gacha
        public const string Gacha_x1_open = "event:/Animations/Gacha/x1/Gacha_x1_open";
        public const string Gacha_x10_open = "event:/Animations/Gacha/x10/Gacha_x10_open";

        //castle screen
        public const string Castle_BuildingAppear = "event:/Animations/Castle_Scenes/Building_Appear";
        public const string Castle_BuildingUpgrade = "event:/Animations/Castle_Scenes/Building_Upgrade";
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
        private static List<FMODEvent> instances = new List<FMODEvent>();
        private static List<EventDescription> localEvents = new List<EventDescription>();

        public static IEnumerator<int> PreloadBanks()
        {
            RuntimeManager.LoadBank("BGM");
            RuntimeManager.LoadBank("Master");
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

        public static FMODEvent GetEventInstance(string eventPath, string bankId = null)
        {//for music
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

        public static void PlayOneShot(string eventPath)
        {//for sfx
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