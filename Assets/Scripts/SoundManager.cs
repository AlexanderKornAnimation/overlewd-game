using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using Debug = UnityEngine.Debug;

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

        //sex 1
        public const string SexScene1_CutInCumshot = "event:/Animations/Sex_Scenes/1_Ulvi_BJ/add_cumshot";
        public const string SexScene1_CutInLick = "event:/Animations/Sex_Scenes/1_Ulvi_BJ/add_lick";
        public const string SexScene1_MainScene = "event:/Animations/Sex_Scenes/1_Ulvi_BJ/main_scene";

        //sex 2
        public const string SexScene2_CutInBeads = "event:/Animations/Sex_Scenes/2_Ulvi_Cowgirl/1_scene_add-beads";
        public const string SexScene2_CutInCreamPie = "event:/Animations/Sex_Scenes/2_Ulvi_Cowgirl/1_scene_add-cum";
        public const string SexScene2_MainScene = "event:/Animations/Sex_Scenes/2_Ulvi_Cowgirl/1_scene";
        public const string SexScene2_FinalScene = "event:/Animations/Sex_Scenes/2_Ulvi_Cowgirl/2_scene";
        
        //dialogue 1
        public const string Dialogue1_CutInUlviScratch = "event:/Animations/Sex_Scenes/Chapter-1_Cut_Ins/Ulvi_Head_Pat";
    }

    public class FMODBank
    {
        private Bank bank;

        private FMODBank(string path)
        {
            RuntimeManager.StudioSystem.loadBankFile(path,
                LOAD_BANK_FLAGS.NORMAL,
                out bank);
        }

        public void Unload()
        {
            bank.unload();
        }

        public static FMODBank LoadFromFile(string bankPath)
        {
            return new FMODBank(bankPath);
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
            return eventDesc.isValid() ? new FMODEvent(eventPath) : null;
        }
    }

    public static class SoundManager
    {
        private static List<FMODEvent> instances = new List<FMODEvent>();

        public static FMODEvent GetEventInstance(string eventPath, string bankId = null)
        {
            if (!String.IsNullOrEmpty(bankId))
            {
                ResourceManager.LoadFMODBank(bankId);
            }

            var inst = instances.Find(item => (item.path == eventPath) && item.IsPlaying());
            if (inst != null)
            {
                return inst;
            }

            var newInst = FMODEvent.GetInstance(eventPath);
            if (newInst != null)
            {
                instances.Add(newInst);
                return newInst;
            }

            return null;
        }

        public static void PlayOneShot(string eventPath)
        {
            RuntimeManager.PlayOneShot(eventPath);
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