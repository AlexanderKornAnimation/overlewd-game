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
    public static class SoundManager
    {
        public static class SoundPath
        {
            public static class UI
            {
                public const string CastleScreenButtons = "event:/UI/Buttons/Castle_Screen/Basic_menu_click";
                public const string CastleWindowSlideOn = "event:/UI/Windows/Castle_Screen/Window_slide_on";
                public const string CastleWindowSlideOff = "event:/UI/Windows/Castle_Screen/Window_slide_off";
                public const string SidebarOverlayOn = "event:/UI/Windows/Castle_Screen/Deeds_slide_on";
                public const string SidebarOverlayOff = "event:/UI/Windows/Castle_Screen/Deeds_slide_off";
                public const string ButtonClick = "event:/UI/Buttons/Generic/Button_click";
                public const string WindowSlideOn = "event:/UI/Windows/Generic/Window_slide_on";
                public const string WindowSlideOff = "event:/UI/Windows/Generic/Window_slide_off";
                public const string PopupSlideOn = "event:/UI/Windows/Generic/Window_popup_slide";
            }

            public static class Animations
            {
                public const string CutInCumshot = "event:/Animations/Sex_Scenes/1_Ulvi_BJ/add_cumshot";
                public const string CutInLick = "event:/Animations/Sex_Scenes/1_Ulvi_BJ/add_lick";
                public const string MainScene = "event:/Animations/Sex_Scenes/1_Ulvi_BJ/main_scene";
            }
        }

        private static Dictionary<string, EventInstance> eventInstances = new Dictionary<string, EventInstance>();
        
        private static Bus animationBus;
        private static Bus uiBus;

        private static void Stop(string keyName, bool allowFade)
        {
            if (allowFade)
            {
                eventInstances[keyName].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                eventInstances[keyName].release();
                eventInstances.Remove(keyName);
                return;
            }

            eventInstances[keyName].stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
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