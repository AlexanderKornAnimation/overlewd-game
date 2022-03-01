using System;
using System.Collections.Generic;
using System.Linq;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Overlewd
{
    public static class SoundPath
    {
        //buttons
        public const string UI_CastleScreenButtons = "event:/UI/Buttons/Castle_Screen/Basic_menu_click";
        public const string UI_GenericButtonClick = "event:/UI/Buttons/Generic/Button_click";
        public const string UI_DialogNextButtonClick = "event:/UI/Buttons/Dialogue/Text_field_click_next";
        public const string UI_DefeatPopupHaremButtonClick = "event:/UI/PopUps/Battle/Boosts/Matriarch_Boost";
        public const string UI_FreeBuildButton = "event:/UI/Buttons/Building/Build_8_hours_button";
        public const string UI_FreeSpellLearnButton = "event:/UI/Buttons/Learn_Spell/Learn_Spell_8_hours";

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
    }

    public class SoundInstance
    {
        private EventInstance instance;
        public string key { get; private set; }
        public bool release { get; private set; } = false;

        private SoundInstance(string eventPath, string key = null)
        {
            instance = RuntimeManager.CreateInstance(eventPath);
            this.key = key ?? eventPath;
            instance.start();
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
            release = true;
            SoundManager.ClearReleased();
        }

        public static SoundInstance GetInstance(string eventPath, string key = null)
        {
            EventDescription eventDesc;
            RuntimeManager.StudioSystem.getEvent(eventPath, out eventDesc);
            return eventDesc.isValid() ? new SoundInstance(eventPath, key) : null;
        }
    }

    public static class SoundManager
    {
        private static List<SoundInstance> instances = new List<SoundInstance>();

        private static Bus animationBus;
        private static Bus uiBus;

        public static void Initialize()
        {
            animationBus = RuntimeManager.GetBus("bus:/Animations");
            uiBus = RuntimeManager.GetBus("bus:/UI");
        }

        public static SoundInstance GetSoundInstance(string eventPath, string key = null)
        {
            var inst = instances.Find(item => item.key == (key ?? eventPath));
            if (inst != null)
            {
                return inst;
            }

            var newInst = SoundInstance.GetInstance(eventPath, key);
            if (newInst != null)
            {
                instances.Add(newInst);
                return newInst;
            }

            return null;
        }
        
        public static void Stop(string key)
        {
            var inst = instances.Find(item => item.key == key);
            inst?.Stop();
            ClearReleased();
        }

        public static void Pause(string key)
        {
            var findInst = instances.Find(item => item.key == key);
            findInst?.Pause();
        }

        public static void Play(string key)
        {
            var findInst = instances.Find(item => item.key == key);
            findInst?.Play();
        }

        public static void StopAll()
        {
            foreach (var inst in instances.ToList())
            {
                inst.Stop();
            }
            ClearReleased();
        }

        public static void ClearReleased()
        {
            instances.RemoveAll(item => item.release);
        }

        public static void OnMusicVolumeChanged(float value)
        {
        }

        public static void OnAnimationVolumeChanged(float value)
        {
            animationBus.setVolume(value);
        }

        public static void PlayOneShoot(string soundEventPath)
        {
            RuntimeManager.PlayOneShot(soundEventPath);
        }

        public static void OnSoundVolumeChanged(float value)
        {
            uiBus.setVolume(value);
        }
    }
}