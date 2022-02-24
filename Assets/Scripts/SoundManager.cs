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
            public static class UI
            {
                //buttons
                public const string CastleScreenButtons = "event:/UI/Buttons/Castle_Screen/Basic_menu_click";
                public const string GenericButtonClick = "event:/UI/Buttons/Generic/Button_click";
                public const string DialogNextButtonClick = "event:/UI/Buttons/Dialogue/Text_field_click_next";
                public const string DefeatPopupHaremButtonClick = "event:/UI/PopUps/Battle/Boosts/Matriarch_Boost";
                public const string FreeBuildButton = "event:/UI/Buttons/Building/Build_8_hours_button";
                public const string FreeSpellLearnButton = "event:/UI/Buttons/Learn_Spell/Learn_Spell_8_hours";
                
                //screens
                public const string CastleWindowShow = "event:/UI/Windows/Castle_Screen/Window_slide_on";
                public const string CastleWindowHide = "event:/UI/Windows/Castle_Screen/Window_slide_off";
                public const string GenericWindowShow = "event:/UI/Windows/Generic/Window_slide_on";
                public const string GenericWindowHide = "event:/UI/Windows/Generic/Window_slide_off";
                public const string BattleScreenShow = "event:/UI/Windows/Battle_Screen/Battle_Screen_Transition";
                public const string PortalScreenFTUE = "event:/Animations/Chapter_Scenes/Chapter-1/Portals_Chamber_Build";
                
                //overlays
                public const string SidebarOverlayShow = "event:/UI/Windows/Castle_Screen/Deeds_slide_on";
                public const string SidebarOverlayHide = "event:/UI/Windows/Castle_Screen/Deeds_slide_off";
                
                //popups & nootifications
                public const string GenericPopupShow = "event:/UI/Windows/Generic/Window_popup_slide";
                public const string GenericDialogNotificationShow = "event:/UI/PopUps/Text/Generic_Text_Window_PopUp";
            }

            public static class Animations
            {
                public static class FirstSex
                {
                    public const string CutInCumshot = "event:/Animations/Sex_Scenes/1_Ulvi_BJ/add_cumshot";
                    public const string CutInLick = "event:/Animations/Sex_Scenes/1_Ulvi_BJ/add_lick";
                    public const string MainSexScene1 = "event:/Animations/Sex_Scenes/1_Ulvi_BJ/main_scene";
                }

                public static class SecondSex
                {
                    public const string CutInBeads = "event:/Animations/Sex_Scenes/2_Ulvi_Cowgirl/1_scene_add-beads";
                    public const string CutInCreamPie = "event:/Animations/Sex_Scenes/2_Ulvi_Cowgirl/1_scene_add-cum";
                    public const string MainSexScene2 = "event:/Animations/Sex_Scenes/2_Ulvi_Cowgirl/1_scene";
                    public const string FinalSexScene2 = "event:/Animations/Sex_Scenes/2_Ulvi_Cowgirl/2_scene";
                }
            }
        }
    
    public static class SoundManager
    {
        

        private static Dictionary<string, EventInstance> eventInstances = new Dictionary<string, EventInstance>();
        
        private static Bus animationBus;
        private static Bus uiBus;

        private static void Stop(string keyName, bool allowFade)
        {
            var stopMode = allowFade ? FMOD.Studio.STOP_MODE.ALLOWFADEOUT : FMOD.Studio.STOP_MODE.IMMEDIATE;
            eventInstances[keyName].stop(stopMode);
            eventInstances[keyName].release();
            eventInstances.Remove(keyName);
        }

        private static void Play(string keyName)
        {
            if (String.IsNullOrWhiteSpace(keyName))
            {
                Debug.LogWarning("Key is null or empty. Instance isn't play");
                return;
            }
            
            eventInstances[keyName].start();
        }

        public static void Initialize()
        {
            animationBus = RuntimeManager.GetBus("bus:/Animations");
            uiBus = RuntimeManager.GetBus("bus:/UI");
        }
        
        public static EventInstance GetInstanceByKey(string keyName)
        {
            if (String.IsNullOrWhiteSpace(keyName))
            {
                Debug.LogWarning("Key is null or empty. Returned default");
                return default;
            }
            
            if (eventInstances.ContainsKey(keyName))
                return eventInstances[keyName];

            return default;
        }

        public static EventInstance CreateEventInstance(string eventPath)
        {
            if (String.IsNullOrWhiteSpace(eventPath))
            {
                Debug.LogWarning("Path is null or empty. Returned default");
                return default;
            }

            if (!eventInstances.ContainsKey(eventPath))
            {
                eventInstances.Add(eventPath, RuntimeManager.CreateInstance(eventPath));
                Play(eventPath);
                return eventInstances[eventPath];
            }

            return eventInstances[eventPath];
        }

        public static EventInstance CreateEventInstance(string keyName, string eventPath)
        {
            if (String.IsNullOrWhiteSpace(keyName) || String.IsNullOrWhiteSpace(eventPath))
            {
                Debug.LogWarning("key or path is null or empty. Returned default");
                return default;
            }

            if (!eventInstances.ContainsKey(keyName))
            {
                eventInstances.Add(keyName, RuntimeManager.CreateInstance(eventPath));
                Play(keyName);
            }

            return eventInstances[keyName];
        }

        public static void SetPause(string keyName, bool pause)
        {
            if (String.IsNullOrWhiteSpace(keyName))
            {
                Debug.LogWarning("Key is null or empty. Pause isn't set");
                return;
            }

            if (eventInstances.ContainsKey(keyName))
                eventInstances[keyName].setPaused(pause);
        }

        public static void StopAllInstances(bool allowFade)
        {
            foreach (var instance in eventInstances.ToList())
            {
                Stop(instance.Key, allowFade);
            }
            
            eventInstances.Clear();
        }

        public static void OnMusicVolumeChanged(float value)
        {
        }

        public static void OnAnimationVolumeChanged(float value)
        {
            animationBus.setVolume(value);
        }

        //Sound
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