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
                public const string CastleScreenButtons = "event:/UI/Buttons/Basic_menu_click";
                public const string CastleWindowSlideOn = "event:/UI/Windows/Window_slide_on";
                public const string CastleWindowSlideOff = "event:/UI/Windows/Window_slide_off";
                public const string SidebarOverlayOn = "event:/UI/Windows/Deeds_slide_on";
                public const string SidebarOverlayOff = "event:/UI/Windows/Deeds_slide_off";
            }

            public static class Animations
            {
                public const string CutInCumshot = "event:/Animations/Sex_Scenes/1_Ulvi_BJ/add_cumshot";
                public const string CutInLick = "event:/Animations/Sex_Scenes/1_Ulvi_BJ/add_lick";
                public const string MainScene = "event:/Animations/Sex_Scenes/1_Ulvi_BJ/main_scene";
            }
        }

        private static Dictionary<string, EventInstance> eventInstances = new Dictionary<string, EventInstance>();

        private static void Stop(string keyName, bool allowFade)
        {
            if (allowFade)
            {
                eventInstances[keyName].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                eventInstances[keyName].release();
                return;
            }

            eventInstances[keyName].stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstances[keyName].release();
        }

        private static void Play(string keyName)
        {
            if (keyName == null)
                return;

            eventInstances[keyName].start();
        }

        public static void CreateEventInstance(string keyName, string eventPath)
        {
            if (keyName == null)
                return;

            if (!eventInstances.ContainsKey(keyName))
            {
                eventInstances.Add(keyName, RuntimeManager.CreateInstance(eventPath));
                Play(keyName);
            }
        }


        public static void SetPause(string keyName, bool pause)
        {
            if (keyName == null)
                return;
            
            if (eventInstances.ContainsKey(keyName))
                eventInstances[keyName].setPaused(pause);
        }

        public static void StopAllInstances(bool allowFade)
        {
            foreach (var instance in eventInstances)
            {
                Stop(instance.Key, allowFade);
            }
        }

        public static void OnMusicVolumeChanged(float value)
        {
            
        }

        public static void OnAnimationVolumeChanged(float value)
        {
            var bus = RuntimeManager.GetBus("bus:/Animations");

            bus.setVolume(value);
        }
        
        //Sound
        public static void PlayOneShoot(string soundEventPath)
        {
            RuntimeManager.PlayOneShot(soundEventPath);
        }
        
        public static void OnSoundVolumeChanged(float value)
        {
            var bus = RuntimeManager.GetBus("bus:/UI");

            bus.setVolume(value);
        }
    }
}